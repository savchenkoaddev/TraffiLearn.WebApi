using FluentValidation;

namespace TraffiLearn.Application.Queries.Questions.GetQuestionTickets
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
