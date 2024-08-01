using TraffiLearn.Domain.Errors.Topics;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Topics;

namespace TraffiLearn.Domain.Entities
{
    public sealed class Topic : Entity
    {
        private readonly List<Question> _questions = [];

        private Topic(Guid id)
            : base(id)
        { }

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

        public Result AddQuestion(Question question)
        {
            if (_questions.Contains(question))
            {
                return TopicErrors.QuestionAlreadyAdded;
            }

            _questions.Add(question);

            if (!question.Topics.Contains(this))
            {
                question.AddTopic(this);
            }

            return Result.Success();
        }

        public Result RemoveQuestion(Question question)
        {
            if (!_questions.Contains(question))
            {
                return TopicErrors.QuestionNotFound;
            }

            _questions.Remove(question);

            if (question.Topics.Contains(this))
            {
                question.RemoveTopic(this);
            }

            return Result.Success();
        }

        public Result Update(
            TopicNumber number,
            TopicTitle title)
        {
            Number = number;
            Title = title;

            return Result.Success();
        }

        public static Result<Topic> Create(
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
