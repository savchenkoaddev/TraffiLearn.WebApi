using FluentValidation;

namespace TraffiLearn.Application.Tickets.Queries.GetTicketQuestions
{
    internal sealed class GetTicketQuestionsQueryValidator
        : AbstractValidator<GetTicketQuestionsQuery>
    {
        public GetTicketQuestionsQueryValidator()
        {
            RuleFor(x => x.TicketId)
                .NotEmpty();
        }
    }
}
