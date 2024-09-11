using FluentValidation;

namespace TraffiLearn.Application.Questions.Queries.GetAIQuestionComments
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