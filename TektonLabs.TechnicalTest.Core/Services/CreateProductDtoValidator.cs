using FluentValidation;
using TektonLabs.TechnicalTest.Core.Dtos;
using TektonLabs.TechnicalTest.Domain.IRepository;

namespace TektonLabs.TechnicalTest.Core.Services
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        private readonly IProductRepository productRepository;


        public CreateProductDtoValidator(IProductRepository productRepository)
        {
            this.productRepository = productRepository;

            RuleFor(e => e.ProductId).NotNull().WithMessage("ProductId must have a value");
            RuleFor(e => e.Status).InclusiveBetween(0,1).WithMessage("Status must be 0 or 1");
            RuleFor(e => e.Stock).GreaterThanOrEqualTo(0).WithMessage("Stars must Greater Than Or Equal To 0");
            RuleFor(e => e).Must(ValidateIfProductExists)
                .WithMessage("Product exists");

        }

        private bool ValidateIfProductExists(CreateProductDto createProductDto)
        {
            return !(productRepository.FindById(createProductDto.ProductId.Value) != null);
        }
    }
}
