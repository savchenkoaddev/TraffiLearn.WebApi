using MediatR;
using TraffiLearn.Application.UseCases.Questions.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Topics.Queries.GetTopicQuestions
{
    public sealed record GetTopicQuestionsQuery(
        Guid TopicId) : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
