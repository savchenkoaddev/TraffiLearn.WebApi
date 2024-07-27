using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Domain.Entities
{
    public sealed class Question : Entity
    {
        private QuestionContent _content;
        private QuestionExplanation _explanation;
        private TicketNumber _ticketNumber;
        private QuestionNumber _questionNumber;
        private ImageUri? _imageUri;
        private List<Answer> _answers = [];
        private readonly List<Topic> _topics = [];

        private Question(
            QuestionId id,
            QuestionContent content,
            QuestionExplanation explanation,
            TicketNumber ticketNumber,
            QuestionNumber questionNumber,
            List<Answer> answers,
            ImageUri? imageUri) : base(id.Value)
        {
            _content = content;
            _explanation = explanation;
            _ticketNumber = ticketNumber;
            _questionNumber = questionNumber;
            _answers = answers;
            _imageUri = imageUri;
        }

        public QuestionContent Content { get; private set; }

        public QuestionExplanation Explanation { get; private set; }

        public TicketNumber TicketNumber { get; private set; }

        public QuestionNumber QuestionNumber { get; private set; }

        public ImageUri? ImageUri { get; private set; }

        public int LikesCount { get; private set; } = 0;

        public int DislikesCount { get; private set; } = 0;

        public IReadOnlyCollection<Topic> Topics => _topics;

        public IReadOnlyCollection<Answer> Answers => _answers;

        public void AddAnswer(Answer answer)
        {
            if (_answers.Contains(answer))
            {
                throw new ArgumentException("The question already contains the answer with the provided text");
            }

            if (_answers.Count == 0 &&
                answer.IsCorrect == false)
            {
                throw new ArgumentException("Newly added answer cannot be incorrect if there are no existing answers in a question.");
            }

            _answers.Add(answer);
        }

        public void RemoveAnswer(Answer answer)
        {
            if (!_answers.Contains(answer))
            {
                throw new ArgumentException("The question does not contain the answer with the provided text.");
            }

            if (_answers.Count(q => q.IsCorrect == true) <= 1 &&
                answer.IsCorrect == true)
            {
                throw new ArgumentException("Cannot delete a single correct answer in the question");
            }

            _answers.Remove(answer);
        }

        public void AddTopic(Topic topic)
        {
            if (_topics.Contains(topic))
            {
                throw new ArgumentException("The same topic has already been added.");
            }

            _topics.Add(topic);
            topic.AddQuestion(this);
        }

        public void RemoveTopic(TopicId topicId)
        {
            var topic = _topics.FirstOrDefault(t => t.Id == topicId.Value);

            if (topic is null)
            {
                throw new ArgumentException("The question does not contain the provided topic.");
            }

            _topics.Remove(topic);
            topic.RemoveQuestion(new QuestionId(Id));
        }

        public void SetImageUri(ImageUri? imageUri)
        {
            ImageUri = imageUri;
        }

        public static Question Create(
            QuestionId id,
            QuestionContent content,
            QuestionExplanation explanation,
            TicketNumber ticketNumber,
            QuestionNumber questionNumber,
            List<Answer> answers,
            ImageUri? imageUri)
        {
            ValidateAnswers(answers);

            return new Question(
                id,
                content,
                explanation,
                ticketNumber,
                questionNumber,
                answers,
                imageUri);
        }

        private static void ValidateAnswers(List<Answer> answers)
        {
            if (answers.Count == 0)
            {
                throw new ArgumentException("There are no answers in the question.");
            }

            if (answers.All(a => a.IsCorrect == false))
            {
                throw new AllAnswersAreIncorrectException();
            }

            var uniqueAnswers = new HashSet<Answer>(answers);

            if (uniqueAnswers.Count != answers.Count)
            {
                throw new ArgumentException("There are duplicate answers in the provided question");
            }
        }
    }

    public sealed record QuestionId(Guid Value);
}
