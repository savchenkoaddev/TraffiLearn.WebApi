using MediatR;
using TraffiLearn.Application.Tickets.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Tickets.Queries.GetRandomTicketWithQuestions
{
    public sealed record GetRandomTicketWithQuestionsQuery
        : IRequest<Result<TicketWithQuestionsResponse>>;
}
