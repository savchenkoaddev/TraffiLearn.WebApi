using MediatR;
using TraffiLearn.Application.AI.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Questions.Queries.GetAIQuestionExplanation
{
    public sealed record GetAIQuestionExplanationQuery(
        Guid QuestionId) : IRequest<Result<AITextResponse>>;
}