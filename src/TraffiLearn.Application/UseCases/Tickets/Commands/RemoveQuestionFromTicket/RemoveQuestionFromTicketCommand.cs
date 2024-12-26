using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Tickets.Commands.RemoveQuestionFromTicket
{
    public sealed record RemoveQuestionFromTicketCommand(
        Guid QuestionId,
        Guid TicketId) : IRequest<Result>;
}
