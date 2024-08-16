using FluentValidation;

namespace TraffiLearn.Application.Queries.Questions.GetTicketQuestions
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
