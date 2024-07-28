using MediatR;

namespace TraffiLearn.Application.Commands.Questions.RemoveTopicForQuestion
{
    public sealed record RemoveTopicForQuestionCommand(
        Guid TopicId,
        Guid QuestionId) : IRequest;
}
