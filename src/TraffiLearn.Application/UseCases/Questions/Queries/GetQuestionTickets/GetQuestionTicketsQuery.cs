using MediatR;
using TraffiLearn.Application.UseCases.Tickets.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Questions.Queries.GetQuestionTickets
{
    public sealed record GetQuestionTicketsQuery(
        Guid? QuestionId) : IRequest<Result<IEnumerable<TicketResponse>>>;
}
