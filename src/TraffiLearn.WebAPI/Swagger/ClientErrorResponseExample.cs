namespace TraffiLearn.WebAPI.Swagger
{
    internal sealed class ClientErrorResponseExample
    {
        private ClientErrorResponseExample()
        { }

        public string? Type { get; init; }

        public string? Title { get; init; }

        public int Status { get; init; }

        public ErrorResponseExample[]? Errors { get; init; }
    }
}
