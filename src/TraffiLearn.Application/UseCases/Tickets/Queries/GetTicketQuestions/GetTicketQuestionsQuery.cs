using MediatR;
using TraffiLearn.Application.UseCases.Questions.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Tickets.Queries.GetTicketQuestions
{
    public sealed record GetTicketQuestionsQuery(
        Guid TicketId) : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
