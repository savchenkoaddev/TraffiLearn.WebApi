using FluentValidation;

namespace TraffiLearn.Application.Questions.Queries.GetQuestionCommentsPaginated
{
    internal sealed class GetQuestionCommentsPaginatedQueryValidator
        : AbstractValidator<GetQuestionCommentsPaginatedQuery>
    {
        public GetQuestionCommentsPaginatedQueryValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty();
        }
    }
}
