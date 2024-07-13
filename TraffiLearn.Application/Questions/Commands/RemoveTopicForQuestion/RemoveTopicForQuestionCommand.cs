using MediatR;

namespace TraffiLearn.Application.Questions.Commands.RemoveTopicForQuestion
{
    public sealed record RemoveTopicForQuestionCommand(
        Guid? TopicId,
        Guid? QuestionId) : IRequest;
}
