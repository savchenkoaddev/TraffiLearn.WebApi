using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Tickets.Queries.GetTicketById
{
    public sealed class GetTicketByIdTests : TicketIntegrationTest
    {
        public GetTicketByIdTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task GetTicketById_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await ApiTicketClient
                .SendGetTicketByIdRequestAsync(
                    ticketId: Guid.NewGuid(),
                    sentFromRole: null);

            response.AssertUnauthorizedStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetTicketById_IfTicketIsNotFound_ShouldReturn404StatusCode(
            Role eligibleRole)
        {
            var response = await ApiTicketClient
                .SendGetTicketByIdRequestAsync(
                    ticketId: Guid.NewGuid(),
                    sentFromRole: eligibleRole);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetTicketById_IfValidCase_ShouldReturn200StatusCode(
            Role eligibleRole)
        {
            var ticketId = await ApiTicketClient
                .CreateValidTicketWithQuestionIdsAsAuthorizedAsync();

            var response = await ApiTicketClient
                .SendGetTicketByIdRequestAsync(
                    ticketId: ticketId,
                    sentFromRole: eligibleRole);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetTicketById_IfValidCase_ShouldReturnIntendedTicket(
            Role eligibleRole)
        {
            var ticketId = await ApiTicketClient
                .CreateValidTicketWithQuestionIdsAsAuthorizedAsync();

            var foundTicket = await ApiTicketClient
                .GetTicketByIdAsync(
                    ticketId: ticketId,
                    getWithRole: eligibleRole);

            foundTicket.TicketId.Should().Be(ticketId);
        }
    }
}
