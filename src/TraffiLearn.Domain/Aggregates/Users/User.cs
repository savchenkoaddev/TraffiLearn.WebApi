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
        private readonly HashSet<CommentId> _commentsIds = [];
        private readonly HashSet<QuestionId> _markedQuestionsIds = [];
        private readonly HashSet<QuestionId> _likedQuestionsIds = [];
        private readonly HashSet<QuestionId> _dislikedQuestionsIds = [];
        private readonly HashSet<CommentId> _likedCommentsIds = [];
        private readonly HashSet<CommentId> _dislikedCommentsIds = [];
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

        public IReadOnlyCollection<CommentId> CommentsIds => _commentsIds;

        public IReadOnlyCollection<QuestionId> MarkedQuestionsIds => _markedQuestionsIds;

        public IReadOnlyCollection<QuestionId> LikedQuestionsIds => _likedQuestionsIds;

        public IReadOnlyCollection<QuestionId> DislikedQuestionsIds => _dislikedQuestionsIds;

        public IReadOnlyCollection<CommentId> LikedCommentsIds => _likedCommentsIds;

        public IReadOnlyCollection<CommentId> DislikedCommentsIds => _dislikedCommentsIds;

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
            if (_commentsIds.Contains(commentId))
            {
                return UserErrors.CommentAlreadyAdded;
            }

            _commentsIds.Add(commentId);

            return Result.Success();
        }

        public Result MarkQuestion(QuestionId questionId)
        {
            if (_markedQuestionsIds.Contains(questionId))
            {
                return UserErrors.QuestionAlreadyMarked;
            }

            _markedQuestionsIds.Add(questionId);

            return Result.Success();
        }

        public Result UnmarkQuestion(QuestionId questionId)
        {
            if (!_markedQuestionsIds.Contains(questionId))
            {
                return UserErrors.QuestionAlreadyUnmarked;
            }

            _markedQuestionsIds.Remove(questionId);

            return Result.Success();
        }

        public Result LikeQuestion(QuestionId questionId)
        {
            if (_likedQuestionsIds.Contains(questionId))
            {
                return UserErrors.QuestionAlreadyLikedByUser;
            }

            if (_dislikedQuestionsIds.Contains(questionId))
            {
                return UserErrors.CantLikeQuestionIfDisliked;
            }

            _likedQuestionsIds.Add(questionId);

            return Result.Success();
        }

        public Result DislikeQuestion(QuestionId questionId)
        {
            if (_dislikedQuestionsIds.Contains(questionId))
            {
                return UserErrors.QuestionAlreadyDislikedByUser;
            }

            if (_likedQuestionsIds.Contains(questionId))
            {
                return UserErrors.CantDislikeQuestionIfLiked;
            }

            _dislikedQuestionsIds.Add(questionId);

            return Result.Success();
        }

        public Result RemoveQuestionLike(QuestionId questionId)
        {
            if (!_likedQuestionsIds.Contains(questionId))
            {
                return UserErrors.QuestionNotLiked;
            }

            _likedQuestionsIds.Remove(questionId);

            return Result.Success();
        }

        public Result RemoveQuestionDislike(QuestionId questionId)
        {
            if (!_dislikedQuestionsIds.Contains(questionId))
            {
                return UserErrors.QuestionNotDisliked;
            }

            _dislikedQuestionsIds.Remove(questionId);

            return Result.Success();
        }

        public Result LikeComment(CommentId commentId)
        {
            if (_likedCommentsIds.Contains(commentId))
            {
                return UserErrors.CommentAlreadyLikedByUser;
            }

            if (_dislikedCommentsIds.Contains(commentId))
            {
                return UserErrors.CantLikeCommentIfDisliked;
            }

            _likedCommentsIds.Add(commentId);

            return Result.Success();
        }

        public Result DislikeComment(CommentId commentId)
        {
            if (_dislikedCommentsIds.Contains(commentId))
            {
                return UserErrors.CommentAlreadyDislikedByUser;
            }

            if (_likedCommentsIds.Contains(commentId))
            {
                return UserErrors.CantDislikeCommentIfLiked;
            }

            _dislikedCommentsIds.Add(commentId);

            return Result.Success();
        }

        public Result RemoveCommentLike(CommentId commentId)
        {
            if (!_likedCommentsIds.Contains(commentId))
            {
                return UserErrors.CommentNotLiked;
            }

            _likedCommentsIds.Remove(commentId);

            return Result.Success();
        }

        public Result RemoveCommentDislike(CommentId commentId)
        {
            if (!_dislikedCommentsIds.Contains(commentId))
            {
                return UserErrors.CommentNotDisliked;
            }

            _dislikedCommentsIds.Remove(commentId);

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
