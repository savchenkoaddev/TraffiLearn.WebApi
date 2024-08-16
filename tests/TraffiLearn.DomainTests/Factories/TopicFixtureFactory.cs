using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;

namespace TraffiLearn.DomainTests.Factories
{
    internal static class TopicFixtureFactory
    {
        public static Domain.Aggregates.Topics.Topic CreateTopic()
        {
            return TopicId.Create(
                new TopicId(Guid.NewGuid()),
                CreateNumber(),
                CreateTitle()).Value;
        }

        public static TopicNumber CreateNumber()
        {
            return TopicNumber.Create(TopicNumber.MinValue).Value;
        }

        public static TopicTitle CreateTitle()
        {
            return TopicTitle.Create("Title").Value;
        }
    }
}
