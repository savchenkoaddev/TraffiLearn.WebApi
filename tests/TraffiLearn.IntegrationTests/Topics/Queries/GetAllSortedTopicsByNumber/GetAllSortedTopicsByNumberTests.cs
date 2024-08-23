using FluentAssertions;
using TraffiLearn.Application.Topics.Commands.Create;
using TraffiLearn.Application.Topics.DTO;
using TraffiLearn.Domain.Aggregates.Users.Enums;
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
            var response = await RequestSender.GetAsync(
                requestUri: TopicEndpointRoutes.GetAllSortedTopicsByNumberRoute);

            response.AssertUnauthorizedStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetAllSortedTopicsByNumber_IfUserIsEligibleButNoTopicsAdded_ShouldReturn200StatusCode(
            Role role)
        {
            var response = await RequestSender.GetAsync(
                requestUri: TopicEndpointRoutes.GetAllSortedTopicsByNumberRoute,
                getFromRole: role);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetAllSortedTopicsByNumber_IfUserIsEligibleButNoTopicsAdded_ShouldReturnEmptyCollection(
           Role role)
        {
            var topics = await RequestSender.GetFromJsonAsync<IEnumerable<TopicResponse>>(
                requestUri: TopicEndpointRoutes.GetAllSortedTopicsByNumberRoute,
                getFromRole: role);

            topics.Should().NotBeNull();
            topics.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetAllSortedTopicsByNumber_IfValidCase_ShouldReturn200StatusCode(
           Role role)
        {
            await ApiTopicClient.CreateValidTopicAsync();

            var response = await RequestSender.GetAsync(
                requestUri: TopicEndpointRoutes.GetAllSortedTopicsByNumberRoute,
                getFromRole: role);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetAllSortedTopicsByNumber_IfValidCase_ShouldReturnSortedTopics(
           Role role)
        {
            var secondTopicId = await ApiTopicClient.CreateTopicAsync(new CreateTopicCommand(
                TopicNumber: 5,
                Title: "title-5"));

            var firstTopicId = await ApiTopicClient.CreateTopicAsync(new CreateTopicCommand(
                TopicNumber: 3,
                Title: "title-3"));

            var lastTopicId = await ApiTopicClient.CreateTopicAsync(new CreateTopicCommand(
                TopicNumber: 123,
                Title: "title-123"));

            var allTopics = await RequestSender.GetFromJsonAsync<IEnumerable<TopicResponse>>(
                requestUri: TopicEndpointRoutes.GetAllSortedTopicsByNumberRoute,
                getFromRole: role);

            allTopics.First().Id.Should().Be(firstTopicId);
            allTopics.Last().Id.Should().Be(lastTopicId);
        }
    }
}
