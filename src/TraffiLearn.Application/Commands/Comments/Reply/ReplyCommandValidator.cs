using FluentValidation;
using TraffiLearn.Domain.ValueObjects.Comments;

namespace TraffiLearn.Application.Commands.Comments.Reply
{
    internal sealed class ReplyCommandValidator 
        : AbstractValidator<ReplyCommand>
    {
        public ReplyCommandValidator()
        {
            RuleFor(x => x.CommentId)
                .NotEmpty();

            RuleFor(x => x.ReplyContent)
                .NotEmpty()
                .MaximumLength(CommentContent.MaxLength);
        }
    }
}
