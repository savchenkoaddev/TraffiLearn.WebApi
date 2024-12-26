namespace TraffiLearn.Infrastructure.External.GroqAI.DTO
{
    public sealed record GroqApiMessage(
        string Role,
        string Content);
}