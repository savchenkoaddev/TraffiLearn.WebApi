using FluentValidation;

namespace TraffiLearn.Application.Questions.Queries.GetRandomQuestions
{
    internal sealed class GetRandomQuestionsQueryValidator
        : AbstractValidator<GetRandomQuestionsQuery>
    {
        public GetRandomQuestionsQueryValidator()
        {
            RuleFor(x => x.Amount)
                .NotEmpty()
                .GreaterThanOrEqualTo(1);
        }
    }
}
