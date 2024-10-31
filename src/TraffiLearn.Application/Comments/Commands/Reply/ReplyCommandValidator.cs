using FluentValidation;
using TraffiLearn.Domain.Comments.CommentContents;

namespace TraffiLearn.Application.Comments.Commands.Reply
{
    internal sealed class ReplyCommandValidator
        : AbstractValidator<ReplyCommand>
    {
        public ReplyCommandValidator()
        {
            RuleFor(x => x.CommentId)
                .NotEmpty();

            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(CommentContent.MaxLength);
        }
    }
}
