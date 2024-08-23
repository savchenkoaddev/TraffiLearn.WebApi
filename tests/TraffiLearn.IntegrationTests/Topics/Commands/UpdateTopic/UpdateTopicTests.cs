using FluentAssertions;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;
using TraffiLearn.IntegrationTests.Helpers;

namespace TraffiLearn.IntegrationTests.Topics.Commands.UpdateTopic
{
    public sealed class UpdateTopicTests : TopicIntegrationTest
    {
        public UpdateTopicTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task UpdateTopic_IfUserNotAuthenticated_ShouldReturn401StatusCode()
        {
            var command = UpdateTopicFixtureFactory.CreateValidCommandWithRandomId();

            var response = await RequestSender.SendJsonRequest(
                method: HttpMethod.Put,
                requestUri: TopicEndpointRoutes.UpdateTopicRoute,
                command);

            response.AssertUnauthorizedStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task UpdateTopic_IfUserIsNotEligible_ShouldReturn403StatusCode(
            Role role)
        {
            var command = UpdateTopicFixtureFactory.CreateValidCommandWithRandomId();

            var response = await RequestSender.SendJsonRequest(
                method: HttpMethod.Put,
                requestUri: TopicEndpointRoutes.UpdateTopicRoute,
                command,
                sentWithRole: role);

            response.AssertForbiddenStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateTopic_IfPassedInvalidArgsAndUserIsEligible_ShouldReturn400StatusCode(
            Role role)
        {
            var invalidCommands = UpdateTopicFixtureFactory.CreateInvalidCommandsWithRandomIds();

            foreach (var command in invalidCommands)
            {
                var response = await RequestSender.SendJsonRequest(
                    method: HttpMethod.Put,
                    requestUri: TopicEndpointRoutes.UpdateTopicRoute,
                    command,
                    sentWithRole: role);

                response.AssertBadRequestStatusCode();
            }
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateTopic_IfPassedValidArgsAndUserIsEligibleButTopicDoesNotExist_ShouldReturn404StatusCode(
            Role role)
        {
            var command = UpdateTopicFixtureFactory.CreateValidCommandWithRandomId();

            var response = await RequestSender.SendJsonRequest(
                method: HttpMethod.Put,
                requestUri: TopicEndpointRoutes.UpdateTopicRoute,
                command,
                sentWithRole: role);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateTopic_IfPassedValidArgsAndUserIsEligible_ShouldReturn204StatusCode(
            Role role)
        {
            await TopicRequestSender.CreateValidTopicAsync();

            var allTopics = await TopicRequestSender.GetAllTopicsSortedByNumberAsync();

            var firstTopicId = allTopics.First().Id;

            var command = UpdateTopicFixtureFactory.CreateValidCommand(
                topicId: firstTopicId);

            var response = await RequestSender.SendJsonRequest(
                method: HttpMethod.Put,
                requestUri: TopicEndpointRoutes.UpdateTopicRoute,
                command,
                sentWithRole: role);

            response.AssertNoContentStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateTopic_IfPassedValidArgsAndUserIsEligible_TopicShouldBeUpdated(
            Role role)
        {
            var topicId = await TopicRequestSender.CreateValidTopicAsync();

            var command = UpdateTopicFixtureFactory.CreateValidCommand(
                topicId: topicId);

            await RequestSender.SendJsonRequest(
                method: HttpMethod.Put,
                requestUri: TopicEndpointRoutes.UpdateTopicRoute,
                command,
                sentWithRole: role);

            var allTopics = await TopicRequestSender.GetAllTopicsSortedByNumberAsync();

            allTopics.First().Title.Should().Be(command.Title);
            allTopics.First().Id.Should().Be(topicId);
        }
    }
}
