using FluentAssertions;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Topics.Commands.DeleteTopic
{
    public sealed class DeleteTopicTests : TopicIntegrationTest
    {
        public DeleteTopicTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task DeleteTopic_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var topicId = Guid.NewGuid();

            var response = await ApiTopicClient.SendDeleteTopicRequestAsync(
                topicId,
                sentFromRole: null);

            response.AssertUnauthorizedStatusCode();
        }

        [Fact]
        public async Task DeleteTopic_IfUserNotAuthenticated_TopicShouldNotBeDeleted()
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            await ApiTopicClient.SendDeleteTopicRequestAsync(
                topicId,
                sentFromRole: null);

            var allTopics = await ApiTopicClient.GetAllTopicsSortedByNumberAsAuthorizedAsync();

            allTopics.Should().HaveCount(1);
            allTopics.First().Id.Should().Be(topicId);
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task DeleteTopic_IfUserIsNotEligible_ShouldReturn401StatusCode(
            Role nonEligibleRole)
        {
            var topicId = Guid.NewGuid();

            var response = await ApiTopicClient.SendDeleteTopicRequestAsync(
                topicId,
                sentFromRole: nonEligibleRole);

            response.AssertForbiddenStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task DeleteTopic_IfUserIsNotEligible_TopicShouldNotBeDeleted(
            Role nonEligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var response = await ApiTopicClient.SendDeleteTopicRequestAsync(
                topicId,
                sentFromRole: nonEligibleRole);

            var allTopics = await ApiTopicClient.GetAllTopicsSortedByNumberAsAuthorizedAsync();

            allTopics.Should().HaveCount(1);
            allTopics.First().Id.Should().Be(topicId);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task DeleteTopic_IfTopicIsNotFound_ShouldReturn404StatusCode(
            Role eligibleRole)
        {
            var topicId = Guid.NewGuid();

            var response = await ApiTopicClient.SendDeleteTopicRequestAsync(
                topicId,
                sentFromRole: eligibleRole);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task DeleteTopic_IfValidCase_ShouldReturn204StatusCode(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var response = await ApiTopicClient.SendDeleteTopicRequestAsync(
                topicId,
                sentFromRole: eligibleRole);

            response.AssertNoContentStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task DeleteTopic_IfValidCase_TopicShouldBeDeleted(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            await ApiTopicClient.SendDeleteTopicRequestAsync(
                topicId,
                sentFromRole: eligibleRole);

            var allTopics = await ApiTopicClient.GetAllTopicsSortedByNumberAsAuthorizedAsync();

            allTopics.Should().BeEmpty();
        }
    }
}
