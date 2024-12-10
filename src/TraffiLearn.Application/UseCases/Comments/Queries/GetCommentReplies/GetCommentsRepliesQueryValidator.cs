using FluentValidation;

namespace TraffiLearn.Application.UseCases.Comments.Queries.GetCommentReplies
{
    internal sealed class GetCommentsRepliesQueryValidator
        : AbstractValidator<GetCommentsRepliesQuery>
    {
        public GetCommentsRepliesQueryValidator()
        {
            RuleFor(x => x.CommentId)
                .NotEmpty();
        }
    }
}
