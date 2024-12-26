using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Topics.Commands.RemoveQuestionFromTopic
{
    public sealed record RemoveQuestionFromTopicCommand(
        Guid QuestionId,
        Guid TopicId) : IRequest<Result>;
}
