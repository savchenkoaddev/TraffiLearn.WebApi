namespace TraffiLearn.Infrastructure.External.GroqAI.DTO
{
    public sealed record GroqApiRequest(
        List<GroqApiMessage> Messages,
        string Model);


    public sealed record GroqApiMessage(
        string Role,
        string Content);
}