using FluentValidation;

namespace TraffiLearn.Application.UseCases.Questions.Queries.GetQuestionTopics
{
    internal sealed class GetQuestionTopicsQueryValidator : AbstractValidator<GetQuestionTopicsQuery>
    {
        public GetQuestionTopicsQueryValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty();
        }
    }
}
