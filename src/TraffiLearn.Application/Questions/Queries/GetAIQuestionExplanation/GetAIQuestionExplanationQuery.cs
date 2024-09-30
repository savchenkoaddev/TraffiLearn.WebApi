using MediatR;
using TraffiLearn.Application.AI.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Questions.Queries.GetAIQuestionExplanation
{
    public sealed record GetAIQuestionExplanationQuery(
        Guid? QuestionId) : IRequest<Result<AITextResponse>>;
}