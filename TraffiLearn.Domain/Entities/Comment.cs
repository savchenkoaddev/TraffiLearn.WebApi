using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Comments;

namespace TraffiLearn.Domain.Entities
{
    public sealed class Comment : Entity
    {
        private readonly List<Comment> _replies = [];

        private Comment(Guid id)
            : base(id)
        { }

        private Comment(
            Guid id,
            CommentContent commentContent,
            User leftBy, 
            Question question) : base(id)
        {
            Content = commentContent;
            User = leftBy;
            Question = question;
        }

        public CommentContent Content { get; private set; }

        public User User { get; private set; }

        public Question Question { get; private set; }

        public Comment? RootComment { get; private set; }

        public IReadOnlyCollection<Comment> Replies => _replies;

        public void UpdateContent(CommentContent content)
        {
            Content = content;
        }

        public Result Reply(Comment comment)
        {
            comment.RootComment = this;
            _replies.Add(comment);

            return Result.Success();
        }

        public static Result<Comment> Create(
            Guid id,
            CommentContent content,
            User leftBy,
            Question question)
        {
            return new Comment(
                id, 
                content, 
                leftBy, 
                question);
        }
    }
}
