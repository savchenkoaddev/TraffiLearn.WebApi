using TraffiLearn.Domain.Users.Roles;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Tickets.Commands.AddQuestionToTicket
{
    public sealed class AddQuestionToTicketTests : TicketIntegrationTest
    {
        public AddQuestionToTicketTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task AddQuestionToTicket_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await ApiTicketClient
                .SendAddQuestionToTicketRequestAsync(
                    questionId: Guid.NewGuid(),
                    ticketId: Guid.NewGuid(),
                    sentFromRole: null);

            response.AssertUnauthorizedStatusCode();
        }

        [Fact]
        public async Task AddQuestionToTicket_IfUserIsNotAuthenticated_QuestionShouldNotBeAddedToTicket()
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = await ApiTicketClient
                .CreateValidTicketWithQuestionIdsAsAuthorizedAsync();

            await ApiTicketClient
                .SendAddQuestionToTicketRequestAsync(
                    questionId: questionId,
                    ticketId: ticketId,
                    sentFromRole: null);

            var ticketQuestions = await ApiTicketClient
                .GetTicketQuestionsAsAuthorizedUserAsync(
                    ticketId: ticketId);

            ticketQuestions.Any(q => q.Id == questionId).Should().BeFalse();

            var questionTickets = await ApiQuestionClient
                .GetQuestionTicketsAsAuthorizedUserAsync(
                    questionId: questionId);

            questionTickets.Any(t => t.TicketId == ticketId).Should().BeFalse();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task AddQuestionToTicket_IfUserIsNotEligible_ShouldReturn403StatusCode(
            Role nonEligibleRole)
        {
            var response = await ApiTicketClient
                .SendAddQuestionToTicketRequestAsync(
                    questionId: Guid.NewGuid(),
                    ticketId: Guid.NewGuid(),
                    sentFromRole: nonEligibleRole);

            response.AssertForbiddenStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task AddQuestionToTicket_IfUserIsNotEligible_QuestionShouldNotBeAddedToTicket(
            Role nonEligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = await ApiTicketClient
                .CreateValidTicketWithQuestionIdsAsAuthorizedAsync();

            await ApiTicketClient
                .SendAddQuestionToTicketRequestAsync(
                    questionId: questionId,
                    ticketId: ticketId,
                    sentFromRole: nonEligibleRole);

            var ticketQuestions = await ApiTicketClient
                .GetTicketQuestionsAsAuthorizedUserAsync(
                    ticketId: ticketId);

            ticketQuestions.Any(q => q.Id == questionId).Should().BeFalse();

            var questionTickets = await ApiQuestionClient
                .GetQuestionTicketsAsAuthorizedUserAsync(
                    questionId: questionId);

            questionTickets.Any(t => t.TicketId == ticketId).Should().BeFalse();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTicket_IfTicketIsNotFound_ShouldReturn404StatusCode(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var response = await ApiTicketClient
                .SendAddQuestionToTicketRequestAsync(
                    questionId: questionId,
                    ticketId: Guid.NewGuid(),
                    sentFromRole: eligibleRole);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTicket_IfTicketIsNotFound_TicketShouldNotBeAddedToQuestion(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            await ApiTicketClient
                .SendAddQuestionToTicketRequestAsync(
                    questionId: questionId,
                    ticketId: Guid.NewGuid(),
                    sentFromRole: eligibleRole);

            var questionTickets = await ApiQuestionClient
                .GetQuestionTicketsAsAuthorizedUserAsync(
                    questionId: questionId);

            questionTickets.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTicket_IfQuestionIsNotFound_ShouldReturn404StatusCode(
            Role eligibleRole)
        {
            var ticketId = await ApiTicketClient
                .CreateValidTicketWithQuestionIdsAsAuthorizedAsync();

            var response = await ApiTicketClient
                .SendAddQuestionToTicketRequestAsync(
                    questionId: Guid.NewGuid(),
                    ticketId: ticketId,
                    sentFromRole: eligibleRole);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTicket_IfQuestionIsNotFound_QuestionShouldNotBeAddedToTicket(
            Role eligibleRole)
        {
            var ticketId = await ApiTicketClient
                .CreateValidTicketWithQuestionIdsAsAuthorizedAsync();

            var beforeTicketQuestionsCount = (await ApiTicketClient
                .GetTicketQuestionsAsAuthorizedUserAsync(ticketId)).Count();

            await ApiTicketClient
                .SendAddQuestionToTicketRequestAsync(
                    questionId: Guid.NewGuid(),
                    ticketId: ticketId,
                    sentFromRole: eligibleRole);

            var ticketQuestions = await ApiTicketClient
                .GetTicketQuestionsAsAuthorizedUserAsync(ticketId);

            ticketQuestions.Should().HaveCount(beforeTicketQuestionsCount);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTicket_IfQuestionAlreadyAddedToTicket_ShouldReturn400StatusCode(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = await ApiTicketClient
                .CreateValidTicketAsAuthorizedAsync(
                    questionIds: [questionId]);

            var response = await ApiTicketClient
                .SendAddQuestionToTicketRequestAsync(
                    questionId: questionId,
                    ticketId: ticketId,
                    sentFromRole: eligibleRole);

            response.AssertBadRequestStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTicket_IfQuestionAlreadyAddedToTicket_QuestionShouldNotBeAddedToTicket(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = await ApiTicketClient
                .CreateValidTicketAsAuthorizedAsync(
                    questionIds: [questionId]);

            await ApiTicketClient
                .SendAddQuestionToTicketRequestAsync(
                    questionId: questionId,
                    ticketId: ticketId,
                    sentFromRole: eligibleRole);

            var ticketQuestions = await ApiTicketClient
                .GetTicketQuestionsAsAuthorizedUserAsync(
                    ticketId: ticketId);

            ticketQuestions.Count(q => q.Id == questionId).Should().BeLessThanOrEqualTo(1);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTicket_IfQuestionAlreadyAddedToTicket_TicketShouldNotBeAddedToQuestion(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = await ApiTicketClient
                .CreateValidTicketAsAuthorizedAsync(
                    questionIds: [questionId]);

            await ApiTicketClient
                .SendAddQuestionToTicketRequestAsync(
                    questionId: questionId,
                    ticketId: ticketId,
                    sentFromRole: eligibleRole);

            var questionTickets = await ApiQuestionClient
                .GetQuestionTicketsAsAuthorizedUserAsync(
                    questionId: questionId);

            questionTickets.Count(q => q.TicketId == ticketId).Should().BeLessThanOrEqualTo(1);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTicket_IfValidCase_ShouldReturn204StatusCode(
           Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = await ApiTicketClient
                .CreateValidTicketWithQuestionIdsAsAuthorizedAsync();

            var response = await ApiTicketClient
                .SendAddQuestionToTicketRequestAsync(
                    questionId: questionId,
                    ticketId: ticketId,
                    sentFromRole: eligibleRole);

            response.AssertNoContentStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTicket_IfValidCase_QuestionShouldBeAddedToTicket(
           Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = await ApiTicketClient
                .CreateValidTicketWithQuestionIdsAsAuthorizedAsync();

            await ApiTicketClient
                .SendAddQuestionToTicketRequestAsync(
                    questionId: questionId,
                    ticketId: ticketId,
                    sentFromRole: eligibleRole);

            var ticketQuestions = await ApiTicketClient
                .GetTicketQuestionsAsAuthorizedUserAsync(
                    ticketId: ticketId);

            ticketQuestions.Any(q => q.Id == questionId).Should().BeTrue();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTicket_IfValidCase_TicketShouldBeAddedToQuestion(
           Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = await ApiTicketClient
                .CreateValidTicketWithQuestionIdsAsAuthorizedAsync();

            await ApiTicketClient
                .SendAddQuestionToTicketRequestAsync(
                    questionId: questionId,
                    ticketId: ticketId,
                    sentFromRole: eligibleRole);

            var questionTickets = await ApiQuestionClient
                .GetQuestionTicketsAsAuthorizedUserAsync(
                    questionId: questionId);

            questionTickets.Any(q => q.TicketId == ticketId).Should().BeTrue();
        }
    }
}
