using TraffiLearn.Domain.Errors.Topics;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Topics;

namespace TraffiLearn.Domain.Entities
{
    public sealed class Topic : Entity<TopicId>
    {
        private TopicNumber _topicNumber;
        private TopicTitle _topicTitle;
        private readonly HashSet<Question> _questions = [];

        private Topic()
            : base(new(Guid.Empty))
        { }

        private Topic(
            TopicId topicId,
            TopicNumber number,
            TopicTitle title) : base(topicId)
        {
            Number = number;
            Title = title;
        }

        public TopicNumber Number
        {
            get 
            {
                return _topicNumber; 
            }
            private set
            {
                ArgumentNullException.ThrowIfNull(value, nameof(value));

                _topicNumber = value;
            }
        }

        public TopicTitle Title
        {
            get
            {
                return _topicTitle;
            }
            private set
            {
                ArgumentNullException.ThrowIfNull(value, nameof(value));

                _topicTitle = value;
            }
        }
        
        public IReadOnlyCollection<Question> Questions => _questions;

        public Result AddQuestion(Question question)
        {
            ArgumentNullException.ThrowIfNull(question, nameof(question));

            if (_questions.Contains(question))
            {
                return TopicErrors.QuestionAlreadyAdded;
            }

            _questions.Add(question);

            return Result.Success();
        }

        public Result RemoveQuestion(Question question)
        {
            ArgumentNullException.ThrowIfNull(question, nameof(question));

            if (!_questions.Contains(question))
            {
                return TopicErrors.QuestionNotFound;
            }

            _questions.Remove(question);

            return Result.Success();
        }

        public Result Update(
            TopicNumber topicNumber,
            TopicTitle topicTitle)
        {
            Number = topicNumber;
            Title = topicTitle;

            return Result.Success();
        }

        public static Result<Topic> Create(
            TopicId topicId,
            TopicNumber number,
            TopicTitle title)
        {
            return new Topic(
                topicId: topicId,
                number: number,
                title: title);
        }
    }
}
