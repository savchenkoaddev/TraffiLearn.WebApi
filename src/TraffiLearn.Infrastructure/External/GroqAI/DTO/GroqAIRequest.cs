namespace TraffiLearn.Infrastructure.External.GroqAI.DTO
{
    public sealed record GroqAIRequest(
        List<GroqAIMessage> Messages,
        string Model);


    public sealed record GroqAIMessage(
        string Role,
        string Content);
}