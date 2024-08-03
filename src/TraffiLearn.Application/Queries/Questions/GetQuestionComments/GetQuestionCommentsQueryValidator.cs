using FluentValidation;

namespace TraffiLearn.Application.Queries.Questions.GetQuestionComments
{
    internal sealed class GetQuestionCommentsQueryValidator
        : AbstractValidator<GetQuestionCommentsQuery>
    {
        public GetQuestionCommentsQueryValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty();
        }
    }
}
