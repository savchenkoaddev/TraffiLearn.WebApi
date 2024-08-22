using TraffiLearn.IntegrationTests.Abstractions;

namespace TraffiLearn.IntegrationTests.Topics
{
    public class TopicIntegrationTest : BaseIntegrationTest
    {
        protected readonly AuthorizedTopicRequestSender TopicRequestSender;

        public TopicIntegrationTest(
            WebApplicationFactory factory)
            : base(factory)
        {
            TopicRequestSender = new AuthorizedTopicRequestSender(RequestSender);
        }
    }
}
