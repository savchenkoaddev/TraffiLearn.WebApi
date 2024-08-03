using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Questions.Delete
{
    public sealed record DeleteQuestionCommand(
        Guid? QuestionId) : IRequest<Result>;
}
