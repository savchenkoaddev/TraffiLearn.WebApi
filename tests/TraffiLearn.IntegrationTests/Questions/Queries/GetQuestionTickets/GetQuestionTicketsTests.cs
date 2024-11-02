using TraffiLearn.Domain.Users.Roles;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Questions.Queries.GetQuestionTickets
{
    public sealed class GetQuestionTicketsTests : QuestionIntegrationTest
    {
        public GetQuestionTicketsTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task GetQuestionTickets_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await ApiQuestionClient.SendGetQuestionTicketsRequestAsync(
                questionId: Guid.NewGuid(),
                sentFromRole: null);

            response.AssertUnauthorizedStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetQuestionTickets_IfQuestionIsNotFound_ShouldReturn404StatusCode(
            Role eligibleRole)
        {
            var response = await ApiQuestionClient.SendGetQuestionTopicsRequestAsync(
                questionId: Guid.NewGuid(),
                sentFromRole: eligibleRole);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetQuestionTickets_IfValidCase_ShouldReturn200StatusCode(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var response = await ApiQuestionClient.SendGetQuestionTicketsRequestAsync(
                questionId: questionId,
                sentFromRole: eligibleRole);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetQuestionTickets_IfValidCase_ShouldReturnQuestionTopics(
            Role eligibleRole)
        {
            await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            List<Guid> ticketIds = [
                await ApiTicketClient.CreateValidTicketAsAuthorizedAsync(
                    questionIds: [questionId]),

                await ApiTicketClient.CreateValidTicketAsAuthorizedAsync(
                    questionIds: [questionId]),
            ];

            var questionTickets = await ApiQuestionClient.GetQuestionTicketsAsync(
                questionId: questionId,
                getWithRole: eligibleRole);

            var questionTicketIds = questionTickets.Select(x => x.TicketId);

            questionTicketIds.Should().HaveCount(ticketIds.Count);
            questionTicketIds.Should().BeEquivalentTo(ticketIds);
        }
    }
}
