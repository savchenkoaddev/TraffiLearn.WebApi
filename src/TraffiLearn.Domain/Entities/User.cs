﻿using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Domain.Entities
{
    public sealed class User : Entity
    {
        private readonly HashSet<Comment> _comments = [];
        private readonly HashSet<Question> _markedQuestions = [];

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

        public IReadOnlyCollection<Question> MarkedQuestions => _markedQuestions;

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
                return UserErrors.QuestionNotFound;
            }

            _markedQuestions.Remove(question);

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
