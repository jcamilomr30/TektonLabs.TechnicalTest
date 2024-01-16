using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TektonLabs.TechnicalTest.Infraestructure.Exceptions;

namespace TektonLabs.TechnicalTest.Core.Extensions
{
    public static class ValidatorExtensions
    {
        public static async Task ValidateOrThrowValidationExceptionAsync<T>(this IValidator<T> instance, T dto, CancellationToken cancellation = default)
        {
            var validationResult = await instance.ValidateAsync(dto, cancellation);

            if (!validationResult.IsValid)
                throw new BusinessValidationException(string.Join(',', validationResult.Errors));
        }

        public static void ValidateOrThrowValidationException<T>(this IValidator<T> instance, T dto)
        {
            var validationResult = instance.Validate(dto);

            if (!validationResult.IsValid)
                throw new BusinessValidationException(string.Join(',', validationResult.Errors));
        }
    }
}
