using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Questions.AddTicketToQuestion
{
    public sealed record AddTicketToQuestionCommand(
        Guid? TicketId,
        Guid? QuestionId) : IRequest<Result>;
}
