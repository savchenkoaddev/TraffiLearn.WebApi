using FluentValidation;

namespace TraffiLearn.Application.Tickets.Queries.GetQuestionTickets
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
