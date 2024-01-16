using System;
using System.Net;
using System.Runtime.Serialization;

namespace TektonLabs.TechnicalTest.Infraestructure.Exceptions
{
    [Serializable]
    public class NotFoundException : Exception
    {
        public HttpStatusCode Status => HttpStatusCode.NotFound;

        public override string Message => "Entity not found.";

        public NotFoundException() : base()
        {

        }

        protected NotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        {
        }
    }
}
