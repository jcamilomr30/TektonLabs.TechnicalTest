using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using TektonLabs.TechnicalTest.Core.Dtos;
using TektonLabs.TechnicalTest.Core.Extensions;
using TektonLabs.TechnicalTest.Core.Interfaces;
using TektonLabs.TechnicalTest.Domain.IRepository;
using System.Threading.Tasks;
using TektonLabs.TechnicalTest.Infraestructure.HttpClients;
using TektonLabs.TechnicalTest.Core.Dtos.Response;
using TektonLabs.TechnicalTest.Infraestructure.Exceptions;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace TektonLabs.TechnicalTest.Core.Services
{
    public class ProductService : Service<int, Domain.Entities.Product, Dtos.ProductDto>, IProductService
    {
        private readonly IProductRepository ProductRepository;
        private readonly IDiscountManagerClient discountManagerClient;
        private IValidator<CreateProductDto> validatorCreate;
        private IValidator<UpdateProductDto> validatorUpdate;
        private readonly IMapper mapper;
        private readonly ICacheProvider cacheProvider;
        private readonly IConfiguration configuration;
        protected const int DefaultCacheTime = 5;
        protected const string StatusCacheKey = "status";

        public ProductService(IProductRepository ProductRepository
            , IDiscountManagerClient discountManagerClient
            , IValidator<CreateProductDto> validatorCreate
            , IValidator<UpdateProductDto> validatorUpdate
            , ICacheProvider cacheProvider
            , IMapper mapper
            , IConfiguration configuration) : base(ProductRepository, mapper)
        {
            this.ProductRepository = ProductRepository;
            this.discountManagerClient = discountManagerClient;
            this.validatorCreate = validatorCreate;
            this.validatorUpdate = validatorUpdate;
            this.mapper = mapper;
            this.cacheProvider = cacheProvider;
            this.configuration = configuration;
        }

        public async Task<ProductDto> GetById(int id)
        {
            var product = ProductRepository.FindById(id);
            var productResponse = Mapper.Map<ProductDto>(product);
            await GetDiscount(productResponse);
            productResponse.StatusName = GetStatusNameFromCache(productResponse.Status);
            return productResponse;
        }

        private async Task GetDiscount(ProductDto product)
        {
            var response = await discountManagerClient.GetAsync<DiscountResponse>($"discount/{product.Id}");
            product.Discount = response != default ? response.Discount : 0;
            product.FinalPrice = product.Price * (100 - product.Discount) / 100;
        }

        private string GetStatusNameFromCache(int status)
        {
            if (!int.TryParse(configuration["CacheTimeStatus"], out int cacheTime))
            {
                cacheTime = DefaultCacheTime;
            }

            Dictionary<int, string> dictionaryCache;
            if (!cacheProvider.TryGetValue(StatusCacheKey, out dictionaryCache))
            {
                dictionaryCache = cacheProvider.Set(StatusCacheKey
                                    , new Dictionary<int, string>()
                                    {
                                        { 1, "Active"  },
                                        { 0, "Inactive"  }
                                    }
                                    , cacheTime);
            }
            return dictionaryCache[status];
        }


        public async Task<int> Create(CreateProductDto ProductDto)
        {
            await validatorCreate.ValidateOrThrowValidationExceptionAsync(ProductDto);

            var mapped = Mapper.Map<Domain.Entities.Product>(ProductDto);

            return ProductRepository.Add(mapped).Id;
        }

        public async Task Update(UpdateProductDto ProductDto, int id)
        {
            await validatorUpdate.ValidateOrThrowValidationExceptionAsync(ProductDto);

            var Product = ProductRepository.FindById(id);
            if (Product == null)
                throw new NotFoundException();

            Mapper.Map(ProductDto, Product);
            await ProductRepository.UpdateAsync(Product);
        }
    }
}
