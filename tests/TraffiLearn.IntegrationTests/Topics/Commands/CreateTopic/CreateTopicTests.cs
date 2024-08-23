using FluentAssertions;
using System.Net.Http.Json;
using TraffiLearn.Application.Topics.Commands.Create;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;
using TraffiLearn.IntegrationTests.Helpers;

namespace TraffiLearn.IntegrationTests.Topics.Commands.CreateTopic
{
    public sealed class CreateTopicTests : TopicIntegrationTest
    {
        public CreateTopicTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task CreateTopic_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await SendUnauthenticatedValidCreateTopicRequestAsync();

            response.AssertUnauthorizedStatusCode();
        }

        [Fact]
        public async Task CreateTopic_IfUserNotAuthenticated_TopicShouldNotBeCreated()
        {
            var response = await SendUnauthenticatedValidCreateTopicRequestAsync();

            var topics = await ApiTopicClient.GetAllTopicsSortedByNumberAsync();

            topics.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task CreateTopic_IfUserIsNotEligible_ShouldReturn403StatusCode(
            Role role)
        {
            var response = await SendValidCreateTopicRequestWithRoleAsync(role);

            response.AssertForbiddenStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task CreateTopic_IfUserIsNotEligible_TopicShouldNotBeCreated(
            Role role)
        {
            var response = await SendValidCreateTopicRequestWithRoleAsync(role);

            var topics = await ApiTopicClient.GetAllTopicsSortedByNumberAsync();

            topics.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateTopic_IfPassedInvalidArgsAndUserIsEligible_ShouldReturn400StatusCode(
            Role role)
        {
            var invalidCommands = CreateTopicFixtureFactory.GetInvalidCommands();

            foreach (var command in invalidCommands)
            {
                var response = await SendCreateTopicRequestWithRoleAsync(
                    role: role,
                    command);

                response.AssertBadRequestStatusCode();
            }
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateTopic_IfPassedInvalidArgsAndUserIsEligible_TopicShouldNotBeCreated(
            Role role)
        {
            var invalidCommands = CreateTopicFixtureFactory.GetInvalidCommands();

            foreach (var command in invalidCommands)
            {
                await SendCreateTopicRequestWithRoleAsync(
                    role: role,
                    command);

                var topics = await ApiTopicClient.GetAllTopicsSortedByNumberAsync();

                topics.Should().BeEmpty();
            }
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateTopic_IfPassedValidArgsAndUserIsEligible_ShouldReturn201Or204StatusCode(
            Role role)
        {
            var response = await SendValidCreateTopicRequestWithRoleAsync(role);

            response.AssertCreatedOrNoContentStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateTopic_IfPassedValidArgsAndUserIsEligible_TopicShouldBeCreated(
            Role role)
        {
            var command = CreateTopicFixtureFactory.CreateValidCommand();

            var response = await SendCreateTopicRequestWithRoleAsync(
                role: role,
                command);

            var topics = await ApiTopicClient.GetAllTopicsSortedByNumberAsync();

            topics.Should().HaveCount(1);
            topics.First().Title.Should().Be(command.Title);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateTopic_IfPassedValidArgsAndUserIsEligible_ShouldReturnId(
           Role role)
        {
            var command = CreateTopicFixtureFactory.CreateValidCommand();

            var response = await SendCreateTopicRequestWithRoleAsync(
                role: role,
                command);

            var content = await response.Content.ReadFromJsonAsync<string>();

            content.Should().NotBeNull();

            var isGuid = Guid.TryParse(content, out var _);

            isGuid.Should().BeTrue();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateTopic_IfPassedValidArgsAndUserIsEligible_ShouldReturnValidTopicId(
           Role role)
        {
            var command = CreateTopicFixtureFactory.CreateValidCommand();

            var response = await SendCreateTopicRequestWithRoleAsync(
                role: role,
                command);

            var topicId = await response.Content.ReadFromJsonAsync<Guid>();

            var allTopics = await ApiTopicClient.GetAllTopicsSortedByNumberAsync();

            allTopics.Should().HaveCount(1);
            allTopics.First().Id.Should().Be(topicId);
        }

        private async Task<HttpResponseMessage> SendValidCreateTopicRequestWithRoleAsync(
           Role role)
        {
            var command = CreateTopicFixtureFactory.CreateValidCommand();

            return await RequestSender
                .SendJsonRequest(
                    method: HttpMethod.Post,
                    requestUri: TopicEndpointRoutes.CreateTopicRoute,
                    command,
                    sentWithRole: role);
        }

        private async Task<HttpResponseMessage> SendUnauthenticatedValidCreateTopicRequestAsync()
        {
            var command = CreateTopicFixtureFactory.CreateValidCommand();

            return await RequestSender
                .SendJsonRequest(
                    method: HttpMethod.Post,
                    requestUri: TopicEndpointRoutes.CreateTopicRoute,
                    command);
        }

        private async Task<HttpResponseMessage> SendCreateTopicRequestWithRoleAsync(
            Role role,
            CreateTopicCommand command)
        {
            return await RequestSender
                .SendJsonRequest(
                    method: HttpMethod.Post,
                    requestUri: TopicEndpointRoutes.CreateTopicRoute,
                    command,
                    sentWithRole: role);
        }
    }
}

