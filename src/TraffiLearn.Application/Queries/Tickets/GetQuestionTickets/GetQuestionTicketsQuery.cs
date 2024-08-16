using MediatR;
using TraffiLearn.Application.DTO.Tickets;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Tickets.GetQuestionTickets
{
    public sealed record GetQuestionTicketsQuery(
        Guid? QuestionId) : IRequest<Result<IEnumerable<TicketResponse>>>;
}
