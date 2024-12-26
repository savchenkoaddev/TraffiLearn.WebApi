using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Tickets.Commands.AddQuestionToTicket
{
    public sealed record AddQuestionToTicketCommand(
        Guid QuestionId,
        Guid TicketId) : IRequest<Result>;
}
