using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Comments;

namespace TraffiLearn.Domain.Entities
{
    public sealed class Comment : Entity
    {
        private readonly List<Comment> _comments = [];

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
            LeftBy = leftBy;
            Question = question;
        }

        public CommentContent Content { get; private set; }

        public User LeftBy { get; private set; }

        public Question Question { get; private set; }

        public IReadOnlyCollection<Comment> Comments => _comments;

        public void UpdateContent(CommentContent content)
        {
            Content = content;
        }

        public Result Answer(Comment comment)
        {
            _comments.Add(comment);

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
