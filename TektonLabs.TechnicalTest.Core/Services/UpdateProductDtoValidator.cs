using FluentValidation;
using TektonLabs.TechnicalTest.Core.Dtos;

namespace TektonLabs.TechnicalTest.Core.Services
{
    public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(e => e.Name).NotNull().WithMessage("Name must have a value");
            RuleFor(e => e.Status).InclusiveBetween(0, 1).WithMessage("Status must be 0 or 1");
            RuleFor(e => e.Stock).GreaterThanOrEqualTo(0).WithMessage("Stars must Greater Than Or Equal To 0");
        }
    }
}
