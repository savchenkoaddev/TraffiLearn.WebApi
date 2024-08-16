using FluentValidation;

namespace TraffiLearn.Application.Queries.Topics.GetQuestionTopics
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
