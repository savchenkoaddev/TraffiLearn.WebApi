using TraffiLearn.Domain.Aggregates.Comments;
using TraffiLearn.Domain.Aggregates.Questions.Errors;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Tickets;
using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Questions
{
    public sealed class Question : AggregateRoot<QuestionId>
    {
        private readonly HashSet<Topic> _topics = [];
        private readonly HashSet<Ticket> _tickets = [];
        private readonly HashSet<Comment> _comments = [];
        private readonly HashSet<User> _likedByUsers = [];
        private readonly HashSet<User> _dislikedByUsers = [];
        private readonly HashSet<User> _markedByUsers = [];
        private HashSet<Answer> _answers = [];
        private QuestionContent _content;
        private QuestionNumber _questionNumber;

        private Question()
            : base(new(Guid.Empty))
        { }

        private Question(
            QuestionId questionId,
            QuestionContent content,
            QuestionExplanation? explanation,
            QuestionNumber questionNumber,
            HashSet<Answer> answers,
            ImageUri? imageUri) : base(questionId)
        {
            Content = content;
            Explanation = explanation;
            QuestionNumber = questionNumber;
            _answers = answers;
            ImageUri = imageUri;
        }

        public QuestionContent Content
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

        public QuestionNumber QuestionNumber
        {
            get
            {
                return _questionNumber;
            }
            private set
            {
                ArgumentNullException.ThrowIfNull(value, nameof(value));

                _questionNumber = value;
            }
        }

        public QuestionExplanation? Explanation { get; private set; }

        public ImageUri? ImageUri { get; private set; }

        public int LikesCount => _likedByUsers.Count;

        public int DislikesCount => _dislikedByUsers.Count;

        public IReadOnlyCollection<Topic> Topics => _topics;

        public IReadOnlyCollection<Answer> Answers => _answers;

        public IReadOnlyCollection<Ticket> Tickets => _tickets;

        public IReadOnlyCollection<Comment> Comments => _comments;

        public IReadOnlyCollection<User> LikedByUsers => _likedByUsers;

        public IReadOnlyCollection<User> DislikedByUsers => _dislikedByUsers;

        public IReadOnlyCollection<User> MarkedByUsers => _markedByUsers;

        public Result Mark(User user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            if (_markedByUsers.Contains(user))
            {
                return QuestionErrors.AlreadyMarkedByUser;
            }

            _markedByUsers.Add(user);

            return Result.Success();
        }

        public Result Unmark(User user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            if (!_markedByUsers.Contains(user))
            {
                return QuestionErrors.IsNotMarkedByUser;
            }

            _markedByUsers.Remove(user);

            return Result.Success();
        }

        public Result AddLike(User user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            if (_likedByUsers.Contains(user))
            {
                return QuestionErrors.AlreadyLikedByUser;
            }

            if (_dislikedByUsers.Contains(user))
            {
                return QuestionErrors.CantLikeIfDislikedByUser;
            }

            _likedByUsers.Add(user);

            return Result.Success();
        }

        public Result AddDislike(User user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            if (_dislikedByUsers.Contains(user))
            {
                return QuestionErrors.AlreadyDislikedByUser;
            }

            if (_likedByUsers.Contains(user))
            {
                return QuestionErrors.CantDislikeIfLikedByUser;
            }

            _dislikedByUsers.Add(user);

            return Result.Success();
        }

        public Result RemoveLike(User user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            if (!_likedByUsers.Contains(user))
            {
                return QuestionErrors.NotLikedByUser;
            }

            _likedByUsers.Remove(user);

            return Result.Success();
        }

        public Result RemoveDislike(User user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            if (!_dislikedByUsers.Contains(user))
            {
                return QuestionErrors.NotDislikedByUser;
            }

            _dislikedByUsers.Remove(user);

            return Result.Success();
        }

        public Result AddComment(Comment comment)
        {
            ArgumentNullException.ThrowIfNull(comment, nameof(comment));

            if (_comments.Contains(comment))
            {
                return QuestionErrors.CommentAlreadyAdded;
            }

            _comments.Add(comment);

            return Result.Success();
        }

        public Result AddAnswer(Answer answer)
        {
            ArgumentNullException.ThrowIfNull(answer, nameof(answer));

            if (_answers.Contains(answer))
            {
                return QuestionErrors.AnswerAlreadyAdded;
            }

            if (_answers.Count == 0 &&
                answer.IsCorrect == false)
            {
                return QuestionErrors.FirstlyAddedAnswerIncorrect;
            }

            _answers.Add(answer);

            return Result.Success();
        }

        public Result RemoveAnswer(Answer answer)
        {
            ArgumentNullException.ThrowIfNull(answer, nameof(answer));

            if (!_answers.Contains(answer))
            {
                return QuestionErrors.AnswerNotFound;
            }

            if (ExistsSingleCorrectAnswerOnly() &&
                answer.IsCorrect == true)
            {
                return QuestionErrors.UnableToRemoveSingleCorrectAnswer;
            }

            _answers.Remove(answer);

            return Result.Success();
        }

        public Result AddTopic(Topic topic)
        {
            ArgumentNullException.ThrowIfNull(topic, nameof(topic));

            if (_topics.Contains(topic))
            {
                return QuestionErrors.TopicAlreadyAdded;
            }

            _topics.Add(topic);

            return Result.Success();
        }

        public Result RemoveTopic(Topic topic)
        {
            ArgumentNullException.ThrowIfNull(topic, nameof(topic));

            if (!_topics.Contains(topic))
            {
                return QuestionErrors.TopicNotFound;
            }

            _topics.Remove(topic);

            return Result.Success();
        }

        public Result AddTicket(Ticket ticket)
        {
            ArgumentNullException.ThrowIfNull(ticket, nameof(ticket));

            if (_tickets.Contains(ticket))
            {
                return QuestionErrors.TicketAlreadyAdded;
            }

            _tickets.Add(ticket);

            return Result.Success();
        }

        public Result RemoveTicket(Ticket ticket)
        {
            ArgumentNullException.ThrowIfNull(ticket, nameof(ticket));

            if (!_tickets.Contains(ticket))
            {
                return QuestionErrors.TicketNotFound;
            }

            _tickets.Remove(ticket);

            return Result.Success();
        }

        public void SetImageUri(ImageUri? imageUri)
        {
            ImageUri = imageUri;
        }

        public Result Update(
            QuestionContent content,
            QuestionExplanation? explanation,
            QuestionNumber number,
            List<Answer> answers,
            ImageUri? imageUri)
        {
            ArgumentNullException.ThrowIfNull(answers, nameof(answers));

            var answersValidationResult = ValidateAnswers(answers);

            if (answersValidationResult.IsFailure)
            {
                return answersValidationResult.Error;
            }

            Content = content;
            Explanation = explanation;
            QuestionNumber = number;
            _answers = answers.ToHashSet();
            ImageUri = imageUri;

            return Result.Success();
        }

        public static Result<Question> Create(
            QuestionId questionId,
            QuestionContent content,
            QuestionExplanation? explanation,
            QuestionNumber questionNumber,
            List<Answer> answers,
            ImageUri? imageUri)
        {
            ArgumentNullException.ThrowIfNull(answers, nameof(answers));

            var answersValidationResult = ValidateAnswers(answers);

            if (answersValidationResult.IsFailure)
            {
                return Result.Failure<Question>(answersValidationResult.Error);
            }

            return new Question(
                questionId,
                content,
                explanation,
                questionNumber,
                answers.ToHashSet(),
                imageUri);
        }

        private static Result ValidateAnswers(List<Answer> answers)
        {
            if (answers.Count == 0)
            {
                return QuestionErrors.NoAnswers;
            }

            if (answers.All(a => a.IsCorrect == false))
            {
                return QuestionErrors.AllAnswersAreIncorrect;
            }

            var uniqueAnswers = new HashSet<Answer>(answers);

            if (uniqueAnswers.Count != answers.Count)
            {
                return QuestionErrors.DuplicateAnswers;
            }

            return Result.Success();
        }

        private bool ExistsSingleCorrectAnswerOnly()
        {
            return _answers.Count(q => q.IsCorrect == true) == 1;
        }
    }
}
