using FluentValidation;

namespace TraffiLearn.Application.Queries.Comments.GetQuestionComments
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
