using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Questions.Commands.Delete
{
    public sealed record DeleteQuestionCommand(
        Guid QuestionId) : IRequest<Result>;
}
