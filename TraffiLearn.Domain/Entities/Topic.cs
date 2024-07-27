using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Domain.Entities
{
    public sealed class Topic : Entity
    {
        private readonly List<Question> _questions = [];

        private Topic(
            TopicId id,
            TopicNumber number,
            TopicTitle title) : base(id.Value)
        {
            Number = number;
            Title = title;
        }

        public TopicNumber Number { get; private set; }

        public TopicTitle Title { get; private set; }

        public IReadOnlyCollection<Question> Questions => _questions;

        public void AddQuestion(Question question)
        {
            if (_questions.Contains(question))
            {
                throw new ArgumentException("The topic already contains the provided question.");
            }

            _questions.Add(question);
            question.AddTopic(this);
        }

        public void RemoveQuestion(QuestionId questionId)
        {
            var question = _questions.FirstOrDefault(q => q.Id == questionId.Value);

            if (question is null)
            {
                throw new ArgumentException("The topic does not contain the provided question.");
            }

            _questions.Remove(question);
            question.RemoveTopic(new TopicId(Id));
        }

        public static Topic Create(
            TopicId id,
            TopicNumber number,
            TopicTitle title)
        {
            return new Topic(
                id: id,
                number: number,
                title: title);
        }
    }

    public sealed record TopicId(Guid Value);
}
