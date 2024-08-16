using TraffiLearn.Domain.Aggregates.Comments.ValueObjects;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.Domain.Aggregates.Users.Errors;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Users
{
    public sealed class User : AggregateRoot<UserId>
    {
        private readonly HashSet<CommentId> _commentIds = [];
        private readonly HashSet<QuestionId> _markedQuestionIds = [];
        private readonly HashSet<QuestionId> _likedQuestionIds = [];
        private readonly HashSet<QuestionId> _dislikedQuestionIds = [];
        private readonly HashSet<CommentId> _likedCommentIds = [];
        private readonly HashSet<CommentId> _dislikedCommentIds = [];
        private Email _email;
        private Username _username;

        private User()
            : base(new(Guid.Empty))
        { }

        private User(
            UserId userId,
            Email email,
            Username username,
            Role role) : base(userId)
        {
            Email = email;
            Username = username;
            Role = role;
        }

        public Email Email
        {
            get
            {
                return _email;
            }
            private set
            {
                ArgumentNullException.ThrowIfNull(value, nameof(value));

                _email = value;
            }
        }

        public Username Username
        {
            get
            {
                return _username;
            }
            private set
            {
                ArgumentNullException.ThrowIfNull(value, nameof(value));

                _username = value;
            }
        }

        public Role Role { get; private set; }

        public IReadOnlyCollection<CommentId> CommentIds => _commentIds;

        public IReadOnlyCollection<QuestionId> MarkedQuestionIds => _markedQuestionIds;

        public IReadOnlyCollection<QuestionId> LikedQuestionIds => _likedQuestionIds;

        public IReadOnlyCollection<QuestionId> DislikedQuestionIds => _dislikedQuestionIds;

        public IReadOnlyCollection<CommentId> LikedCommentIds => _likedCommentIds;

        public IReadOnlyCollection<CommentId> DislikedCommentIds => _dislikedCommentIds;

        public Result DowngradeRole()
        {
            var roles = Enum.GetValues(typeof(Role)).Cast<Role>().OrderBy(r => r).ToList();

            var currentRoleIndex = roles.IndexOf(Role);

            if (currentRoleIndex <= 0)
            {
                return UserErrors.AccountCannotBeDowngraded;
            }

            var newRole = roles[currentRoleIndex - 1];

            Role = newRole;

            return Result.Success();
        }

        public Result AddComment(CommentId commentId)
        {
            if (_commentIds.Contains(commentId))
            {
                return UserErrors.CommentAlreadyAdded;
            }

            _commentIds.Add(commentId);

            return Result.Success();
        }

        public Result MarkQuestion(QuestionId questionId)
        {
            if (_markedQuestionIds.Contains(questionId))
            {
                return UserErrors.QuestionAlreadyMarked;
            }

            _markedQuestionIds.Add(questionId);

            return Result.Success();
        }

        public Result UnmarkQuestion(QuestionId questionId)
        {
            if (!_markedQuestionIds.Contains(questionId))
            {
                return UserErrors.QuestionAlreadyUnmarked;
            }

            _markedQuestionIds.Remove(questionId);

            return Result.Success();
        }

        public Result LikeQuestion(QuestionId questionId)
        {
            if (_likedQuestionIds.Contains(questionId))
            {
                return UserErrors.QuestionAlreadyLikedByUser;
            }

            if (_dislikedQuestionIds.Contains(questionId))
            {
                return UserErrors.CantLikeQuestionIfDisliked;
            }

            _likedQuestionIds.Add(questionId);

            return Result.Success();
        }

        public Result DislikeQuestion(QuestionId questionId)
        {
            if (_dislikedQuestionIds.Contains(questionId))
            {
                return UserErrors.QuestionAlreadyDislikedByUser;
            }

            if (_likedQuestionIds.Contains(questionId))
            {
                return UserErrors.CantDislikeQuestionIfLiked;
            }

            _dislikedQuestionIds.Add(questionId);

            return Result.Success();
        }

        public Result RemoveQuestionLike(QuestionId questionId)
        {
            if (!_likedQuestionIds.Contains(questionId))
            {
                return UserErrors.QuestionNotLiked;
            }

            _likedQuestionIds.Remove(questionId);

            return Result.Success();
        }

        public Result RemoveQuestionDislike(QuestionId questionId)
        {
            if (!_dislikedQuestionIds.Contains(questionId))
            {
                return UserErrors.QuestionNotDisliked;
            }

            _dislikedQuestionIds.Remove(questionId);

            return Result.Success();
        }

        public Result LikeComment(CommentId commentId)
        {
            if (_likedCommentIds.Contains(commentId))
            {
                return UserErrors.CommentAlreadyLikedByUser;
            }

            if (_dislikedCommentIds.Contains(commentId))
            {
                return UserErrors.CantLikeCommentIfDisliked;
            }

            _likedCommentIds.Add(commentId);

            return Result.Success();
        }

        public Result DislikeComment(CommentId commentId)
        {
            if (_dislikedCommentIds.Contains(commentId))
            {
                return UserErrors.CommentAlreadyDislikedByUser;
            }

            if (_likedCommentIds.Contains(commentId))
            {
                return UserErrors.CantDislikeCommentIfLiked;
            }

            _dislikedCommentIds.Add(commentId);

            return Result.Success();
        }

        public Result RemoveCommentLike(CommentId commentId)
        {
            if (!_likedCommentIds.Contains(commentId))
            {
                return UserErrors.CommentNotLiked;
            }

            _likedCommentIds.Remove(commentId);

            return Result.Success();
        }

        public Result RemoveCommentDislike(CommentId commentId)
        {
            if (!_dislikedCommentIds.Contains(commentId))
            {
                return UserErrors.CommentNotDisliked;
            }

            _dislikedCommentIds.Remove(commentId);

            return Result.Success();
        }

        public static Result<User> Create(
            UserId userId,
            Email email,
            Username username,
            Role role)
        {
            return new User(
                userId,
                email,
                username,
                role);
        }
    }
}
