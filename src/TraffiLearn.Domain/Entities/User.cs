using TraffiLearn.Domain.Enums;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Domain.Entities
{
    public sealed class User : Entity<UserId>
    {
        private readonly HashSet<Comment> _comments = [];
        private readonly HashSet<Question> _markedQuestions = [];
        private readonly HashSet<Question> _likedQuestions = [];
        private readonly HashSet<Question> _dislikedQuestions = [];

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

        public Email Email { get; private set; }

        public Username Username { get; private set; }
        
        public Role Role { get; private set; }

        public IReadOnlyCollection<Comment> Comments => _comments;

        public IReadOnlyCollection<Question> MarkedQuestions => _markedQuestions;

        public IReadOnlyCollection<Question> LikedQuestions => _likedQuestions;

        public IReadOnlyCollection<Question> DislikedQuestions => _dislikedQuestions;

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
            if (_comments.Contains(comment))
            {
                return UserErrors.CommentAlreadyAdded;
            }

            _comments.Add(comment);

            return Result.Success();
        }

        public Result MarkQuestion(Question question)
        {
            if (_markedQuestions.Contains(question))
            {
                return UserErrors.QuestionAlreadyMarked;
            }

            _markedQuestions.Add(question);

            return Result.Success();
        }

        public Result UnmarkQuestion(Question question)
        {
            if (!_markedQuestions.Contains(question))
            {
                return UserErrors.QuestionAlreadyUnmarked;
            }

            _markedQuestions.Remove(question);

            return Result.Success();
        }

        public Result LikeQuestion(Question question)
        {
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
            if (!_likedQuestions.Contains(question))
            {
                return UserErrors.QuestionNotLiked;
            }

            _likedQuestions.Remove(question);

            return Result.Success();
        }

        public Result RemoveQuestionDislike(Question question)
        {
            if (!_dislikedQuestions.Contains(question))
            {
                return UserErrors.QuestionNotDisliked;
            }

            _dislikedQuestions.Remove(question);

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
