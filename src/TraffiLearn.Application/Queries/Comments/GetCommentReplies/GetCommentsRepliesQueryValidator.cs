using FluentValidation;

namespace TraffiLearn.Application.Queries.Comments.GetCommentReplies
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
