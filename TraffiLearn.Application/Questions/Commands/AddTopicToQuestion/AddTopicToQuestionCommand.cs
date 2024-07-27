using MediatR;

namespace TraffiLearn.Application.Questions.Commands.AddTopicToQuestion
{
    public sealed record AddTopicToQuestionCommand(
        Guid TopicId,
        Guid QuestionId) : IRequest;
}
