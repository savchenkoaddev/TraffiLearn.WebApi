using MediatR;
using TraffiLearn.Application.Tickets.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Tickets.Queries.GetQuestionTickets
{
    public sealed record GetQuestionTicketsQuery(
        Guid? QuestionId) : IRequest<Result<IEnumerable<TicketResponse>>>;
}
