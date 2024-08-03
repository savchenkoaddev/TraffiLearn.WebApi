using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Tickets.RemoveQuestionFromTicket
{
    public sealed record RemoveQuestionFromTicketCommand(
        Guid? QuestionId,
        Guid? TicketId) : IRequest<Result>;
}
