using TraffiLearn.IntegrationTests.Abstractions;

namespace TraffiLearn.IntegrationTests.Topics.Queries
{
    public sealed class GetAllSortedTopicsByNumberTests : BaseIntegrationTest
    {
        public GetAllSortedTopicsByNumberTests(
            WebApplicationFactory factory)
            : base(factory)
        {
        }

        //[Fact]
        //public async Task GetAllSortedTopicsByNumber_IfNoTopicsInStorage_ShouldBeSuccesful()
        //{
        //    var result = await Sender.Send(new GetAllSortedTopicsByNumberQuery());

        //    result.IsSuccess.Should().BeTrue();
        //}

        //[Fact]
        //public async Task GetAllSortedTopicsByNumber_IfNoTopicsInStorage_ShouldReturnEmptyCollection()
        //{
        //    var topics = (await Sender.Send(new GetAllSortedTopicsByNumberQuery())).Value;

        //    topics.Should().NotBeNull();
        //    topics.Should().BeEmpty();
        //}

        //[Fact]
        //public async Task GetAllSortedTopicsByNumber_IfValidCase_ShouldBeSuccesful()
        //{
        //    await _topicTestHelper.CreateTopicAsync(
        //        number: 1);
        //    await _topicTestHelper.CreateTopicAsync(
        //        number: 8);
        //    await _topicTestHelper.CreateTopicAsync(
        //        number: 6);

        //    var topics = (await Sender.Send(new GetAllSortedTopicsByNumberQuery())).Value;

        //    topics.Should().NotBeEmpty();
        //    topics.Select(t => t.TopicNumber).Should().BeInAscendingOrder();
        //}
    }
}
