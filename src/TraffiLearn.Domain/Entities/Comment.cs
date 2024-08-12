using TraffiLearn.Domain.Errors.Comments;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Comments;

namespace TraffiLearn.Domain.Entities
{
    public sealed class Comment : Entity<CommentId>
    {
        private readonly HashSet<Comment> _replies = [];

        private Comment()
            : base(new(Guid.Empty))
        { }

        private Comment(
            CommentId commentId,
            CommentContent commentContent,
            User creator,
            Question question) : base(commentId)
        {
            Content = commentContent;
            Creator = creator;
            Question = question;
        }

        public CommentContent Content { get; private set; }

        public User Creator { get; private set; }

        public Question Question { get; private set; }

        public Comment? RootComment { get; private set; }

        public IReadOnlyCollection<Comment> Replies => _replies;

        public void UpdateContent(CommentContent content)
        {
            Content = content;
        }

        public Result Reply(Comment comment)
        {
            if (_replies.Contains(comment))
            {
                return CommentErrors.CommentAlreadyAdded;
            }

            comment.RootComment = this;
            _replies.Add(comment);

            return Result.Success();
        }

        public static Result<Comment> Create(
            CommentId commentId,
            CommentContent content,
            User creator,
            Question question)
        {
            return new Comment(
                commentId,
                content,
                creator,
                question);
        }
    }
}
