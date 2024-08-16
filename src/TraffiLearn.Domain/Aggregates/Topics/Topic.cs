using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Topics.Errors;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Topics
{
    public sealed class Topic : AggregateRoot<TopicId>
    {
        private readonly HashSet<QuestionId> _questionIds = [];
        private TopicNumber _topicNumber;
        private TopicTitle _topicTitle;

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

        public IReadOnlyCollection<QuestionId> QuestionIds => _questionIds;

        public Result AddQuestion(QuestionId questionId)
        {
            if (_questionIds.Contains(questionId))
            {
                return TopicErrors.QuestionAlreadyAdded;
            }

            _questionIds.Add(questionId);

            return Result.Success();
        }

        public Result RemoveQuestion(QuestionId questionId)
        {
            if (!_questionIds.Contains(questionId))
            {
                return TopicErrors.QuestionNotFound;
            }

            _questionIds.Remove(questionId);

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
