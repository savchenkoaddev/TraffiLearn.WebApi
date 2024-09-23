using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Questions.Queries.GetPaginatedQuestions
{
    public sealed class GetPaginatedQuestionsTests : QuestionIntegrationTest
    {
        public GetPaginatedQuestionsTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task GetPaginatedQuestions_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await ApiQuestionClient
                .SendGetPaginatedQuestionsRequestAsync(
                    sentFromRole: null);

            response.AssertUnauthorizedStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetPaginatedQuestions_IfNoQuestionsExist_ShouldReturn200StatusCode(
            Role eligibleRole)
        {
            var response = await ApiQuestionClient
                .SendGetPaginatedQuestionsRequestAsync(
                    page: 1,
                    pageSize: 10,
                    sentFromRole: eligibleRole);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetPaginatedQuestions_IfNoQuestionsExist_ShouldReturnEmptyCollection(
            Role eligibleRole)
        {
            var allQuestions = await ApiQuestionClient
                .GetPaginatedQuestionsAsync(
                    page: 1,
                    pageSize: 10,
                    getWithRole: eligibleRole);

            allQuestions.Should().NotBeNull();
            allQuestions.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetPaginatedQuestions_IfNoParamsProvided_ShouldReturn200StatusCode(
            Role eligibleRole)
        {
            var response = await ApiQuestionClient
                .SendGetPaginatedQuestionsRequestAsync(
                    sentFromRole: eligibleRole);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetPaginatedQuestions_IfPassedInvalidArgs_ShouldReturn400StatusCode(
            Role eligibleRole)
        {
            List<HttpResponseMessage> responses = [
                await ApiQuestionClient
                    .SendGetPaginatedQuestionsRequestAsync(
                        page: -1,
                        pageSize: 10,
                        sentFromRole: eligibleRole),

                await ApiQuestionClient
                    .SendGetPaginatedQuestionsRequestAsync(
                        page: 0,
                        pageSize: 10,
                        sentFromRole: eligibleRole),

                await ApiQuestionClient
                    .SendGetPaginatedQuestionsRequestAsync(
                        page: 1,
                        pageSize: -1,
                        sentFromRole: eligibleRole),

                await ApiQuestionClient
                    .SendGetPaginatedQuestionsRequestAsync(
                        page: 1,
                        pageSize: 0,
                        sentFromRole: eligibleRole),
            ];

            responses.ForEach(response => response.AssertBadRequestStatusCode());
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetPaginatedQuestions_IfPageExceedsAvailableData_ShouldReturn200StatusCode(
            Role eligibleRole)
        {
            await ApiQuestionClient.CreateValidQuestionWithTopicAsAuthorizedAsync();

            var response = await ApiQuestionClient
                .SendGetPaginatedQuestionsRequestAsync(
                    page: 2,
                    pageSize: 1,
                    sentFromRole: eligibleRole);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetPaginatedQuestions_IfPageExceedsAvailableData_ShouldReturnEmptyCollection(
            Role eligibleRole)
        {
            await ApiQuestionClient.CreateValidQuestionWithTopicAsAuthorizedAsync();

            var questions = await ApiQuestionClient
                .GetPaginatedQuestionsAsync(
                    page: 2,
                    pageSize: 1,
                    getWithRole: eligibleRole);

            questions.Should().NotBeNull();
            questions.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetPaginatedQuestions_IfPageSizeExceedsTotalQuestions_ShouldReturn200StatusCode(
            Role eligibleRole)
        {
            await ApiQuestionClient.CreateValidQuestionWithTopicAsAuthorizedAsync();

            var response = await ApiQuestionClient
                .SendGetPaginatedQuestionsRequestAsync(
                    page: 1,
                    pageSize: 5,
                    sentFromRole: eligibleRole);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetPaginatedQuestions_IfPageSizeExceedsTotalQuestions_ShouldReturnAvailableQuestions(
            Role eligibleRole)
        {
            List<Guid> questionIds = [
                await ApiQuestionClient.CreateValidQuestionWithTopicAsAuthorizedAsync(),
                await ApiQuestionClient.CreateValidQuestionWithTopicAsAuthorizedAsync()
            ];

            var questions = await ApiQuestionClient
                .GetPaginatedQuestionsAsync(
                    page: 1,
                    pageSize: 5,
                    getWithRole: eligibleRole);

            questions.Should().HaveCount(questionIds.Count);
            questions.Select(q => q.Id).Should().BeEquivalentTo(questionIds);
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetPaginatedQuestions_IfValidPagesRequested_ShouldReturnCorrectDataForEachPage(
            Role eligibleRole)
        {
            List<Guid> questionIds = [
                await ApiQuestionClient.CreateValidQuestionWithTopicAsAuthorizedAsync(),
                await ApiQuestionClient.CreateValidQuestionWithTopicAsAuthorizedAsync(),
                await ApiQuestionClient.CreateValidQuestionWithTopicAsAuthorizedAsync()
            ];

            var pageSize = 2;

            var firstPageQuestions = await ApiQuestionClient
                .GetPaginatedQuestionsAsync(
                    page: 1,
                    pageSize: pageSize,
                    getWithRole: eligibleRole);

            firstPageQuestions.Should().HaveCount(pageSize);

            var secondPageQuestions = await ApiQuestionClient
                .GetPaginatedQuestionsAsync(
                    page: 2,
                    pageSize: pageSize,
                    getWithRole: eligibleRole);

            secondPageQuestions.Should().HaveCount(1);

            firstPageQuestions.Select(q => q.Id).Should()
                .NotContain(secondPageQuestions.Single().Id);
        }
    }
}
