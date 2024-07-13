using FluentValidation;

namespace TraffiLearn.Application.Questions.Queries.GetTopicsForQuestion
{
    public sealed class GetTopicsForQuestionQueryValidator : AbstractValidator<GetTopicsForQuestionQuery>
    {
        public GetTopicsForQuestionQueryValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty();
        }
    }
}
