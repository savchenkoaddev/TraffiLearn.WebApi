using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.ValueObjects.Topics;

namespace TraffiLearn.DomainTests.Factories
{
    internal sealed class TopicFixtureFactory
    {
        public static Topic CreateValidTopic()
        {
            return Topic.Create(
                new TopicId(Guid.NewGuid()),
                CreateValidNumber(),
                CreateValidTitle()).Value;
        }

        public static TopicNumber CreateValidNumber()
        {
            return TopicNumber.Create(TopicNumber.MinValue).Value;
        }

        public static TopicTitle CreateValidTitle()
        {
            return TopicTitle.Create("Title").Value;
        }
    }
}
