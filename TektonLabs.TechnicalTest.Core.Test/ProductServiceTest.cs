using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TektonLabs.TechnicalTest.Core.Dtos;
using TektonLabs.TechnicalTest.Core.Dtos.Response;
using TektonLabs.TechnicalTest.Core.Interfaces;
using TektonLabs.TechnicalTest.Core.Services;
using TektonLabs.TechnicalTest.Domain.Entities;
using TektonLabs.TechnicalTest.Domain.IRepository;
using TektonLabs.TechnicalTest.Infraestructure.HttpClients;
using Xunit;

namespace TektonLabs.TechnicalTest.Core.Test
{
    public class ProductServiceTest
    {
        private readonly IProductRepository ProductRepository;
        private readonly IDiscountManagerClient discountManagerClient;
        private IValidator<CreateProductDto> validatorCreate;
        private IValidator<UpdateProductDto> validatorUpdate;
        private readonly IMapper mapper;
        private readonly ICacheProvider cacheProvider;
        private readonly IConfiguration configuration;

        public ProductServiceTest()
        {
            ProductRepository = Mock.Of<IProductRepository>();
            discountManagerClient = Mock.Of<IDiscountManagerClient>();
            validatorCreate = Mock.Of<IValidator<CreateProductDto>>();
            validatorUpdate = Mock.Of<IValidator<UpdateProductDto>>();
            cacheProvider = Mock.Of<ICacheProvider>();
            configuration = Mock.Of<IConfiguration>();
            mapper = Mock.Of<IMapper>();

            this.Target = new ProductService(ProductRepository
                , discountManagerClient
                , validatorCreate
                , validatorUpdate
                , cacheProvider
                , mapper
                , configuration
                );
        }

        private IProductService Target { get; set; }

        public class Method_GetProductAsync : ProductServiceTest
        {
            public Method_GetProductAsync()
            {
                var id = 1;
                Mock.Get(discountManagerClient)
                    .Setup(edr => edr.GetAsync<DiscountResponse>(It.IsAny<string>()))
                    .ReturnsAsync(new DiscountResponse
                    {
                        Discount = 5
                    });

                Dictionary<int, string> dictionaryCache= new Dictionary<int, string>()
                                {
                                    { 1, "Active" },
                                    { 0, "Inactive" }
                                };

                Mock.Get(cacheProvider)
                    .Setup(edr => edr.TryGetValue(It.IsAny<string>(), out dictionaryCache))
                    .Returns(true);

                Mock.Get(mapper)
                  .Setup(x => x.Map<ProductDto>(It.IsAny<Product>()))
                  .Returns(new ProductDto
                  {
                      Id = id,
                      Discount = 5,
                      Price = 5000
                  });

            }

            [Fact]
            public async Task GetDiscount_Is_Called_Once_With_Specific_Id()
            {
                //Arrange


                //Act
                await Target.GetById(It.IsAny<int>());

                //Assert
                Mock.Get(discountManagerClient)
                    .Verify(edr => edr.GetAsync<DiscountResponse>(It.IsAny<string>()), Times.Once);
            }

            [Fact]
            public async Task FinalPrice_WhenGetProductById()
            {
                //Arrange


                //Act
                var result = await Target.GetById(It.IsAny<int>());

                //Assert
                Assert.NotNull(result);
                Assert.Equal(result.FinalPrice, result.Price - (result.Price * result.Discount / 100));
            }

            [Fact]
            public async void GetProduct_Method_Returns_ProductDto_Type()
            {
                // Act
                var result = await Target.GetById(It.IsAny<int>());

                // Assert
                Assert.IsType<ProductDto>(result);
            }
        }

    }
}