using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.Entities
{
    public sealed class Topic : Entity
    {
        private const int MaxTitleLength = 300;
        private readonly List<Question> _questions = [];

        private Topic(
            Guid id,
            int number,
            string title) : base(id)
        {
            if (number <= 0)
            {
                throw new ArgumentException("Topic number must be more than 0.");
            }

            if (title.Length > MaxTitleLength)
            {
                throw new ArgumentException($"Provided title length exceeds {MaxTitleLength} characters.");
            }

            Number = number;
            Title = title;
        }

        public int Number { get; private set; }

        public string Title { get; private set; }

        public IReadOnlyCollection<Question> Questions => _questions;

        public void AddQuestion(Question? question)
        {
            ArgumentNullException.ThrowIfNull(question, nameof(question));

            if (_questions.Contains(question))
            {
                throw new ArgumentException("The topic already contains the provided question.");
            }

            _questions.Add(question);
            question.AddTopic(this);
        }

        public void RemoveQuestion(Guid? questionId)
        {
            ArgumentNullException.ThrowIfNull(questionId, nameof(questionId));

            var question = _questions.Find(q => q.Id == questionId);

            if (question is null)
            {
                throw new ArgumentException("The topic does not contain the provided question.");
            }

            _questions.Remove(question);
            question.RemoveTopic(this);
        }

        public static Topic Create(
            Guid id,
            int? number,
            string? title)
        {
            ArgumentNullException.ThrowIfNull(number);
            ArgumentException.ThrowIfNullOrWhiteSpace(title);

            if (number <= 0)
            {
                throw new ArgumentException("Topic number must be more than 0.");
            }

            if (title.Length > MaxTitleLength)
            {
                throw new ArgumentException($"Provided title length exceeds {MaxTitleLength} characters.");
            }

            return new Topic(
                id: id,
                number: number.Value,
                title: title);
        }
    }
}
