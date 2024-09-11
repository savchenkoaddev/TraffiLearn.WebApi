using TraffiLearn.Application.AI.DTO;

namespace TraffiLearn.Application.Abstractions.AI
{
    public interface IAIService
    {
        Task<AITextResponse> SendTextQueryAsync(
            AITextRequest? request,
            CancellationToken cancellationToken = default);
    }
}