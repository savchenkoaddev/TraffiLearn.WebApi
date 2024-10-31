using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Topics.TopicNumbers;
using TraffiLearn.Domain.Aggregates.Topics.TopicTitles;
using TraffiLearn.Domain.Common.ImageUris;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Topics
{
    public sealed class Topic : AggregateRoot<TopicId>
    {
        private readonly HashSet<Question> _questions = [];
        private TopicNumber _topicNumber;
        private TopicTitle _topicTitle;

        private Topic()
            : base(new(Guid.Empty))
        { }

        private Topic(
            TopicId topicId,
            TopicNumber number,
            TopicTitle title,
            ImageUri? imageUri) : base(topicId)
        {
            Number = number;
            Title = title;
            ImageUri = imageUri;
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

        public ImageUri? ImageUri { get; private set; }

        public IReadOnlyCollection<Question> Questions => _questions;

        public void SetImageUri(ImageUri? imageUri)
        {
            ImageUri = imageUri;
        }

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
            TopicTitle title,
            ImageUri? imageUri = null)
        {
            return new Topic(
                topicId: topicId,
                number: number,
                title: title,
                imageUri);
        }
    }
}
