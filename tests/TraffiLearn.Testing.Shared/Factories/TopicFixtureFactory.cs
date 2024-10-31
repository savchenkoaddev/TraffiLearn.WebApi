using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Aggregates.Topics.TopicNumbers;
using TraffiLearn.Domain.Aggregates.Topics.TopicTitles;

namespace TraffiLearn.Testing.Shared.Factories
{
    public static class TopicFixtureFactory
    {
        public static Topic CreateTopic(int number = TopicNumber.MinValue,
            string title = "Title")
        {
            return Topic.Create(
                new TopicId(Guid.NewGuid()),
                CreateNumber(number),
                CreateTitle(title),
                null).Value;
        }

        public static TopicNumber CreateNumber(int number = TopicNumber.MinValue)
        {
            return TopicNumber.Create(TopicNumber.MinValue).Value;
        }

        public static TopicTitle CreateTitle(string title = "Title")
        {
            return TopicTitle.Create(title).Value;
        }
    }
}
