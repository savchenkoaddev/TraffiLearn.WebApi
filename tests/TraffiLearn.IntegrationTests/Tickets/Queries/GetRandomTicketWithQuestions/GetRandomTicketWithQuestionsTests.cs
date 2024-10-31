using TraffiLearn.Domain.Aggregates.Users.Roles;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Tickets.Queries.GetRandomTicketWithQuestions
{
    public sealed class GetRandomTicketWithQuestionsTests : TicketIntegrationTest
    {
        public GetRandomTicketWithQuestionsTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task GetRandomTicketWithQuestions_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await ApiTicketClient
                .SendGetRandomTicketWithQuestionsRequestAsync(
                    sentFromRole: null);

            response.AssertUnauthorizedStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetRandomTicketWithQuestions_IfNoTicketsExist_ShouldReturn500StatusCode(
            Role eligibleRole)
        {
            var response = await ApiTicketClient
                .SendGetRandomTicketWithQuestionsRequestAsync(
                    sentFromRole: eligibleRole);

            response.AssertInternalServerErrorStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetRandomTicketWithQuestions_IfTicketDoesNotContainAnyQuestions_ShouldReturn200StatusCode(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = await ApiTicketClient
                .CreateValidTicketAsAuthorizedAsync(
                    questionIds: [questionId]);

            await ApiTicketClient
                .RemoveQuestionFromTicketAsAuthorizedAsync(
                    questionId: questionId,
                    ticketId: ticketId);

            var response = await ApiTicketClient
                .SendGetRandomTicketWithQuestionsRequestAsync(
                    sentFromRole: eligibleRole);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetRandomTicketWithQuestions_IfTicketDoesNotContainAnyQuestions_ShouldReturnTicketWithEmptyQuestionsCollection(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = await ApiTicketClient
                .CreateValidTicketAsAuthorizedAsync(
                    questionIds: [questionId]);

            await ApiTicketClient
                .RemoveQuestionFromTicketAsAuthorizedAsync(
                    questionId: questionId,
                    ticketId: ticketId);

            var ticketWithQuestions = await ApiTicketClient
                .GetRandomTicketWithQuestionsAsync(
                    getWithRole: eligibleRole);

            ticketWithQuestions.Questions.Should().NotBeNull();
            ticketWithQuestions.Questions.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetRandomTicketWithQuestions_IfValidCase_ShouldReturn200StatusCode(
            Role eligibleRole)
        {
            var ticketId = await ApiTicketClient
                .CreateValidTicketWithQuestionIdsAsAuthorizedAsync();

            var response = await ApiTicketClient
                .SendGetRandomTicketWithQuestionsRequestAsync(
                    sentFromRole: eligibleRole);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetRandomTicketWithQuestions_IfValidCase_ShouldReturnValidTicketWithQuestions(
            Role eligibleRole)
        {
            List<Guid> questionIds = [
                await ApiQuestionClient
                    .CreateValidQuestionWithTopicAsAuthorizedAsync(),

                await ApiQuestionClient
                    .CreateValidQuestionWithTopicAsAuthorizedAsync()
            ];

            var ticketId = await ApiTicketClient
                .CreateValidTicketAsAuthorizedAsync(
                    questionIds: questionIds);

            var ticketWithQuestions = await ApiTicketClient
                .GetRandomTicketWithQuestionsAsync(
                    getWithRole: eligibleRole);

            ticketWithQuestions.TicketId.Should().Be(ticketId);
            ticketWithQuestions.Questions.Should().HaveCount(2);

            var ticketQuestionIds = ticketWithQuestions.Questions.Select(q => q.Id);

            ticketQuestionIds.Should().BeEquivalentTo(questionIds);
        }
    }
}
