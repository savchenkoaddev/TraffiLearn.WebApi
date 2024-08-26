using FluentAssertions;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Tickets.DeleteTicket
{
    public sealed class DeleteTicketTests : TicketIntegrationTest
    {
        public DeleteTicketTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task DeleteTicket_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var ticketId = Guid.NewGuid();

            var response = await ApiTicketClient
                .SendDeleteTicketRequestAsync(
                    ticketId: ticketId,
                    sentFromRole: null);

            response.AssertUnauthorizedStatusCode();
        }

        [Fact]
        public async Task DeleteTicket_IfUserIsNotAuthenticated_TicketShouldNotBeDeleted()
        {
            var ticketId = await ApiTicketClient
                .CreateValidTicketWithQuestionIdsAsAuthorizedAsync();

            await ApiTicketClient
                .SendDeleteTicketRequestAsync(
                    ticketId: ticketId,
                    sentFromRole: null);

            await AssertAllTicketsCollectionIsNotEmpty();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task DeleteTicket_IfUserIsNotEligible_ShouldReturn401StatusCode(
            Role nonEligibleRole)
        {
            var ticketId = Guid.NewGuid();

            var response = await ApiTicketClient.SendDeleteTicketRequestAsync(
                ticketId: ticketId,
                sentFromRole: nonEligibleRole);

            response.AssertForbiddenStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task DeleteTicket_IfUserIsNotEligible_TicketShouldNotBeDeleted(
            Role nonEligibleRole)
        {
            var ticketId = await ApiTicketClient.
                CreateValidTicketWithQuestionIdsAsAuthorizedAsync();

            var response = await ApiTicketClient.SendDeleteTicketRequestAsync(
                ticketId: ticketId,
                sentFromRole: nonEligibleRole);

            await AssertAllTicketsCollectionIsNotEmpty();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task DeleteTicket_IfTicketIsNotFound_ShouldReturn404StatusCode(
            Role eligibleRole)
        {
            var ticketId = Guid.NewGuid();

            var response = await ApiTicketClient.SendDeleteTicketRequestAsync(
                ticketId,
                sentFromRole: eligibleRole);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task DeleteTicket_IfValidCase_ShouldReturn204StatusCode(
            Role eligibleRole)
        {
            var ticketId = await ApiTicketClient
                .CreateValidTicketWithQuestionIdsAsAuthorizedAsync();

            var response = await ApiTicketClient.SendDeleteTicketRequestAsync(
                ticketId,
                sentFromRole: eligibleRole);

            response.AssertNoContentStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task DeleteTicket_IfValidCase_TicketShouldBeDeleted(
            Role eligibleRole)
        {
            var ticketId = await ApiTicketClient
                 .CreateValidTicketWithQuestionIdsAsAuthorizedAsync();

            await ApiTicketClient.DeleteTicketAsync(
                ticketId,
                deletedWithRole: eligibleRole);

            var allTopics = await ApiTicketClient
                .GetAllTicketsAsAuthorizedUserAsync();

            allTopics.Should().BeEmpty();
        }

        private async Task AssertAllTicketsCollectionIsNotEmpty()
        {
            var allTickets = await ApiTicketClient
                .GetAllTicketsAsAuthorizedUserAsync();

            allTickets.Should().NotBeEmpty();
        }
    }
}
