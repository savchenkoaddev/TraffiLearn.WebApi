using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Tickets.Commands.RemoveQuestionFromTicket
{
    public sealed record RemoveQuestionFromTicketCommand(
        Guid? QuestionId,
        Guid? TicketId) : IRequest<Result>;
}
