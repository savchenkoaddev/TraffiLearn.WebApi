using TraffiLearn.Application.UseCases.Tickets.Commands.Update;
using TraffiLearn.Domain.Users.Roles;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Tickets.Commands.UpdateTicket
{
    public sealed class UpdateTicketTests : TicketIntegrationTest
    {
        private readonly UpdateTicketCommandFactory _commandFactory;

        public UpdateTicketTests(
            WebApplicationFactory factory)
            : base(factory)
        {
            _commandFactory = new(ApiQuestionClient);
        }

        [Fact]
        public async Task UpdateTicket_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var ticketId = Guid.NewGuid();

            var response = await ApiTicketClient
                .SendValidUpdateTicketRequestWithRandomQuestionIdsAsync(
                    ticketId: ticketId,
                    updatedWithRole: null);

            response.AssertUnauthorizedStatusCode();
        }

        [Fact]
        public async Task UpdateTicket_IfUserIsNotAuthenticated_TicketShouldNotBeUpdated()
        {
            var ticketId = await ApiTicketClient
                .CreateValidTicketWithQuestionIdsAsAuthorizedAsync();

            var command = new UpdateTicketCommand(
                TicketId: ticketId,
                TicketNumber: 123,
                QuestionIds: [Guid.NewGuid()]);

            await ApiTicketClient
                .SendUpdateTicketRequestAsync(
                    request: command,
                    sentFromRole: null);

            var allTickets = await ApiTicketClient
                .GetAllTicketsAsAuthorizedUserAsync();

            var singleTopic = allTickets.Single();

            singleTopic.TicketNumber.Should().NotBe(command.TicketNumber);
        }

        [Fact]
        public async Task UpdateTicket_IfUserIsNotAuthenticated_TicketQuestionsShouldNotBeUpdated()
        {
            var ticketId = await ApiTicketClient
                .CreateValidTicketWithQuestionIdsAsAuthorizedAsync();

            var command = new UpdateTicketCommand(
                TicketId: ticketId,
                TicketNumber: 123,
                QuestionIds: [Guid.NewGuid()]);

            await ApiTicketClient
                .SendUpdateTicketRequestAsync(
                    request: command,
                    sentFromRole: null);

            var ticketQuestions = await ApiTicketClient
                .GetTicketQuestionsAsAuthorizedUserAsync(ticketId);

            ticketQuestions.Single().Id.Should().NotBe(command.QuestionIds!.Single());
        }

        [Fact]
        public async Task UpdateTicket_IfUserIsNotAuthenticated_QuestionTicketsShouldNotBeUpdated()
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = await ApiTicketClient
                .CreateValidTicketAsAuthorizedAsync(
                    questionIds: [questionId]);

            var command = new UpdateTicketCommand(
                TicketId: ticketId,
                TicketNumber: 123,
                QuestionIds: [Guid.NewGuid()]);

            await ApiTicketClient
                .SendUpdateTicketRequestAsync(
                    request: command,
                    sentFromRole: null);

            var questionTickets = await ApiQuestionClient
                .GetQuestionTicketsAsAuthorizedUserAsync(questionId);

            questionTickets.Single().TicketId.Should().Be(ticketId);
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task UpdateTicket_IfUserIsNotEligible_ShouldReturn403StatusCode(
            Role nonEligibleRole)
        {
            var ticketId = Guid.NewGuid();

            var response = await ApiTicketClient
                .SendValidUpdateTicketRequestWithRandomQuestionIdsAsync(
                    ticketId: ticketId,
                    updatedWithRole: nonEligibleRole);

            response.AssertForbiddenStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task UpdateTicket_IfUserIsNotEligible_TicketShouldNotBeUpdated(
            Role nonEligibleRole)
        {
            var ticketId = await ApiTicketClient
                .CreateValidTicketWithQuestionIdsAsAuthorizedAsync();

            var command = new UpdateTicketCommand(
                TicketId: ticketId,
                TicketNumber: 123,
                QuestionIds: [Guid.NewGuid()]);

            await ApiTicketClient
                .SendUpdateTicketRequestAsync(
                    request: command,
                    sentFromRole: nonEligibleRole);

            var allTickets = await ApiTicketClient
                .GetAllTicketsAsAuthorizedUserAsync();

            var singleTopic = allTickets.Single();

            singleTopic.TicketNumber.Should().NotBe(command.TicketNumber);
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task UpdateTicket_IfUserIsNotEligible_TicketQuestionsShouldNotBeUpdated(
            Role nonEligibleRole)
        {
            var ticketId = await ApiTicketClient
                .CreateValidTicketWithQuestionIdsAsAuthorizedAsync();

            var command = new UpdateTicketCommand(
                TicketId: ticketId,
                TicketNumber: 123,
                QuestionIds: [Guid.NewGuid()]);

            await ApiTicketClient
                .SendUpdateTicketRequestAsync(
                    request: command,
                    sentFromRole: nonEligibleRole);

            var ticketQuestions = await ApiTicketClient
                .GetTicketQuestionsAsAuthorizedUserAsync(ticketId);

            ticketQuestions.Single().Id.Should().NotBe(command.QuestionIds!.Single());
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task UpdateTicket_IfUserIsNotEligible_QuestionTicketsShouldNotBeUpdated(
            Role nonEligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = await ApiTicketClient
                .CreateValidTicketAsAuthorizedAsync(
                    questionIds: [questionId]);

            var command = new UpdateTicketCommand(
                TicketId: ticketId,
                TicketNumber: 123,
                QuestionIds: [Guid.NewGuid()]);

            await ApiTicketClient
                .SendUpdateTicketRequestAsync(
                    request: command,
                    sentFromRole: nonEligibleRole);

            var questionTickets = await ApiQuestionClient
                .GetQuestionTicketsAsAuthorizedUserAsync(questionId);

            questionTickets.Single().TicketId.Should().Be(ticketId);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateTicket_IfPassedInvalidArgs_ShouldReturn400StatusCode(
            Role eligibleRole)
        {
            var invalidCommands = _commandFactory
                .CreateInvalidCommandsWithRandomIds();

            await RequestSender.EnsureEachSentJsonRequestReturnsBadRequestAsync(
                method: HttpMethod.Put,
                requestUri: TicketEndpointRoutes.UpdateTicketRoute,
                requests: invalidCommands,
                sentFromRole: eligibleRole);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateTicket_IfTicketIsNotFound_ShouldReturn404StatusCode(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = Guid.NewGuid();

            var response = await ApiTicketClient.SendValidUpdateTicketRequestAsync(
                ticketId,
                questionIds: [questionId],
                sentFromRole: eligibleRole);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateTicket_IfTicketQuestionsAreNotFound_ShouldReturn404StatusCode(
            Role eligibleRole)
        {
            var ticketId = await ApiTicketClient
                .CreateValidTicketWithQuestionIdsAsAuthorizedAsync();

            var response = await ApiTicketClient.SendValidUpdateTicketRequestAsync(
                ticketId,
                questionIds: [Guid.NewGuid()],
                sentFromRole: eligibleRole);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateTicket_IfValidCase_ShouldReturn204StatusCode(
            Role eligibleRole)
        {
            var ticketId = await ApiTicketClient
                .CreateValidTicketWithQuestionIdsAsAuthorizedAsync();

            var response = await ApiTicketClient.SendValidUpdateTicketRequestWithRandomQuestionIdsAsync(
                ticketId,
                updatedWithRole: eligibleRole);

            response.AssertNoContentStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateTicket_IfValidCase_TicketShouldBeUpdated(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = await ApiTicketClient
                .CreateValidTicketWithQuestionIdsAsAuthorizedAsync();

            var command = new UpdateTicketCommand(
                TicketId: ticketId,
                TicketNumber: 11234,
                QuestionIds: [questionId]);

            var response = await ApiTicketClient.SendUpdateTicketRequestAsync(
                command,
                sentFromRole: eligibleRole);

            var allTickets = await ApiTicketClient.GetAllTicketsAsAuthorizedUserAsync();

            allTickets.Single().TicketNumber.Should().Be(command.TicketNumber);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateTicket_IfSingleQuestionReplaced_TicketQuestionsShouldContainReplacedQuestion(
            Role eligibleRole)
        {
            var newQuestionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = await ApiTicketClient
                .CreateValidTicketWithQuestionIdsAsAuthorizedAsync();

            var response = await ApiTicketClient.SendValidUpdateTicketRequestAsync(
                ticketId,
                questionIds: [newQuestionId],
                sentFromRole: eligibleRole);

            var ticketQuestions = await ApiTicketClient
                .GetTicketQuestionsAsAuthorizedUserAsync(ticketId);

            ticketQuestions.Should().HaveCount(1);
            ticketQuestions.Single().Id.Should().Be(newQuestionId);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateTicket_IfProvidedExistingQuestionIdPlusNewQuestionId_TicketQuestionsShouldContainBothQuestionIds(
            Role eligibleRole)
        {
            var oldQuestionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = await ApiTicketClient
                .CreateValidTicketAsAuthorizedAsync(
                    questionIds: [oldQuestionId]);

            var newQuestionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var response = await ApiTicketClient.SendValidUpdateTicketRequestAsync(
                ticketId,
                questionIds: [oldQuestionId, newQuestionId],
                sentFromRole: eligibleRole);

            var ticketQuestions = await ApiTicketClient
                .GetTicketQuestionsAsAuthorizedUserAsync(ticketId);

            ticketQuestions.Should().HaveCount(2);
            ticketQuestions.Any(q => q.Id == oldQuestionId).Should().BeTrue();
            ticketQuestions.Any(q => q.Id == newQuestionId).Should().BeTrue();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateTicket_IfAnOldQuestionIdExistsAndProvidedFewNewQuestionIds_TicketQuestionsShouldContainOnlyNewQuestionIds(
            Role eligibleRole)
        {
            var ticketId = await ApiTicketClient
                .CreateValidTicketWithQuestionIdsAsAuthorizedAsync();

            List<Guid> newQuestionIds = [
                await ApiQuestionClient
                    .CreateValidQuestionWithTopicAsAuthorizedAsync(),

                await ApiQuestionClient
                    .CreateValidQuestionWithTopicAsAuthorizedAsync(),
            ];

            var response = await ApiTicketClient.SendValidUpdateTicketRequestAsync(
                ticketId,
                questionIds: newQuestionIds,
                sentFromRole: eligibleRole);

            var ticketQuestions = await ApiTicketClient
                .GetTicketQuestionsAsAuthorizedUserAsync(ticketId);

            ticketQuestions.Should().HaveCount(2);
            ticketQuestions.Any(q => q.Id == newQuestionIds.First()).Should().BeTrue();
            ticketQuestions.Any(q => q.Id == newQuestionIds.Last()).Should().BeTrue();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateTicket_IfFewOldQuestionsExistAndProvidedTheOldOnes_TicketQuestionsShouldNotChange(
            Role eligibleRole)
        {
            List<Guid> oldQuestionIds = [
                await ApiQuestionClient
                    .CreateValidQuestionWithTopicAsAuthorizedAsync(),

                await ApiQuestionClient
                    .CreateValidQuestionWithTopicAsAuthorizedAsync(),
            ];

            var ticketId = await ApiTicketClient
                .CreateValidTicketAsAuthorizedAsync(
                    questionIds: oldQuestionIds);

            var response = await ApiTicketClient.SendValidUpdateTicketRequestAsync(
                ticketId,
                questionIds: oldQuestionIds,
                sentFromRole: eligibleRole);

            var ticketQuestions = await ApiTicketClient
                .GetTicketQuestionsAsAuthorizedUserAsync(ticketId);

            ticketQuestions.Should().HaveCount(2);
            ticketQuestions.Any(q => q.Id == oldQuestionIds.First()).Should().BeTrue();
            ticketQuestions.Any(q => q.Id == oldQuestionIds.Last()).Should().BeTrue();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task UpdateTicket_IfUpdatedTicketQuestions_QuestionTicketsShouldBeUpdatedAsWell(
            Role eligibleRole)
        {
            List<Guid> oldQuestionIds = [
                await ApiQuestionClient
                    .CreateValidQuestionWithTopicAsAuthorizedAsync(),

                await ApiQuestionClient
                    .CreateValidQuestionWithTopicAsAuthorizedAsync(),
            ];

            var ticketId = await ApiTicketClient
                .CreateValidTicketAsAuthorizedAsync(
                    questionIds: oldQuestionIds);

            var newQuestionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var response = await ApiTicketClient.SendValidUpdateTicketRequestAsync(
                ticketId,
                questionIds: [newQuestionId],
                sentFromRole: eligibleRole);

            foreach (var oldQuestionId in oldQuestionIds)
            {
                var questionTickets = await ApiQuestionClient
                    .GetQuestionTicketsAsAuthorizedUserAsync(oldQuestionId);

                questionTickets.Any(t => t.TicketId == ticketId).Should().BeFalse();
            }

            var newQuestionTickets = await ApiQuestionClient
                .GetQuestionTicketsAsAuthorizedUserAsync(newQuestionId);

            newQuestionTickets.Single().TicketId.Should().Be(ticketId);
        }
    }
}
