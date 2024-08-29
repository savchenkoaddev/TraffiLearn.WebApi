using FluentAssertions;
using System.Net.Http.Json;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Topics.Commands.CreateTopic
{
    public sealed class CreateTopicTests : TopicIntegrationTest
    {
        private readonly CreateTopicCommandFactory _commandFactory = new();

        public CreateTopicTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task CreateTopic_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await ApiTopicClient.SendCreateTopicRequestAsync(
                sentFromRole: null);

            response.AssertUnauthorizedStatusCode();
        }

        [Fact]
        public async Task CreateTopic_IfUserIsNotAuthenticated_TopicShouldNotBeCreated()
        {
            await ApiTopicClient.SendCreateTopicRequestAsync(
                sentFromRole: null);

            var allTopics = await ApiTopicClient.GetAllTopicsSortedByNumberAsAuthorizedAsync();

            allTopics.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task CreateTopic_IfUserIsNotEligible_ShouldReturn403StatusCode(
            Role nonEligibleRole)
        {
            var response = await ApiTopicClient.SendCreateTopicRequestAsync(
                sentFromRole: nonEligibleRole);

            response.AssertForbiddenStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task CreateTopic_IfUserIsNotEligible_TopicShouldNotBeCreated(
            Role eligibleRole)
        {
            var response = await ApiTopicClient.SendCreateTopicRequestAsync(
                sentFromRole: eligibleRole);

            var allTopics = await ApiTopicClient.GetAllTopicsSortedByNumberAsAuthorizedAsync();

            allTopics.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateTopic_IfPassedInvalidArgs_ShouldReturn400StatusCode(
            Role eligibleRole)
        {
            var invalidCommands = _commandFactory.GetInvalidCommands();

            await RequestSender.EnsureEachSentJsonRequestReturnsBadRequestAsync(
                method: HttpMethod.Post,
                requestUri: TopicEndpointRoutes.CreateTopicRoute,
                requests: invalidCommands,
                sentFromRole: eligibleRole);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateTopic_IfPassedInvalidArgs_TopicShouldNotBeCreated(
            Role eligibleRole)
        {
            var invalidCommands = _commandFactory.GetInvalidCommands();

            await RequestSender.SendAllAsJsonAsync(
                method: HttpMethod.Post,
                requestUri: TopicEndpointRoutes.CreateTopicRoute,
                requests: invalidCommands,
                sentFromRole: eligibleRole);

            var allTopics = await ApiTopicClient.GetAllTopicsSortedByNumberAsAuthorizedAsync();

            allTopics.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateTopic_IfValidCase_ShouldReturn204Or201StatusCode(
            Role eligibleRole)
        {
            var response = await ApiTopicClient.SendCreateTopicRequestAsync(
                sentFromRole: eligibleRole);

            response.AssertCreatedOrNoContentStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateTopic_IfValidCase_TopicShouldBeCreated(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateTopicAsync(
                createdWithRole: eligibleRole);

            var allTopics = await ApiTopicClient.GetAllTopicsSortedByNumberAsAuthorizedAsync();

            allTopics.Should().HaveCount(1);
            allTopics.Single().Id.Should().Be(topicId);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateTopic_IfValidCase_ShouldReturnValidTopicId(
           Role eligibleRole)
        {
            var response = await ApiTopicClient.SendCreateTopicRequestAsync(
                sentFromRole: eligibleRole);

            var content = await response.Content.ReadFromJsonAsync<string>();

            content.Should().NotBeNullOrWhiteSpace();

            var isGuid = Guid.TryParse(content, out var id);

            isGuid.Should().BeTrue();
            id.Should().NotBe(Guid.Empty);

            var allTopics = await ApiTopicClient.GetAllTopicsSortedByNumberAsAuthorizedAsync();

            allTopics.Single().Id.Should().Be(id);
        }
    }
}

