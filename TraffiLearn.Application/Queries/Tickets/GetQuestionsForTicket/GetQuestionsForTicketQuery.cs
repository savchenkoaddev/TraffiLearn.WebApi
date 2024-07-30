using MediatR;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Tickets.GetQuestionsForTicket
{
    public sealed record GetQuestionsForTicketQuery(
        Guid? TicketId) : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
