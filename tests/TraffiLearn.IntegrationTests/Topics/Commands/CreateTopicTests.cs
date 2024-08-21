using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using TraffiLearn.Application.Topics.Commands.Create;
using TraffiLearn.Application.Topics.DTO;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Auth;
using TraffiLearn.IntegrationTests.Helpers;

namespace TraffiLearn.IntegrationTests.Topics.Commands
{
    public sealed class CreateTopicTests : BaseIntegrationTest
    {
        private readonly Authenticator _authenticator;
        private readonly RequestSender _requestSender;

        public CreateTopicTests(
            WebApplicationFactory factory)
            : base(factory)
        {
            _authenticator = new(HttpClient);
            _requestSender = new(HttpClient, _authenticator);
        }

        [Fact]
        public async Task CreateTopic_IfUserNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await SendUnauthenticatedCreateTopicRequestAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task CreateTopic_IfUserNotAuthenticated_TopicShouldNotBeCreated()
        {
            await SendUnauthenticatedCreateTopicRequestAsync();

            var topics = await GetAllTopicsSortedByNumberAsUserAsync();

            topics.Should().BeEmpty();
        }

        private async Task<HttpResponseMessage> SendUnauthenticatedCreateTopicRequestAsync()
        {
            var command = CreateValidCreateTopicCommand();

            return await HttpClient.PostAsJsonAsync(
                requestUri: TopicRoutes.CreateTopicRoute,
                command);
        }


        [Fact]
        public async Task CreateTopic_IfUserIsNotEligible_ShouldReturn403StatusCode()
        {
            var response = await SendNonEligibleCreateTopicRequestAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task CreateTopic_IfUserIsNotEligible_TopicShouldNotBeCreated()
        {
            await SendNonEligibleCreateTopicRequestAsync();

            var topics = await GetAllTopicsSortedByNumberAsUserAsync();

            topics.Should().BeEmpty();
        }

        private async Task<HttpResponseMessage> SendNonEligibleCreateTopicRequestAsync()
        {
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Post,
                requestUri: TopicRoutes.CreateTopicRoute);

            var request = CreateValidCreateTopicCommand();

            return await _requestSender
                .SendJsonRequestAsRegularUserWithTestCredentials(
                    httpRequestMessage,
                    request);
        }


        [Fact]
        public async Task CreateTopic_IfPassedInvalidArgsAndUserIsAdmin_ShouldReturn400StatusCode()
        {
            var invalidCommands = GetInvalidCommands();

            foreach (var command in invalidCommands)
            {
                var response = await SendCreateTopicRequestAsAdminAsync(command);

                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        [Fact]
        public async Task CreateTopic_IfPassedInvalidArgsAndUserIsAdmin_TopicShouldNotBeCreated()
        {
            var invalidCommands = GetInvalidCommands();

            foreach (var command in invalidCommands)
            {
                await SendCreateTopicRequestAsAdminAsync(command);

                var topics = await GetAllTopicsSortedByNumberAsUserAsync();

                topics.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task CreateTopic_IfPassedValidArgsAndUserIsAdmin_ShouldReturn201Or204StatusCode()
        {
            var command = CreateValidCreateTopicCommand();

            var response = await SendCreateTopicRequestAsAdminAsync(command);

            AssertStatusCodeIs201Or204(response);
        }

        [Fact]
        public async Task CreateTopic_IfPassedValidArgsAndUserIsAdmin_TopicShouldBeCreated()
        {
            var topicNumber = 1;
            var title = "title";

            var command = new CreateTopicCommand(
                topicNumber,
                title);

            var response = await SendCreateTopicRequestAsAdminAsync(command);

            var topics = await GetAllTopicsSortedByNumberAsUserAsync();

            topics.Should().HaveCount(1);
            topics.First().Title.Should().Be(title);
        }

        private async Task<HttpResponseMessage> SendCreateTopicRequestAsAdminAsync(
            CreateTopicCommand command)
        {
            var httpRequestMessage = new HttpRequestMessage(
                    method: HttpMethod.Post,
                    requestUri: TopicRoutes.CreateTopicRoute);

            return await _requestSender
                .SendJsonRequestAsAdminWithTestCredentials(
                    httpRequestMessage,
                    command);
        }


        [Fact]
        public async Task CreateTopic_IfPassedInvalidArgsAndUserIsOwner_ShouldReturn400StatusCode()
        {
            var invalidCommands = GetInvalidCommands();

            foreach (var command in invalidCommands)
            {
                var response = await SendCreateTopicRequestAsOwnerAsync(command);

                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        [Fact]
        public async Task CreateTopic_IfPassedInvalidArgsAndUserIsOwner_TopicShouldNotBeCreated()
        {
            var invalidCommands = GetInvalidCommands();

            foreach (var command in invalidCommands)
            {
                await SendCreateTopicRequestAsOwnerAsync(command);

                var topics = await GetAllTopicsSortedByNumberAsUserAsync();

                topics.Should().BeEmpty();
            }
        }
        
        [Fact]
        public async Task CreateTopic_IfPassedValidArgsAndUserIsOwner_ShouldReturn201Or204StatusCode()
        {
            var command = CreateValidCreateTopicCommand();

            var response = await SendCreateTopicRequestAsOwnerAsync(command);

            AssertStatusCodeIs201Or204(response);
        }

        [Fact]
        public async Task CreateTopic_IfPassedValidArgsAndUserIsOwner_TopicShouldBeCreated()
        {
            var topicNumber = 1;
            var title = "title";

            var command = new CreateTopicCommand(
                topicNumber,
                title);

            var response = await SendCreateTopicRequestAsOwnerAsync(command);

            var topics = await GetAllTopicsSortedByNumberAsUserAsync();

            topics.Should().HaveCount(1);
            topics.First().Title.Should().Be(title);
        }

        private async Task<HttpResponseMessage> SendCreateTopicRequestAsOwnerAsync(
            CreateTopicCommand command)
        {
            var httpRequestMessage = new HttpRequestMessage(
                    method: HttpMethod.Post,
                    requestUri: TopicRoutes.CreateTopicRoute);

            return await _requestSender
                .SendJsonRequestAsOwnerWithTestCredentials(
                    httpRequestMessage,
                    command);
        }

        private CreateTopicCommand CreateValidCreateTopicCommand()
        {
            return new CreateTopicCommand(
                TopicNumber: 1,
                Title: "title");
        }

        private void AssertStatusCodeIs201Or204(HttpResponseMessage response)
        {
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.Created,
                HttpStatusCode.NoContent);
        }

        private Task<IEnumerable<TopicResponse>> GetAllTopicsSortedByNumberAsUserAsync()
        {
            return _requestSender
                    .GetFromJsonAsRegularUserAsync<IEnumerable<TopicResponse>>(
                        TopicRoutes.GetAllSortedTopicsByNumberRoute);
        }

        private List<CreateTopicCommand> GetInvalidCommands()
        {
            return [
                new CreateTopicCommand(null, "title"),
                new CreateTopicCommand(1, string.Empty),
                new CreateTopicCommand(-1, "title"),
                new CreateTopicCommand(1, null),
                new CreateTopicCommand(1, new string('1', TopicTitle.MaxLength + 1))
            ];
        }
    }
}

