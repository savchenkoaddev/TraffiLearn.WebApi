using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Topics.Commands.AddQuestionToTopic
{
    public sealed record AddQuestionToTopicCommand(
        Guid? QuestionId,
        Guid? TopicId) : IRequest<Result>;
}
