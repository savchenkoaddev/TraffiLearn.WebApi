using TraffiLearn.Domain.Aggregates.Users.Roles;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Tickets.Queries.GetTicketQuestions
{
    public sealed class GetTicketQuestionsTests : TicketIntegrationTest
    {
        public GetTicketQuestionsTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task GetTicketQuestions_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await ApiTicketClient
                .SendGetTicketQuestionsRequestAsync(
                    ticketId: Guid.NewGuid(),
                    sentFromRole: null);

            response.AssertUnauthorizedStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetTicketQuestions_IfTicketIsNotFound_ShouldReturn404StatusCode(
            Role eligibleRole)
        {
            var response = await ApiTicketClient
                .SendGetTicketQuestionsRequestAsync(
                    ticketId: Guid.NewGuid(),
                    sentFromRole: eligibleRole);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetTicketQuestions_IfTicketDoesNotContainAnyQuestions_ShouldReturn200StatusCode(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = await ApiTicketClient.CreateValidTicketAsAuthorizedAsync(
                questionIds: [questionId]);

            await ApiTicketClient.RemoveQuestionFromTicketAsAuthorizedAsync(
                questionId: questionId,
                ticketId: ticketId);

            var response = await ApiTicketClient
                .SendGetTicketQuestionsRequestAsync(
                    ticketId: ticketId,
                    sentFromRole: eligibleRole);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetTicketQuestions_IfTicketDoesNotContainAnyQuestions_ShouldReturnEmptyCollection(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = await ApiTicketClient.CreateValidTicketAsAuthorizedAsync(
                questionIds: [questionId]);

            await ApiTicketClient.RemoveQuestionFromTicketAsAuthorizedAsync(
                questionId: questionId,
                ticketId: ticketId);

            var ticketQuestions = await ApiTicketClient
                .GetTicketQuestionsAsync(
                    ticketId: ticketId,
                    getWithRole: eligibleRole);

            ticketQuestions.Should().NotBeNull();
            ticketQuestions.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetTicketQuestions_IfValidCase_ShouldReturn200StatusCode(
            Role eligibleRole)
        {
            var ticketId = await ApiTicketClient
                .CreateValidTicketWithQuestionIdsAsAuthorizedAsync();

            var response = await ApiTicketClient
                .SendGetTicketQuestionsRequestAsync(
                    ticketId: ticketId,
                    sentFromRole: eligibleRole);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetTicketQuestions_IfValidCase_ShouldReturnValidTicketQuestions(
            Role eligibleRole)
        {
            List<Guid> questionIds = [
                await ApiQuestionClient
                    .CreateValidQuestionWithTopicAsAuthorizedAsync(),

                await ApiQuestionClient
                    .CreateValidQuestionWithTopicAsAuthorizedAsync(),
            ];

            var ticketId = await ApiTicketClient
                .CreateValidTicketAsAuthorizedAsync(
                    questionIds: questionIds);

            var ticketQuestions = await ApiTicketClient
                .GetTicketQuestionsAsync(
                    ticketId: ticketId,
                    getWithRole: eligibleRole);

            ticketQuestions.Should().HaveCount(2);

            foreach (var question in ticketQuestions)
            {
                var questionTickets = await ApiQuestionClient.GetQuestionTicketsAsAuthorizedUserAsync(
                    questionId: question.Id);

                questionTickets.Any(t => t.TicketId == ticketId).Should().BeTrue();
            }
        }
    }
}
