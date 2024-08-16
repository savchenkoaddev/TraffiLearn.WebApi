using TraffiLearn.Domain.Errors.Comments;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Comments;

namespace TraffiLearn.Domain.Entities
{
    public sealed class Comment : AggregateRoot<CommentId>
    {
        private readonly HashSet<Comment> _replies = [];
        private readonly HashSet<User> _likedByUsers = [];
        private readonly HashSet<User> _dislikedByUsers = [];
        private CommentContent _content;
        private User _creator;
        private Question _question;

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

        public CommentContent Content
        {
            get
            {
                return _content;
            }
            private set
            {
                ArgumentNullException.ThrowIfNull(value, nameof(value));

                _content = value;
            }
        }

        public User Creator
        {
            get
            {
                return _creator;
            }
            private set
            {
                ArgumentNullException.ThrowIfNull(value, nameof(value));

                _creator = value;
            }
        }

        public Question Question
        {
            get
            {
                return _question;
            }
            private set
            {
                ArgumentNullException.ThrowIfNull(value, nameof(value));

                _question = value;
            }
        }

        public Comment? RootComment { get; private set; }

        public int LikesCount => _likedByUsers.Count;

        public int DislikesCount => _dislikedByUsers.Count;

        public IReadOnlyCollection<Comment> Replies => _replies;

        public IReadOnlyCollection<User> LikedByUsers => _likedByUsers;

        public IReadOnlyCollection<User> DislikedByUsers => _dislikedByUsers;

        public Result AddLike(User user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));

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
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            if (!_likedByUsers.Contains(user))
            {
                return CommentErrors.NotLikedByUser;
            }

            _likedByUsers.Remove(user);

            return Result.Success();
        }

        public Result AddDislike(User user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));

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
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            if (!_dislikedByUsers.Contains(user))
            {
                return CommentErrors.NotDislikedByUser;
            }

            _dislikedByUsers.Remove(user);

            return Result.Success();
        }

        public Result Update(CommentContent content)
        {
            Content = content;

            return Result.Success();
        }

        public Result Reply(Comment comment)
        {
            ArgumentNullException.ThrowIfNull(comment, nameof(comment));

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
