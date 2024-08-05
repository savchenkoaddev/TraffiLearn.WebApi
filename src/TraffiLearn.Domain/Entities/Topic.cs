using TraffiLearn.Domain.Errors.Topics;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Topics;

namespace TraffiLearn.Domain.Entities
{
    public sealed class Topic : Entity
    {
        private readonly HashSet<Question> _questions = [];

        private Topic(Guid id)
            : base(id)
        { }

        private Topic(
            Guid id,
            TopicNumber number,
            TopicTitle title) : base(id)
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

            return Result.Success();
        }

        public Result RemoveQuestion(Question question)
        {
            if (!_questions.Contains(question))
            {
                return TopicErrors.QuestionNotFound;
            }

            _questions.Remove(question);

            return Result.Success();
        }

        public Result Update(Topic topic)
        {
            Number = topic.Number;
            Title = topic.Title;

            return Result.Success();
        }

        public static Result<Topic> Create(
            Guid id,
            TopicNumber number,
            TopicTitle title)
        {
            return new Topic(
                id: id,
                number: number,
                title: title);
        }
    }
}
