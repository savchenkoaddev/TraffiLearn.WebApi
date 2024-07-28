using MediatR;

namespace TraffiLearn.Application.Commands.Questions.AddTopicToQuestion
{
    public sealed record AddTopicToQuestionCommand(
        Guid TopicId,
        Guid QuestionId) : IRequest;
}
