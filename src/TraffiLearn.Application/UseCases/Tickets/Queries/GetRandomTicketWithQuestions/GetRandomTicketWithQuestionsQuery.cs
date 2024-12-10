using MediatR;
using TraffiLearn.Application.UseCases.Tickets.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Tickets.Queries.GetRandomTicketWithQuestions
{
    public sealed record GetRandomTicketWithQuestionsQuery
        : IRequest<Result<TicketWithQuestionsResponse>>;
}
