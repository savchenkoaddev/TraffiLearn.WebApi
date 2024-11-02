using TraffiLearn.Domain.Users.Roles;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Tickets.Commands.CreateTicket
{
    public sealed class CreateTicketTests : TicketIntegrationTest
    {
        private readonly CreateTicketCommandFactory _commandFactory;

        public CreateTicketTests(
            WebApplicationFactory factory)
            : base(factory)
        {
            _commandFactory = new CreateTicketCommandFactory(ApiQuestionClient);
        }

        [Fact]
        public async Task CreateTicket_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await ApiTicketClient
                .SendCreateTicketRequestWithQuestionsAsync(
                    sentFromRole: null);

            response.AssertUnauthorizedStatusCode();
        }

        [Fact]
        public async Task CreateTicket_IfUserIsNotAuthenticated_TicketShouldNotBeCreated()
        {
            await ApiTicketClient
                .SendCreateTicketRequestWithQuestionsAsync(
                    sentFromRole: null);

            var allTickets = await ApiTicketClient.GetAllTicketsAsAuthorizedUserAsync();

            allTickets.Should().BeEmpty();
        }

        [Fact]
        public async Task CreateTicket_IfUserIsNotAuthenticated_QuestionShouldNotContainNewlyCreatedTicket()
        {
            var questionId = await ApiQuestionClient.CreateValidQuestionWithTopicAsAuthorizedAsync();

            await ApiTicketClient
                .SendCreateTicketRequestAsync(
                    questionIds: [questionId],
                    sentFromRole: null);

            var questionTickets = await ApiQuestionClient
                .GetQuestionTicketsAsAuthorizedUserAsync(questionId);

            questionTickets.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task CreateTicket_IfUserIsNotEligible_ShouldReturn403StatusCode(
            Role nonEligibleRole)
        {
            var response = await ApiTicketClient
                .SendCreateTicketRequestWithQuestionsAsync(
                    sentFromRole: nonEligibleRole);

            response.AssertForbiddenStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task CreateTicket_IfUserIsNotEligible_TicketShouldNotBeCreated(
            Role nonEligibleRole)
        {
            await ApiTicketClient
                .SendCreateTicketRequestWithQuestionsAsync(
                    sentFromRole: nonEligibleRole);

            var allTickets = await ApiTicketClient.GetAllTicketsAsAuthorizedUserAsync();

            allTickets.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task CreateTicket_IfUserIsNotEligible_QuestionShouldNotContainNewlyCreatedTicket(
            Role nonEligibleRole)
        {
            var questionId = await ApiQuestionClient.CreateValidQuestionWithTopicAsAuthorizedAsync();

            await ApiTicketClient
                .SendCreateTicketRequestAsync(
                    questionIds: [questionId],
                    sentFromRole: nonEligibleRole);

            var questionTickets = await ApiQuestionClient
                .GetQuestionTicketsAsAuthorizedUserAsync(questionId);

            questionTickets.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateTicket_IfPassedInvalidArgs_ShouldReturn400StatusCode(
            Role eligibleRole)
        {
            var invalidCommands = _commandFactory.CreateInvalidCommands();

            await RequestSender.EnsureEachSentJsonRequestReturnsBadRequestAsync(
                method: HttpMethod.Post,
                requestUri: TicketEndpointRoutes.CreateTicketRoute,
                requests: invalidCommands,
                sentFromRole: eligibleRole);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateTicket_IfPassedInvalidArgs_TicketShouldNotBeCreated(
            Role eligibleRole)
        {
            var invalidCommands = _commandFactory.CreateInvalidCommands();

            await RequestSender.SendAllAsJsonAsync(
                method: HttpMethod.Post,
                requestUri: TicketEndpointRoutes.CreateTicketRoute,
                requests: invalidCommands,
                sentFromRole: eligibleRole);

            var allTickets = await ApiTicketClient.GetAllTicketsAsAuthorizedUserAsync();

            allTickets.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateTicket_IfValidCase_ShouldReturn204Or201StatusCode(
            Role eligibleRole)
        {
            var response = await ApiTicketClient.SendCreateTicketRequestWithQuestionsAsync(
                sentFromRole: eligibleRole);

            response.AssertCreatedOrNoContentStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateTicket_IfValidCase_TicketShouldBeCreated(
            Role eligibleRole)
        {
            var ticketId = await ApiTicketClient.CreateValidTicketWithQuestionIdsAsync(
                createdWithRole: eligibleRole);

            var allTopics = await ApiTicketClient.GetAllTicketsAsAuthorizedUserAsync();

            allTopics.Should().HaveCount(1);
            allTopics.First().TicketId.Should().Be(ticketId);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateTicket_IfValidCase_TicketShouldContainQuestions(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = await ApiTicketClient.CreateValidTicketAsync(
                questionIds: [questionId],
                createdWithRole: eligibleRole);

            var ticketQuestions = await ApiTicketClient.GetTicketQuestionsAsAuthorizedUserAsync(ticketId);

            ticketQuestions.Single().Id.Should().Be(questionId);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateTicket_IfValidCase_QuestionShouldContainTicket(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient.CreateValidQuestionWithTopicAsAuthorizedAsync();

            var ticketId = await ApiTicketClient.CreateValidTicketAsync(
                questionIds: [questionId],
                createdWithRole: eligibleRole);

            var questionTickets = await ApiQuestionClient.GetQuestionTicketsAsAuthorizedUserAsync(questionId);

            questionTickets.Single().TicketId.Should().Be(ticketId);
        }
    }
}
