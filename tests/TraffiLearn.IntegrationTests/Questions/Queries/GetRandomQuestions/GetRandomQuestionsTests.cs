using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Questions.Queries.GetRandomQuestions
{
    public sealed class GetRandomQuestionsTests : QuestionIntegrationTest
    {
        private readonly GetRandomQuestionsQueryFactory _queryFactory;

        public GetRandomQuestionsTests(
            WebApplicationFactory factory)
            : base(factory)
        {
            _queryFactory = new GetRandomQuestionsQueryFactory();
        }

        [Fact]
        public async Task GetRandomQuestions_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await ApiQuestionClient.SendGetRandomQuestionsRequestAsync(
                amount: 1,
                sentFromRole: null);

            response.AssertUnauthorizedStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetRandomQuestions_IfPassedInvalidArgs_ShouldReturn400StatusCode(
            Role eligibleRole)
        {
            var invalidQueries = _queryFactory.CreateInvalidQueries();

            foreach (var query in invalidQueries)
            {
                var response = await ApiQuestionClient.SendGetRandomQuestionsRequestAsync(
                    amount: query.Amount,
                    sentFromRole: eligibleRole);

                response.AssertBadRequestStatusCode();
            }
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetRandomQuestions_IfNoQueryParamsPassed_ShouldReturn200StatusCode(
            Role eligibleRole)
        {
            var response = await RequestSender.GetAsync(
                requestUri: QuestionEndpointRoutes.GetRandomQuestionsRoute(),
                getWithRole: eligibleRole);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetRandomQuestions_IfNoQueryParamsPassedAndNoQuestionsExist_ShouldReturnEmptyCollection(
            Role eligibleRole)
        {
            var response = await RequestSender.GetFromJsonAsync<IEnumerable<QuestionResponse>>(
                requestUri: QuestionEndpointRoutes.GetRandomQuestionsRoute(),
                getWithRole: eligibleRole);

            response.Should().NotBeNull();
            response.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetRandomQuestions_IfNoQueryParamsPassed_ShouldReturnOneRandomQuestion(
            Role eligibleRole)
        {
            List<Guid> questionIds = [
                await ApiQuestionClient.CreateValidQuestionWithTopicAsAuthorizedAsync(),
                await ApiQuestionClient.CreateValidQuestionWithTopicAsAuthorizedAsync()
            ];

            var response = await RequestSender.GetFromJsonAsync<IEnumerable<QuestionResponse>>(
                requestUri: QuestionEndpointRoutes.GetRandomQuestionsRoute(),
                getWithRole: eligibleRole);

            response.Should().NotBeNullOrEmpty();
            response.Should().HaveCount(1);
            questionIds.Should().Contain(response.Single().Id);
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetRandomQuestions_IfNoQuestionsExist_ShouldReturn200StatusCode(
            Role eligibleRole)
        {
            var response = await ApiQuestionClient.SendGetRandomQuestionsRequestAsync(
                amount: 1,
                sentFromRole: eligibleRole);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetRandomQuestions_IfNoQuestionsExist_ShouldReturnEmptyCollection(
            Role eligibleRole)
        {
            var response = await ApiQuestionClient.GetRandomQuestionsAsync(
                amount: 1,
                getWithRole: eligibleRole);

            response.Should().NotBeNull();
            response.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetRandomQuestions_IfValidCase_ShouldReturn200StatusCode(
            Role eligibleRole)
        {
            await ApiQuestionClient.CreateValidQuestionWithTopicAsAuthorizedAsync();

            var response = await ApiQuestionClient.SendGetRandomQuestionsRequestAsync(
                amount: 1,
                sentFromRole: eligibleRole);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetRandomQuestions_IfValidCase_ShouldReturnRandomQuestions(
            Role eligibleRole)
        {
            List<Guid> questionIds = [
                await ApiQuestionClient.CreateValidQuestionWithTopicAsAuthorizedAsync(),
                await ApiQuestionClient.CreateValidQuestionWithTopicAsAuthorizedAsync(),
                await ApiQuestionClient.CreateValidQuestionWithTopicAsAuthorizedAsync()
            ];

            var oneRandomQuestionResponse = await ApiQuestionClient.GetRandomQuestionsAsync(
                amount: 1,
                getWithRole: eligibleRole);

            oneRandomQuestionResponse.Should().NotBeNullOrEmpty();
            oneRandomQuestionResponse.Should().HaveCount(1);
            questionIds.Should().Contain(oneRandomQuestionResponse.Single().Id);

            var twoRandomQuestionResponse = await ApiQuestionClient.GetRandomQuestionsAsync(
                amount: 2,
                getWithRole: eligibleRole);

            twoRandomQuestionResponse.Should().NotBeNullOrEmpty();
            twoRandomQuestionResponse.Should().HaveCount(2);
            twoRandomQuestionResponse.Select(x => x.Id)
                .Should().BeSubsetOf(questionIds);
        }
    }
}
