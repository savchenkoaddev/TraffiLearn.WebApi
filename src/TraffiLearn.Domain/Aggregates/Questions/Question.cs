using TraffiLearn.Domain.Aggregates.Comments.ValueObjects;
using TraffiLearn.Domain.Aggregates.Questions.Errors;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Tickets.ValueObjects;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Questions
{
    public sealed class Question : AggregateRoot<QuestionId>
    {
        private readonly HashSet<TopicId> _topicIds = [];
        private readonly HashSet<TicketId> _ticketIds = [];
        private readonly HashSet<CommentId> _commentIds = [];
        private readonly HashSet<UserId> _likedByUsersIds = [];
        private readonly HashSet<UserId> _dislikedByUsersIds = [];
        private HashSet<Answer> _answers = [];
        private QuestionContent _content;
        private QuestionExplanation _explanation;
        private QuestionNumber _questionNumber;

        private Question()
            : base(new(Guid.Empty))
        { }

        private Question(
            QuestionId questionId,
            QuestionContent content,
            QuestionExplanation explanation,
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

        public QuestionExplanation Explanation
        {
            get
            {
                return _explanation;
            }
            private set
            {
                ArgumentNullException.ThrowIfNull(value, nameof(value));

                _explanation = value;
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

        public ImageUri? ImageUri { get; private set; }

        public int LikesCount => _likedByUsersIds.Count;

        public int DislikesCount => _dislikedByUsersIds.Count;

        public IReadOnlyCollection<TopicId> TopicIds => _topicIds;

        public IReadOnlyCollection<Answer> Answers => _answers;

        public IReadOnlyCollection<TicketId> TicketIds => _ticketIds;

        public IReadOnlyCollection<CommentId> CommentIds => _commentIds;

        public IReadOnlyCollection<UserId> LikedByUsersIds => _likedByUsersIds;

        public IReadOnlyCollection<UserId> DislikedByUsersIds => _dislikedByUsersIds;

        public Result AddLike(UserId userId)
        {
            if (_likedByUsersIds.Contains(userId))
            {
                return QuestionErrors.AlreadyLikedByUser;
            }

            if (_dislikedByUsersIds.Contains(userId))
            {
                return QuestionErrors.CantLikeIfDislikedByUser;
            }

            _likedByUsersIds.Add(userId);

            return Result.Success();
        }

        public Result AddDislike(UserId userId)
        {
            if (_dislikedByUsersIds.Contains(userId))
            {
                return QuestionErrors.AlreadyDislikedByUser;
            }

            if (_likedByUsersIds.Contains(userId))
            {
                return QuestionErrors.CantDislikeIfLikedByUser;
            }

            _dislikedByUsersIds.Add(userId);

            return Result.Success();
        }

        public Result RemoveLike(UserId userId)
        {
            if (!_likedByUsersIds.Contains(userId))
            {
                return QuestionErrors.NotLikedByUser;
            }

            _likedByUsersIds.Remove(userId);

            return Result.Success();
        }

        public Result RemoveDislike(UserId userId)
        {
            if (!_dislikedByUsersIds.Contains(userId))
            {
                return QuestionErrors.NotDislikedByUser;
            }

            _dislikedByUsersIds.Remove(userId);

            return Result.Success();
        }

        public Result AddComment(CommentId commentId)
        {
            if (_commentIds.Contains(commentId))
            {
                return QuestionErrors.CommentAlreadyAdded;
            }

            _commentIds.Add(commentId);

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

        public Result AddTopic(TopicId topicId)
        {
            if (_topicIds.Contains(topicId))
            {
                return QuestionErrors.TopicAlreadyAdded;
            }

            _topicIds.Add(topicId);

            return Result.Success();
        }

        public Result RemoveTopic(TopicId topicId)
        {
            if (!_topicIds.Contains(topicId))
            {
                return QuestionErrors.TopicNotFound;
            }

            _topicIds.Remove(topicId);

            return Result.Success();
        }

        public Result AddTicket(TicketId ticketId)
        {
            if (_ticketIds.Contains(ticketId))
            {
                return QuestionErrors.TicketAlreadyAdded;
            }

            _ticketIds.Add(ticketId);

            return Result.Success();
        }

        public Result RemoveTicket(TicketId ticketId)
        {
            if (!_ticketIds.Contains(ticketId))
            {
                return QuestionErrors.TicketNotFound;
            }

            _ticketIds.Remove(ticketId);

            return Result.Success();
        }

        public void SetImageUri(ImageUri? imageUri)
        {
            ImageUri = imageUri;
        }

        public Result Update(
            QuestionContent content,
            QuestionExplanation explanation,
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
            QuestionExplanation explanation,
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
