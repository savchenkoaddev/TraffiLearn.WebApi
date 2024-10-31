using TraffiLearn.Domain.Aggregates.Users.Roles;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Tickets.Queries.GetAllTickets
{
    public sealed class GetAllTicketsTests : TicketIntegrationTest
    {
        public GetAllTicketsTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task GetAllTickets_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await ApiTicketClient
                .SendGetAllTicketsRequestAsync(
                    sentFromRole: null);

            response.AssertUnauthorizedStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetAllTickets_IfNoTicketsExist_ShouldReturn200StatusCode(
            Role eligibleRole)
        {
            var response = await ApiTicketClient
                .SendGetAllTicketsRequestAsync(
                    sentFromRole: eligibleRole);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetAllTickets_IfNoTicketsExist_ShouldReturnEmptyCollection(
            Role eligibleRole)
        {
            var allTickets = await ApiTicketClient
                .GetAllTicketsAsync(
                    getWithRole: eligibleRole);

            allTickets.Should().NotBeNull();
            allTickets.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetAllTickets_IfTicketsArePresent_ShouldReturn200StatusCode(
            Role eligibleRole)
        {
            await ApiTicketClient
                .CreateValidTicketWithQuestionIdsAsAuthorizedAsync();

            var response = await ApiTicketClient
                .SendGetAllTicketsRequestAsync(
                    sentFromRole: eligibleRole);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetAllTickets_IfTicketsArePresent_ShouldReturnExistingTickets(
            Role eligibleRole)
        {
            List<Guid> existingTicketIds = [
                await ApiTicketClient
                    .CreateValidTicketWithQuestionIdsAsAuthorizedAsync(),

                await ApiTicketClient
                    .CreateValidTicketWithQuestionIdsAsAuthorizedAsync(),
            ];

            var allTickets = await ApiTicketClient
                .GetAllTicketsAsync(
                    getWithRole: eligibleRole);

            var allTicketsIds = allTickets.Select(t => t.TicketId);

            allTicketsIds.Should().BeEquivalentTo(existingTicketIds);
        }
    }
}
