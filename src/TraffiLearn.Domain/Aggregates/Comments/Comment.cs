using TraffiLearn.Domain.Aggregates.Comments.Errors;
using TraffiLearn.Domain.Aggregates.Comments.ValueObjects;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Comments
{
    public sealed class Comment : AggregateRoot<CommentId>
    {
        private readonly HashSet<Comment> _replies = [];
        private readonly HashSet<UserId> _likedByUsers = [];
        private readonly HashSet<UserId> _dislikedByUsers = [];
        private CommentContent _content;
        private UserId _creatorId;
        private QuestionId _questionId;

        private Comment()
            : base(new(Guid.Empty))
        { }

        private Comment(
            CommentId commentId,
            CommentContent commentContent,
            UserId creatorId,
            QuestionId questionId) : base(commentId)
        {
            Content = commentContent;
            CreatorId = creatorId;
            QuestionId = questionId;
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

        public UserId CreatorId { get; private set; }

        public QuestionId QuestionId { get; private set; }

        public Comment? RootComment { get; private set; }

        public int LikesCount => _likedByUsers.Count;

        public int DislikesCount => _dislikedByUsers.Count;

        public IReadOnlyCollection<Comment> Replies => _replies;

        public IReadOnlyCollection<UserId> LikedByUsers => _likedByUsers;

        public IReadOnlyCollection<UserId> DislikedByUsers => _dislikedByUsers;

        public Result AddLike(UserId userId)
        {
            if (_likedByUsers.Contains(userId))
            {
                return CommentErrors.AlreadyLikedByUser;
            }

            if (_dislikedByUsers.Contains(userId))
            {
                return CommentErrors.CantLikeIfDislikedByUser;
            }

            _likedByUsers.Add(userId);

            return Result.Success();
        }

        public Result RemoveLike(UserId userId)
        {
            if (!_likedByUsers.Contains(userId))
            {
                return CommentErrors.NotLikedByUser;
            }

            _likedByUsers.Remove(userId);

            return Result.Success();
        }

        public Result AddDislike(UserId userId)
        {
            if (_dislikedByUsers.Contains(userId))
            {
                return CommentErrors.AlreadyDislikedByUser;
            }

            if (_likedByUsers.Contains(userId))
            {
                return CommentErrors.CantDislikeIfLikedByUser;
            }

            _dislikedByUsers.Add(userId);

            return Result.Success();
        }

        public Result RemoveDislike(UserId userId)
        {
            if (!_dislikedByUsers.Contains(userId))
            {
                return CommentErrors.NotDislikedByUser;
            }

            _dislikedByUsers.Remove(userId);

            return Result.Success();
        }

        public Result Update(CommentContent content)
        {
            Content = content;

            return Result.Success();
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
            UserId creatorId,
            QuestionId questionId)
        {
            return new Comment(
                commentId,
                content,
                creatorId,
                questionId);
        }
    }
}
