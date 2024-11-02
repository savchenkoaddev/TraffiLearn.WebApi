using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Tickets.Commands.AddQuestionToTicket
{
    public sealed record AddQuestionToTicketCommand(
        Guid? QuestionId,
        Guid? TicketId) : IRequest<Result>;
}
