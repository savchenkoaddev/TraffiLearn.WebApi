using MediatR;
using TraffiLearn.Application.DTO.Tickets;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Tickets.GetRandomTicketWithQuestions
{
    public sealed record GetRandomTicketWithQuestionsQuery 
        : IRequest<Result<TicketWithQuestionsResponse>>;
}
