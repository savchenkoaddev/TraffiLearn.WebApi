using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Domain.Entities
{
    public sealed class User : Entity
    {
        private readonly List<Comment> _comments = [];

        private User(Guid id)
            : base(id)
        { }

        private User(
            Guid id,
            Email email,
            Username username)
            : base(id)
        {
            Email = email;
            Username = username;
        }

        public Email Email { get; private set; }

        public Username Username { get; private set; }

        public IReadOnlyCollection<Comment> Comments => _comments;

        public Result AddComment(Comment comment)
        {
            _comments.Add(comment);

            return Result.Success();
        }

        public static Result<User> Create(
            Guid id,
            Email email,
            Username username)
        {
            return new User(
                id,
                email,
                username);
        }
    }
}
