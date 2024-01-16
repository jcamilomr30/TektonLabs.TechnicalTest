namespace TektonLabs.TechnicalTest.Infraestructure.Exceptions
{
    public class ErrorDetails
    {
        public ErrorDetails(string code, string message)
        {
            Message = message;
            Code = code;
        }

        public string Message { get; private set; }
        public string Code { get; private set; }
    }
}
