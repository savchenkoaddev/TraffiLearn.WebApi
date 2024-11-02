using TraffiLearn.Domain.Users.Roles;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Tickets.Commands.RemoveQuestionFromTicket
{
    public sealed class RemoveQuestionFromTicketTests : TicketIntegrationTest
    {
        public RemoveQuestionFromTicketTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task RemoveQuestionFromTicket_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await ApiTicketClient
                .SendRemoveQuestionFromTicketRequestAsync(
                    questionId: Guid.NewGuid(),
                    ticketId: Guid.NewGuid(),
                    sentFromRole: null);

            response.AssertUnauthorizedStatusCode();
        }

        [Fact]
        public async Task RemoveQuestionFromTicket_IfUserIsNotAuthenticated_QuestionShouldNotBeRemovedFromTicket()
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = await ApiTicketClient
                .CreateValidTicketAsAuthorizedAsync(
                    questionIds: [questionId]);

            await ApiTicketClient
                .SendRemoveQuestionFromTicketRequestAsync(
                    questionId: questionId,
                    ticketId: ticketId,
                    sentFromRole: null);

            var ticketQuestions = await ApiTicketClient
                .GetTicketQuestionsAsAuthorizedUserAsync(
                    ticketId: ticketId);

            ticketQuestions.Should().NotBeEmpty();
        }

        [Fact]
        public async Task RemoveQuestionFromTicket_IfUserIsNotAuthenticated_TicketShouldNotBeRemovedFromQuestion()
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = await ApiTicketClient
                .CreateValidTicketAsAuthorizedAsync(
                    questionIds: [questionId]);

            await ApiTicketClient
                .SendRemoveQuestionFromTicketRequestAsync(
                    questionId: questionId,
                    ticketId: ticketId,
                    sentFromRole: null);

            var questionTickets = await ApiQuestionClient
                .GetQuestionTicketsAsAuthorizedUserAsync(
                    questionId: questionId);

            questionTickets.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task RemoveQuestionFromTicket_IfUserIsNotEligible_ShouldReturn403StatusCode(
            Role nonEligibleRole)
        {
            var response = await ApiTicketClient
                .SendRemoveQuestionFromTicketRequestAsync(
                    questionId: Guid.NewGuid(),
                    ticketId: Guid.NewGuid(),
                    sentFromRole: nonEligibleRole);

            response.AssertForbiddenStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task RemoveQuestionFromTicket_IfUserIsNotEligible_QuestionShouldNotBeRemovedFromTicket(
            Role nonEligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = await ApiTicketClient
                .CreateValidTicketAsAuthorizedAsync(
                    questionIds: [questionId]);

            await ApiTicketClient
                .SendRemoveQuestionFromTicketRequestAsync(
                    questionId: questionId,
                    ticketId: ticketId,
                    sentFromRole: nonEligibleRole);

            var ticketQuestions = await ApiTicketClient
                .GetTicketQuestionsAsAuthorizedUserAsync(
                    ticketId: ticketId);

            ticketQuestions.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task RemoveQuestionFromTicket_IfUserIsNotEligible_TicketShouldNotBeRemovedFromQuestion(
            Role nonEligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = await ApiTicketClient
                .CreateValidTicketAsAuthorizedAsync(
                    questionIds: [questionId]);

            await ApiTicketClient
                .SendRemoveQuestionFromTicketRequestAsync(
                    questionId: questionId,
                    ticketId: ticketId,
                    sentFromRole: nonEligibleRole);

            var questionTickets = await ApiQuestionClient
                .GetQuestionTicketsAsAuthorizedUserAsync(
                    questionId: questionId);

            questionTickets.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task RemoveQuestionFromTicket_IfTicketIsNotFound_ShouldReturn404StatusCode(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var response = await ApiTicketClient
                .SendRemoveQuestionFromTicketRequestAsync(
                    questionId: questionId,
                    ticketId: Guid.NewGuid(),
                    sentFromRole: eligibleRole);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task RemoveQuestionFromTicket_IfQuestionIsNotFound_ShouldReturn404StatusCode(
            Role eligibleRole)
        {
            var ticketId = await ApiTicketClient
                .CreateValidTicketWithQuestionIdsAsAuthorizedAsync();

            var response = await ApiTicketClient
                .SendRemoveQuestionFromTicketRequestAsync(
                    questionId: Guid.NewGuid(),
                    ticketId: ticketId,
                    sentFromRole: eligibleRole);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task RemoveQuestionFromTicket_IfQuestionIsNotPresentInTicket_ShouldReturn404StatusCode(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = await ApiTicketClient
                .CreateValidTicketWithQuestionIdsAsAuthorizedAsync();

            var response = await ApiTicketClient
                .SendRemoveQuestionFromTicketRequestAsync(
                    questionId: questionId,
                    ticketId: ticketId,
                    sentFromRole: eligibleRole);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task RemoveQuestionFromTicket_IfValidCase_ShouldReturn204StatusCode(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = await ApiTicketClient
                .CreateValidTicketAsAuthorizedAsync(
                    questionIds: [questionId]);

            var response = await ApiTicketClient
                .SendRemoveQuestionFromTicketRequestAsync(
                    questionId: questionId,
                    ticketId: ticketId,
                    sentFromRole: eligibleRole);

            response.AssertNoContentStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task RemoveQuestionFromTicket_IfValidCase_QuestionShouldBeRemovedFromTicket(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = await ApiTicketClient
                .CreateValidTicketAsAuthorizedAsync(
                    questionIds: [questionId]);

            var response = await ApiTicketClient
                .SendRemoveQuestionFromTicketRequestAsync(
                    questionId: questionId,
                    ticketId: ticketId,
                    sentFromRole: eligibleRole);

            var ticketQuestions = await ApiTicketClient
                .GetTicketQuestionsAsAuthorizedUserAsync(
                    ticketId: ticketId);

            ticketQuestions.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task RemoveQuestionFromTicket_IfValidCase_TicketShouldBeRemovedFromQuestion(
           Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = await ApiTicketClient
                .CreateValidTicketAsAuthorizedAsync(
                    questionIds: [questionId]);

            var response = await ApiTicketClient
                .SendRemoveQuestionFromTicketRequestAsync(
                    questionId: questionId,
                    ticketId: ticketId,
                    sentFromRole: eligibleRole);

            var questionTickets = await ApiQuestionClient
                .GetQuestionTicketsAsAuthorizedUserAsync(
                    questionId: questionId);

            questionTickets.Should().BeEmpty();
        }
    }
}
