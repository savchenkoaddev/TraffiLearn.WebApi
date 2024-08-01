using TraffiLearn.Domain.Errors.Questions;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Question;

namespace TraffiLearn.Domain.Entities
{
    public sealed class Question : Entity
    {
        private List<Answer> _answers = [];
        private readonly List<Topic> _topics = [];
        private readonly List<Ticket> _tickets = [];

        private Question(Guid id)
            : base(id)
        { }

        private Question(
            QuestionId id,
            QuestionContent content,
            QuestionExplanation explanation,
            QuestionNumber questionNumber,
            List<Answer> answers,
            ImageUri? imageUri) : base(id.Value)
        {
            Content = content;
            Explanation = explanation;
            QuestionNumber = questionNumber;
            _answers = answers;
            ImageUri = imageUri;
        }

        public QuestionContent Content { get; private set; }

        public QuestionExplanation Explanation { get; private set; }

        public QuestionNumber QuestionNumber { get; private set; }

        public ImageUri? ImageUri { get; private set; }

        public int LikesCount { get; private set; } = 0;

        public int DislikesCount { get; private set; } = 0;

        public IReadOnlyCollection<Topic> Topics => _topics;

        public IReadOnlyCollection<Answer> Answers => _answers;

        public IReadOnlyCollection<Ticket> Tickets => _tickets;

        public Result AddAnswer(Answer answer)
        {
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
            if (_topics.Contains(topic))
            {
                return QuestionErrors.TopicAlreadyAdded;
            }

            _topics.Add(topic);

            if (!topic.Questions.Contains(this))
            {
                var addResult = topic.AddQuestion(this);

                if (addResult.IsFailure)
                {
                    return addResult.Error;
                }
            }

            return Result.Success();
        }

        public Result RemoveTopic(Topic topic)
        {
            if (!_topics.Contains(topic))
            {
                return QuestionErrors.TopicNotFound;
            }

            _topics.Remove(topic);

            if (topic.Questions.Contains(this))
            {
                var removeResult = topic.RemoveQuestion(this);

                if (removeResult.IsFailure)
                {
                    return removeResult.Error;
                }
            }

            return Result.Success();
        }

        public Result AddTicket(Ticket ticket)
        {
            if (_tickets.Contains(ticket))
            {
                return QuestionErrors.TicketAlreadyAdded;
            }

            _tickets.Add(ticket);

            if (!ticket.Questions.Contains(this))
            {
                var addResult = ticket.AddQuestion(this);

                if (addResult.IsFailure)
                {
                    return addResult.Error;
                }
            }

            return Result.Success();
        }

        public Result RemoveTicket(Ticket ticket)
        {
            if (!_tickets.Contains(ticket))
            {
                return QuestionErrors.TicketNotFound;
            }

            _tickets.Add(ticket);

            if (ticket.Questions.Contains(this))
            {
                var addResult = ticket.RemoveQuestion(this);

                if (addResult.IsFailure)
                {
                    return addResult.Error;
                }
            }

            return Result.Success();
        }

        public void SetImageUri(ImageUri? imageUri)
        {
            ImageUri = imageUri;
        }

        public Result Update(
            QuestionContent content,
            QuestionExplanation explanation,
            QuestionNumber questionNumber,
            List<Answer> answers,
            ImageUri? imageUri)
        {
            var validationResult = ValidateAnswers(answers);

            if (validationResult.IsFailure)
            {
                return Result.Failure<Question>(validationResult.Error);
            }

            Content = content;
            Explanation = explanation;
            QuestionNumber = questionNumber;
            _answers = answers;
            ImageUri = imageUri;

            return Result.Success();
        }

        public static Result<Question> Create(
            QuestionId id,
            QuestionContent content,
            QuestionExplanation explanation,
            QuestionNumber questionNumber,
            List<Answer> answers,
            ImageUri? imageUri)
        {
            var answersValidationResult = ValidateAnswers(answers);

            if (answersValidationResult.IsFailure)
            {
                return Result.Failure<Question>(answersValidationResult.Error);
            }

            return new Question(
                id,
                content,
                explanation,
                questionNumber,
                answers,
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

    public sealed record QuestionId(Guid Value);
}
