using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace TektonLabs.TechnicalTest.Infraestructure.Exceptions
{
    [Serializable]
    public class BusinessValidationException : Exception
    {
        public ValidationProblemDetails Details { get; set; }

        public override string Message => "One or more validation errors occurred.";

        public BusinessValidationException(string property, params ErrorDetails[] failures)
        {
            var errors = new Dictionary<string, ErrorDetails[]>()
            {
                { property, failures }
            };

            Details = new ValidationProblemDetails(errors)
            {
                Status = (int)HttpStatusCode.BadRequest,
                Detail = Message
            };
        }

        public BusinessValidationException(IEnumerable<ValidationFailure> validationFailures)
        {
            var errors = validationFailures
                    .GroupBy(v => v.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(v => new ErrorDetails(v.ErrorCode, v.ErrorMessage))
                    .ToArray());

            Details = new ValidationProblemDetails(errors)
            {
                Status = (int)HttpStatusCode.BadRequest,
                Detail = Message
            };
        }
        protected BusinessValidationException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
        }

    }
}
