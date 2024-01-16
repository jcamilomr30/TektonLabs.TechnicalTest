using System;
using System.Net;

namespace TektonLabs.TechnicalTest.Infraestructure.Exceptions
{
    [Serializable]
    public class NotContentException : Exception
    {
        public int Status { get; set; }

        public override string Message { get; }

        public NotContentException(string message)
        {
            const string defaultMessage = "One or more validation errors occurred.";

            Status = (int)HttpStatusCode.NoContent;
            Message = string.IsNullOrEmpty(message) ? defaultMessage : message;
        }
    }
}
