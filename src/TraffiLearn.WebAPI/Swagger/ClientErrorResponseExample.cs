namespace TraffiLearn.WebAPI.Swagger
{
    internal sealed class ClientErrorResponseExample
    {
        private ClientErrorResponseExample()
        { }

        public string? Type { get; init; }

        public string? Title { get; init; }

        public int Status { get; init; }

        public string? Detail { get; init; }

        public string? Instance { get; init; }

        public string? RequestId { get; init; }

        public string? TraceId { get; init; }

        public ErrorResponseExample[]? Errors { get; init; }
    }
}
