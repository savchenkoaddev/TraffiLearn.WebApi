using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TraffiLearn.Application.Questions.Options;
using TraffiLearn.Domain.Aggregates.Users.Roles;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Questions.Queries.GetQuestionsForTheoryTest
{
    public sealed class GetQuestionsForTheoryTestTests : QuestionIntegrationTest
    {
        private readonly int _neededQuestionsCount;

        public GetQuestionsForTheoryTestTests(
            WebApplicationFactory factory)
            : base(factory)
        {
            _neededQuestionsCount = factory.Services
                .GetRequiredService<IOptions<QuestionsSettings>>()
                .Value
                .TheoryTestQuestionsCount;
        }

        [Fact]
        public async Task GetQuestionsForTheoryTest_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await ApiQuestionClient.SendGetQuestionsForTheoryTestRequestAsync(
                sentFromRole: null);

            response.AssertUnauthorizedStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetQuestionsForTheoryTest_IfNotEnoughRecords_ShouldReturn500StatusCode(
            Role eligibleRole)
        {
            var response = await ApiQuestionClient.SendGetQuestionsForTheoryTestRequestAsync(
                sentFromRole: eligibleRole);

            response.AssertInternalServerErrorStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetQuestionsForTheoryTest_IfValidCase_ShouldReturn200StatusCode(
            Role eligibleRole)
        {
            await CreateRandomQuestions(_neededQuestionsCount + 1);

            var response = await ApiQuestionClient.SendGetQuestionsForTheoryTestRequestAsync(
                sentFromRole: eligibleRole);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetQuestionsForTheoryTest_IfValidCase_ShouldReturnValidAmountOfQuestions(
            Role eligibleRole)
        {
            await CreateRandomQuestions(_neededQuestionsCount + 10);

            var questions = await ApiQuestionClient.GetQuestionsForTheoryTestAsync(
                getWithRole: eligibleRole);

            questions.Should().HaveCount(_neededQuestionsCount);
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetQuestionsForTheoryTest_IfValidCase_ShouldReturnValidQuestions(
            Role eligibleRole)
        {
            var questionIds = await CreateRandomQuestions(_neededQuestionsCount + 10);

            var questions = await ApiQuestionClient.GetQuestionsForTheoryTestAsync(
                getWithRole: eligibleRole);

            questions.Select(q => q.Id)
                .Should().BeSubsetOf(questionIds);
        }

        private async Task<List<Guid>> CreateRandomQuestions(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException(nameof(amount));
            }

            List<Guid> questionIds = [];

            for (int i = 0; i < amount; i++)
            {
                questionIds.Add(await ApiQuestionClient.CreateValidQuestionWithTopicAsAuthorizedAsync());
            }

            return questionIds;
        }
    }
}
