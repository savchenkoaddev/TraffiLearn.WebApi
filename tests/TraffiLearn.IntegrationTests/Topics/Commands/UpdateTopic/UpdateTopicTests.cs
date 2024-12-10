using TraffiLearn.Application.UseCases.Topics.Commands.Update;
using TraffiLearn.Domain.Users.Roles;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;
using TraffiLearn.IntegrationTests.Helpers;

namespace TraffiLearn.IntegrationTests.Topics.Commands.UpdateTopic
{
    public sealed class UpdateTopicTests : TopicIntegrationTest
    {
        private readonly UpdateTopicCommandFactory _commandFactory = new();

        public UpdateTopicTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task UpdateTopic_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await ApiTopicClient.SendValidUpdateTopicRequestAsync(
                topicId: Guid.NewGuid(),
                updatedWithRole: null);

            response.AssertUnauthorizedStatusCode();
        }

        [Fact]
        public async Task UpdateTopic_IfUserIsNotAuthenticated_TopicShouldNotBeUpdated()
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var command = new UpdateTopicCommand(
                topicId,
                TopicNumber: 233,
                Title: "Updated-title");

            await ApiTopicClient.SendUpdateTopicRequestAsync(
                command,
                updatedWithRole: null);

            var allTopics = await ApiTopicClient.GetAllTopicsSortedByNumberAsAuthorizedAsync();

            var singleTopic = allTopics.Single();

            singleTopic.Id.Should().Be(topicId);
            singleTopic.TopicNumber.Should().NotBe(command.TopicNumber);
            singleTopic.Title.Should().NotBe(command.Title);
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task UpdateTopic_IfUserIsNotEligible_ShouldReturn403StatusCode(
            Role nonEligibleRole)
        {
            var response = await ApiTopicClient.SendValidUpdateTopicRequestAsync(
                topicId: Guid.NewGuid(),
                updatedWithRole: nonEligibleRole);

            response.AssertForbiddenStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task UpdateTopic_IfUserIsNotEligible_TopicShouldNotBeUpdated(
            Role nonEligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var command = new UpdateTopicCommand(
                topicId,
                TopicNumber: 233,
                Title: "Updated-title");

            await ApiTopicClient.SendUpdateTopicRequestAsync(
                command,
                updatedWithRole: nonEligibleRole);

            var allTopics = await ApiTopicClient.GetAllTopicsSortedByNumberAsAuthorizedAsync();

            var singleTopic = allTopics.Single();

            singleTopic.Id.Should().Be(topicId);
            singleTopic.TopicNumber.Should().NotBe(command.TopicNumber);
            singleTopic.Title.Should().NotBe(command.Title);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateTopic_IfPassedInvalidArgs_ShouldReturn400StatusCode(
            Role eligibleRole)
        {
            var invalidCommands = _commandFactory.CreateInvalidCommandsWithRandomIds();

            foreach (var command in invalidCommands)
            {
                var response = await RequestSender.SendMultipartFormDataWithJsonAndFileRequest(
                    method: HttpMethod.Put,
                    requestUri: TopicEndpointRoutes.UpdateTopicRoute,
                    value: command,
                    sentFromRole: eligibleRole);

                response.AssertBadRequestStatusCode();
            }
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateTopic_IfTopicDoesNotExist_ShouldReturn404StatusCode(
            Role eligibleRole)
        {
            var response = await ApiTopicClient.SendValidUpdateTopicRequestAsync(
                topicId: Guid.NewGuid(),
                updatedWithRole: eligibleRole);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateTopic_IfValidCase_ShouldReturn204StatusCode(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var command = new UpdateTopicCommand(
                TopicId: topicId,
                TopicNumber: 552,
                Title: "Updated-title");

            var response = await ApiTopicClient.SendUpdateTopicRequestAsync(
                command,
                updatedWithRole: eligibleRole);

            response.AssertNoContentStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateTopic_IfValidCase_TopicShouldBeUpdated(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var command = new UpdateTopicCommand(
                TopicId: topicId,
                TopicNumber: 552,
                Title: "Updated-title");

            var response = await ApiTopicClient.SendUpdateTopicRequestAsync(
                command,
                updatedWithRole: eligibleRole);

            var allTopics = await ApiTopicClient
                .GetAllTopicsSortedByNumberAsAuthorizedAsync();

            allTopics.Should().HaveCount(1);

            var singleTopic = allTopics.Single();

            singleTopic.Id.Should().Be(topicId);

            singleTopic.TopicNumber.Should().Be(command.TopicNumber);
            singleTopic.Title.Should().Be(command.Title);
        }
    }
}
