using MediatR;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Questions.GetTicketQuestions
{
    public sealed record GetTicketQuestionsQuery(
        Guid? TicketId) : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
