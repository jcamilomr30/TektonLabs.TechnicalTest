using System.Collections.Generic;

namespace TektonLabs.TechnicalTest.Infraestructure.Exceptions
{
    public class ValidationProblemDetails
    {
        public int Status { get; set; }

        public string Detail { get; set; }

        public IDictionary<string, ErrorDetails[]> Errors { get; set; }

        public ValidationProblemDetails(IDictionary<string, ErrorDetails[]> errors)
        {
            Errors = errors;
        }
    }
}
