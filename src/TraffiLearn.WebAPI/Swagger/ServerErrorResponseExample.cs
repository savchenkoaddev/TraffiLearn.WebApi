namespace TraffiLearn.WebAPI.Swagger
{
    internal sealed class ServerErrorResponseExample
    {
        private ServerErrorResponseExample()
        { }

        public string Type => "https://tools.ietf.org/html/rfc7231#section-6.6.1";

        public string Title => "Internal Server Error";

        public int Status => 500;

        public string? Instance { get; init; }

        public string? RequestId { get; init; }

        public string? TraceId { get; init; }
    }
}
