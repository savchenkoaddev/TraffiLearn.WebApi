using FluentValidation;

namespace TraffiLearn.Application.Queries.Questions.GetTopicsForQuestion
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
