using FluentValidation;

namespace TraffiLearn.Application.Queries.Questions.GetRandomQuestions
{
    internal sealed class GetRandomQuestionsQueryValidator : AbstractValidator<GetRandomQuestionsQuery>
    {
        public GetRandomQuestionsQueryValidator()
        {
            RuleFor(x => x.Amount)
                .NotEmpty()
                .GreaterThanOrEqualTo(1);
        }
    }
}
