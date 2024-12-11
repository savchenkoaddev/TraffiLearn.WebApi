using TraffiLearn.Application.UseCases.Topics.Commands.Create;
using TraffiLearn.Domain.Users.Roles;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Topics.Queries.GetAllSortedTopicsByNumber
{
    public sealed class GetAllSortedTopicsByNumberTests : TopicIntegrationTest
    {
        public GetAllSortedTopicsByNumberTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task GetAllSortedTopicsByNumber_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await ApiTopicClient
                .SendGetAllTopicsSortedByNumberRequestAsync(
                    sentFromRole: null);

            response.AssertUnauthorizedStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetAllSortedTopicsByNumber_IfNoTopicsAdded_ShouldReturn200StatusCode(
            Role eligibleRole)
        {
            var response = await ApiTopicClient
                .SendGetAllTopicsSortedByNumberRequestAsync(
                    sentFromRole: eligibleRole);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetAllSortedTopicsByNumber_IfNoTopicsAdded_ShouldReturnEmptyCollection(
           Role eligibleRole)
        {
            var topics = await ApiTopicClient.GetAllTopicsSortedByNumberAsync(
                getWithRole: eligibleRole);

            topics.Should().NotBeNull();
            topics.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetAllSortedTopicsByNumber_IfValidCase_ShouldReturn200StatusCode(
           Role eligibleRole)
        {
            await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var response = await ApiTopicClient.SendGetAllTopicsSortedByNumberRequestAsync(
                sentFromRole: eligibleRole);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetAllSortedTopicsByNumber_IfValidCase_ShouldReturnSortedTopics(
           Role eligibleRole)
        {
            var secondTopicId = await ApiTopicClient.CreateTopicAsAuthorizedAsync(new CreateTopicCommand(
                TopicNumber: 5,
                Title: "title-5"));
            
            var firstTopicId = await ApiTopicClient.CreateTopicAsAuthorizedAsync(new CreateTopicCommand(
                TopicNumber: 3,
                Title: "title-3"));

            var lastTopicId = await ApiTopicClient.CreateTopicAsAuthorizedAsync(new CreateTopicCommand(
                TopicNumber: 123,
                Title: "title-123"));

            var allTopics = await ApiTopicClient.GetAllTopicsSortedByNumberAsync(
                getWithRole: eligibleRole);

            allTopics.First().Id.Should().Be(firstTopicId);
            allTopics.Last().Id.Should().Be(lastTopicId);
        }
    }
}
