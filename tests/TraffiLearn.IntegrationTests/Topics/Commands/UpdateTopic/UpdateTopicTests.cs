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
        public async Task UpdateTopic_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var command = UpdateTopicCommandFactory.CreateValidCommandWithRandomId();

            var response = await RequestSender.SendJsonAsync(
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
            var command = UpdateTopicCommandFactory.CreateValidCommandWithRandomId();

            var response = await RequestSender.SendJsonAsync(
                method: HttpMethod.Put,
                requestUri: TopicEndpointRoutes.UpdateTopicRoute,
                command,
                sentFromRole: role);

            response.AssertForbiddenStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateTopic_IfPassedInvalidArgsAndUserIsEligible_ShouldReturn400StatusCode(
            Role role)
        {
            var invalidCommands = UpdateTopicCommandFactory.CreateInvalidCommandsWithRandomIds();

            foreach (var command in invalidCommands)
            {
                var response = await RequestSender.SendJsonAsync(
                    method: HttpMethod.Put,
                    requestUri: TopicEndpointRoutes.UpdateTopicRoute,
                    command,
                    sentFromRole: role);

                response.AssertBadRequestStatusCode();
            }
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateTopic_IfPassedValidArgsAndUserIsEligibleButTopicDoesNotExist_ShouldReturn404StatusCode(
            Role role)
        {
            var command = UpdateTopicCommandFactory.CreateValidCommandWithRandomId();

            var response = await RequestSender.SendJsonAsync(
                method: HttpMethod.Put,
                requestUri: TopicEndpointRoutes.UpdateTopicRoute,
                command,
                sentFromRole: role);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateTopic_IfPassedValidArgsAndUserIsEligible_ShouldReturn204StatusCode(
            Role role)
        {
            await ApiTopicClient.CreateValidTopicAsync();

            var allTopics = await ApiTopicClient.GetAllTopicsSortedByNumberAsync();

            var firstTopicId = allTopics.First().Id;

            var command = UpdateTopicCommandFactory.CreateValidCommand(
                topicId: firstTopicId);

            var response = await RequestSender.SendJsonAsync(
                method: HttpMethod.Put,
                requestUri: TopicEndpointRoutes.UpdateTopicRoute,
                command,
                sentFromRole: role);

            response.AssertNoContentStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateTopic_IfPassedValidArgsAndUserIsEligible_TopicShouldBeUpdated(
            Role role)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsync();

            var command = UpdateTopicCommandFactory.CreateValidCommand(
                topicId: topicId);

            await RequestSender.SendJsonAsync(
                method: HttpMethod.Put,
                requestUri: TopicEndpointRoutes.UpdateTopicRoute,
                command,
                sentFromRole: role);

            var allTopics = await ApiTopicClient.GetAllTopicsSortedByNumberAsync();

            allTopics.First().Title.Should().Be(command.Title);
            allTopics.First().Id.Should().Be(topicId);
        }
    }
}
