using TraffiLearn.Domain.Errors.Comments;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Comments;

namespace TraffiLearn.Domain.Entities
{
    public sealed class Comment : Entity<CommentId>
    {
        private readonly HashSet<Comment> _replies = [];
        private readonly HashSet<User> _likedByUsers = [];
        private readonly HashSet<User> _dislikedByUsers = [];

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

        public IReadOnlyCollection<User> LikedByUsers => _likedByUsers;

        public IReadOnlyCollection<User> DislikedByUsers => _likedByUsers;

        public Result AddLike(User user)
        {
            if (_likedByUsers.Contains(user))
            {
                return CommentErrors.AlreadyLikedByUser;
            }

            if (_dislikedByUsers.Contains(user))
            {
                return CommentErrors.CantLikeIfDislikedByUser;
            }

            _likedByUsers.Add(user);

            return Result.Success();
        }

        public Result RemoveLike(User user)
        {
            if (!_likedByUsers.Contains(user))
            {
                return CommentErrors.NotLikedByUser;
            }

            _likedByUsers.Remove(user);

            return Result.Success();
        }

        public Result AddDislike(User user)
        {
            if (_dislikedByUsers.Contains(user))
            {
                return CommentErrors.AlreadyDislikedByUser;
            }

            if (_likedByUsers.Contains(user))
            {
                return CommentErrors.CantDislikeIfLikedByUser;
            }

            _dislikedByUsers.Add(user);

            return Result.Success();
        }

        public Result RemoveDislike(User user)
        {
            if (!_dislikedByUsers.Contains(user))
            {
                return CommentErrors.NotDislikedByUser;
            }

            return Result.Success();
        }

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
