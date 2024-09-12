namespace TraffiLearn.WebAPI.Swagger
{
    internal sealed class ServerErrorResponseExample
    {
        private ServerErrorResponseExample()
        { }

        public string? Type => "https://tools.ietf.org/html/rfc7231#section-6.6.1";

        public string? Title => "Internal Server Error";

        public int Status => 500;
    }
}
