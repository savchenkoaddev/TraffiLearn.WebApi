namespace TraffiLearn.Infrastructure.External.GroqAI.DTO
{
    public sealed record GroqAIResponse(
        List<GroqAIChoice> Choices);

    public sealed record GroqAIChoice(
        GroqAIMessage Message);
}