using MediatR;

namespace TraffiLearn.Application.Questions.Commands.DeleteQuestion
{
    public sealed record DeleteQuestionCommand(
        Guid? QuestionId) : IRequest;
}
