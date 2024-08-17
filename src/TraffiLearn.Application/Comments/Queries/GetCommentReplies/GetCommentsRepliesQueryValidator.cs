using FluentValidation;

namespace TraffiLearn.Application.Comments.Queries.GetCommentReplies
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
