using TraffiLearn.Application.Topics.Queries.GetById;

namespace TraffiLearn.IntegrationTests.Topics.Queries.GetTopicById
{
    internal static class GetTopicByIdFixtureFactory
    {
        public static GetTopicByIdQuery CreateQueryWithRandomId()
        {
            return new GetTopicByIdQuery(
                TopicId: Guid.NewGuid());
        }
    }
}
