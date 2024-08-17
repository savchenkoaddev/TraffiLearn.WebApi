using FluentValidation;

namespace TraffiLearn.Application.Comments.Queries.GetQuestionComments
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
