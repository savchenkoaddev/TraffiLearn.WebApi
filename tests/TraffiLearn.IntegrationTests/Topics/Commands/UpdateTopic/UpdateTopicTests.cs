﻿using FluentAssertions;
using TraffiLearn.Application.Topics.Commands.Update;
using TraffiLearn.Domain.Aggregates.Users.Enums;
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
            var topicId = await ApiTopicClient.CreateTopicAsAuthorizedAsync();

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
            var topicId = await ApiTopicClient.CreateTopicAsAuthorizedAsync();

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

            await RequestSender.EnsureEachSentRequestReturnsBadRequestAsync(
                method: HttpMethod.Put,
                requestUri: TopicEndpointRoutes.UpdateTopicRoute,
                requests: invalidCommands,
                sentFromRole: eligibleRole);
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
            var topicId = await ApiTopicClient.CreateTopicAsAuthorizedAsync();

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
            var topicId = await ApiTopicClient.CreateTopicAsAuthorizedAsync();

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