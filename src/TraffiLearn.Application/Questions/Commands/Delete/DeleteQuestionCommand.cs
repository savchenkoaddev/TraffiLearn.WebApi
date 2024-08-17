using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Questions.Commands.Delete
{
    public sealed record DeleteQuestionCommand(
        Guid? QuestionId) : IRequest<Result>;
}
