namespace TraffiLearn.WebAPI.Swagger
{
    internal sealed class ClientErrorResponseExample
    {
        private ClientErrorResponseExample()
        { }

        public string? Type { get; set; }

        public string? Title { get; set; }

        public int Status { get; set; }

        public ErrorResponseExample[]? Errors { get; set; }
    }
}
