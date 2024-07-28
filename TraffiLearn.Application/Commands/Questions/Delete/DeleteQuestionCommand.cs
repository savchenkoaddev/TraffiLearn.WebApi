using MediatR;

namespace TraffiLearn.Application.Commands.Questions.Delete
{
    public sealed record DeleteQuestionCommand(Guid QuestionId) : IRequest;
}
