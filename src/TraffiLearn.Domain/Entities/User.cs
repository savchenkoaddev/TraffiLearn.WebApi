using TraffiLearn.Domain.Enums;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Domain.Entities
{
    public sealed class User : AggregateRoot<UserId>
    {
        private readonly HashSet<Comment> _comments = [];
        private readonly HashSet<Question> _markedQuestions = [];
        private readonly HashSet<Question> _likedQuestions = [];
        private readonly HashSet<Question> _dislikedQuestions = [];
        private readonly HashSet<Comment> _likedComments = [];
        private readonly HashSet<Comment> _dislikedComments = [];
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

        public IReadOnlyCollection<Comment> Comments => _comments;

        public IReadOnlyCollection<Question> MarkedQuestions => _markedQuestions;

        public IReadOnlyCollection<Question> LikedQuestions => _likedQuestions;

        public IReadOnlyCollection<Question> DislikedQuestions => _dislikedQuestions;

        public IReadOnlyCollection<Comment> LikedComments => _likedComments;

        public IReadOnlyCollection<Comment> DislikedComments => _dislikedComments;

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

        public Result AddComment(Comment comment)
        {
            ArgumentNullException.ThrowIfNull(comment, nameof(comment));

            if (_comments.Contains(comment))
            {
                return UserErrors.CommentAlreadyAdded;
            }

            _comments.Add(comment);

            return Result.Success();
        }

        public Result MarkQuestion(Question question)
        {
            ArgumentNullException.ThrowIfNull(question, nameof(question));

            if (_markedQuestions.Contains(question))
            {
                return UserErrors.QuestionAlreadyMarked;
            }

            _markedQuestions.Add(question);

            return Result.Success();
        }

        public Result UnmarkQuestion(Question question)
        {
            ArgumentNullException.ThrowIfNull(question, nameof(question));

            if (!_markedQuestions.Contains(question))
            {
                return UserErrors.QuestionAlreadyUnmarked;
            }

            _markedQuestions.Remove(question);

            return Result.Success();
        }

        public Result LikeQuestion(Question question)
        {
            ArgumentNullException.ThrowIfNull(question, nameof(question));

            if (_likedQuestions.Contains(question))
            {
                return UserErrors.QuestionAlreadyLikedByUser;
            }

            if (_dislikedQuestions.Contains(question))
            {
                return UserErrors.CantLikeQuestionIfDisliked;
            }

            _likedQuestions.Add(question);

            return Result.Success();
        }

        public Result DislikeQuestion(Question question)
        {
            ArgumentNullException.ThrowIfNull(question, nameof(question));

            if (_dislikedQuestions.Contains(question))
            {
                return UserErrors.QuestionAlreadyDislikedByUser;
            }

            if (_likedQuestions.Contains(question))
            {
                return UserErrors.CantDislikeQuestionIfLiked;
            }

            _dislikedQuestions.Add(question);

            return Result.Success();
        }

        public Result RemoveQuestionLike(Question question)
        {
            ArgumentNullException.ThrowIfNull(question, nameof(question));

            if (!_likedQuestions.Contains(question))
            {
                return UserErrors.QuestionNotLiked;
            }

            _likedQuestions.Remove(question);

            return Result.Success();
        }

        public Result RemoveQuestionDislike(Question question)
        {
            ArgumentNullException.ThrowIfNull(question, nameof(question));

            if (!_dislikedQuestions.Contains(question))
            {
                return UserErrors.QuestionNotDisliked;
            }

            _dislikedQuestions.Remove(question);

            return Result.Success();
        }

        public Result LikeComment(Comment comment)
        {
            ArgumentNullException.ThrowIfNull(comment, nameof(comment));

            if (_likedComments.Contains(comment))
            {
                return UserErrors.CommentAlreadyLikedByUser;
            }

            if (_dislikedComments.Contains(comment))
            {
                return UserErrors.CantLikeCommentIfDisliked;
            }

            _likedComments.Add(comment);

            return Result.Success();
        }

        public Result DislikeComment(Comment comment)
        {
            ArgumentNullException.ThrowIfNull(comment, nameof(comment));

            if (_dislikedComments.Contains(comment))
            {
                return UserErrors.CommentAlreadyDislikedByUser;
            }

            if (_likedComments.Contains(comment))
            {
                return UserErrors.CantDislikeCommentIfLiked;
            }

            _dislikedComments.Add(comment);

            return Result.Success();
        }

        public Result RemoveCommentLike(Comment comment)
        {
            ArgumentNullException.ThrowIfNull(comment, nameof(comment));

            if (!_likedComments.Contains(comment))
            {
                return UserErrors.CommentNotLiked;
            }

            _likedComments.Remove(comment);

            return Result.Success();
        }

        public Result RemoveCommentDislike(Comment comment)
        {
            ArgumentNullException.ThrowIfNull(comment, nameof(comment));

            if (!_dislikedComments.Contains(comment))
            {
                return UserErrors.CommentNotDisliked;
            }

            _dislikedComments.Remove(comment);

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
