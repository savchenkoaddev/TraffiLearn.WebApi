﻿using FluentAssertions;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;
using TraffiLearn.IntegrationTests.Questions;

namespace TraffiLearn.IntegrationTests.Topics.Queries.GetRandomTopicWithQuestions
{
    public sealed class GetRandomTopicWithQuestionsTests : TopicIntegrationTest
    {
        public GetRandomTopicWithQuestionsTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task GetRandomTopicWithQuestions_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await ApiTopicClient.SendGetRandomTopicWithQuestionsAsync(
                sentWithRole: null);

            response.AssertUnauthorizedStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetRandomTopicWithQuestions_IfNoTopicsExist_ShouldReturn500StatusCode(
            Role eligibleRole)
        {
            var response = await ApiTopicClient.SendGetRandomTopicWithQuestionsAsync(
                sentWithRole: eligibleRole);

            response.AssertInternalServerErrorStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetRandomTopicWithQuestions_IfTopicDoesNotContainQuestions_ShouldReturn200StatusCode(
            Role eligibleRole)
        {
            await ApiTopicClient.CreateTopicAsAuthorizedAsync();

            var response = await ApiTopicClient.SendGetRandomTopicWithQuestionsAsync(
                sentWithRole: eligibleRole);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetRandomTopicWithQuestions_IfTopicDoesNotContainQuestions_ShouldReturnTopicWithEmptyQuestions(
            Role eligibleRole)
        {
            await ApiTopicClient.CreateTopicAsAuthorizedAsync();

            var topicWithQuestions = await ApiTopicClient.GetRandomTopicWithQuestionsAsync(
                sentWithRole: eligibleRole);

            topicWithQuestions.Questions.Should().NotBeNull();
            topicWithQuestions.Questions.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetRandomTopicWithQuestions_IfTopicDoesNotContainQuestions_TopicShouldBeCorrect(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateTopicAsAuthorizedAsync();

            var topicWithQuestions = await ApiTopicClient.GetRandomTopicWithQuestionsAsync(
                sentWithRole: eligibleRole);

            topicWithQuestions.TopicId.Should().Be(topicId);
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetRandomTopicWithQuestions_IfValidCase_ShouldReturn200StatusCode(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var response = await ApiTopicClient.SendGetRandomTopicWithQuestionsAsync(
                sentWithRole: eligibleRole);

            response.AssertOkStatusCode();
        }

        /* The endpoint is not being tested completelly,
          due to the inability to check for randomness in the correct way */
        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetRandomTopicWithQuestions_IfOnlyOneTopicExists_ShouldReturnRandomTopicWithQuestions(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var topicWithQuestions = await ApiTopicClient.GetRandomTopicWithQuestionsAsync(
                sentWithRole: eligibleRole);

            topicWithQuestions.Questions.Should().HaveCount(1);
            topicWithQuestions.Questions.First().Id.Should().Be(questionId);
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetRandomTopicWithQuestions_IfOnlyOneTopicExists_TopicShouldBeCorrect(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateTopicAsAuthorizedAsync();

            var questionId = await ApiQuestionClient.CreateValidQuestionAsAuthorizedAsync(
                topicIds: [topicId]);

            var topicWithQuestions = await ApiTopicClient.GetRandomTopicWithQuestionsAsync(
                sentWithRole: eligibleRole);

            topicWithQuestions.TopicId.Should().Be(topicId);
        }
    }
}