using FluentValidation;

namespace TraffiLearn.Application.Queries.Tickets.GetQuestionTickets
{
    internal sealed class GetQuestionTicketsQueryValidator
        : AbstractValidator<GetQuestionTicketsQuery>
    {
        public GetQuestionTicketsQueryValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty();
        }
    }
}
