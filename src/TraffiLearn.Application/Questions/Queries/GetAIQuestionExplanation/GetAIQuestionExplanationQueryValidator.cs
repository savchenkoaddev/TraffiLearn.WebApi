using FluentValidation;

namespace TraffiLearn.Application.Questions.Queries.GetAIQuestionExplanation
{
    internal sealed class GetAIQuestionExplanationQueryValidator
        : AbstractValidator<GetAIQuestionExplanationQuery>
    {
        public GetAIQuestionExplanationQueryValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty();
        }
    }
}