using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Domain.Entities
{
    public sealed class Question : Entity
    {
        private const int MaxContentLength = 2000;
        private const int MaxExplanationLength = 2000;
        private const int MaxImageUriLength = 300;

        private readonly List<Topic> _topics = [];
        private readonly List<Answer> _answers = [];

        private Question(
            Guid id,
            string content,
            string explanation,
            QuestionTitleDetails questionTitleDetails,
            List<Answer> answers,
            List<Topic> topics,
            string? imageUri) : base(id)
        {
            Content = content;
            Explanation = explanation;
            TitleDetails = questionTitleDetails;
            ImageUri = imageUri;
            _answers = answers;
            _topics = topics;
        }

        public string Content { get; private set; }

        public string Explanation { get; private set; }

        public int LikesCount { get; private set; } = 0;

        public int DislikesCount { get; private set; } = 0;

        public QuestionTitleDetails TitleDetails { get; private set; }

        public IReadOnlyCollection<Topic> Topics => _topics;

        public IReadOnlyCollection<Answer> Answers => _answers;

        public string? ImageUri { get; private set; }

        public void AddAnswer(Answer? answer)
        {
            ArgumentNullException.ThrowIfNull(answer, nameof(answer));

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

        public void RemoveAnswer(Answer? answer)
        {
            ArgumentNullException.ThrowIfNull(answer, nameof(answer));

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

        public void AddTopic(Topic? topic)
        {
            ArgumentNullException.ThrowIfNull(topic, nameof(topic));

            if (_topics.Contains(topic))
            {
                throw new ArgumentException("The same topic has already been added.");
            }

            _topics.Add(topic);
            topic.AddQuestion(this);
        }

        public void RemoveTopic(Topic? topic)
        {
            ArgumentNullException.ThrowIfNull(topic, nameof(topic));

            if (!_topics.Contains(topic))
            {
                throw new ArgumentException("The question does not contain the provided topic.");
            }

            _topics.Remove(topic);
            topic.RemoveQuestion(questionId: Id);
        }

        public static Question Create(
            Guid id,
            string? content,
            string? explanation,
            QuestionTitleDetails? questionTitleDetails,
            List<Answer>? answers,
            List<Topic>? topics,
            string? imageUri)
        {
            ValidateForCreate(
                content,
                explanation,
                questionTitleDetails,
                answers,
                topics,
                imageUri);

            var question = new Question(
                id,
                content: content,
                explanation: explanation,
                questionTitleDetails: questionTitleDetails,
                answers: answers,
                topics: topics,
                imageUri: imageUri);

            foreach (var topic in topics)
            {
                topic.AddQuestion(question);
            }

            return question;
        }

        private static void ValidateForCreate(
            string? content,
            string? explanation,
            QuestionTitleDetails? questionTitleDetails,
            List<Answer>? answers,
            List<Topic>? topics,
            string? imageUri)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(content, nameof(content));
            ArgumentException.ThrowIfNullOrWhiteSpace(explanation, nameof(explanation));

            ArgumentNullException.ThrowIfNull(questionTitleDetails, nameof(questionTitleDetails));
            ArgumentNullException.ThrowIfNull(answers, nameof(answers));
            ArgumentNullException.ThrowIfNull(topics, nameof(topics));

            if (answers.Count == 0)
            {
                throw new ArgumentException("There are no answers in the question.");
            }

            if (topics.Count == 0)
            {
                throw new ArgumentException("There are no topics provided for the question.");
            }

            if (content.Length > MaxContentLength)
            {
                throw new ArgumentException($"The content length must be less than {MaxContentLength} characters.");
            }

            if (explanation.Length > MaxExplanationLength)
            {
                throw new ArgumentException($"The explanation length must be less than {MaxContentLength} characters.");
            }

            if (imageUri is not null &&
                (!Uri.IsWellFormedUriString(imageUri, UriKind.Absolute) ||
                imageUri.Length > MaxImageUriLength))
            {
                throw new ArgumentException($"Image uri is in an invalid format or exceeds {MaxImageUriLength} characters.");
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

            var uniqueTopics = new HashSet<Topic>(topics);

            if (uniqueTopics.Count != topics.Count)
            {
                throw new ArgumentException("There are duplicate topics in the provided question");
            }
        }
    }
}
