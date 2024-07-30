using FluentValidation;

namespace TraffiLearn.Application.Queries.Questions.GetTopicsForQuestion
{
    internal sealed class GetTopicsForQuestionQueryValidator : AbstractValidator<GetTopicsForQuestionQuery>
    {
        public GetTopicsForQuestionQueryValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty();
        }
    }
}
