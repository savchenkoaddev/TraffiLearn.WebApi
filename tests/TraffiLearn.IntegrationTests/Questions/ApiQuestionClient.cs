using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Application.Topics.DTO;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Helpers;
using TraffiLearn.IntegrationTests.Questions.CreateQuestion;

namespace TraffiLearn.IntegrationTests.Questions
{
    public sealed class ApiQuestionClient
    {
        private readonly RequestSender _requestSender;

        public ApiQuestionClient(RequestSender requestSender)
        {
            _requestSender = requestSender;
        }

        public async Task<Guid> CreateValidQuestionAsync(
            List<Guid> topicIds,
            IFormFile? image = null)
        {
            var request = CreateQuestionFixtureFactory.CreateValidCommand(
                topicIds,
                image);

            var response = await _requestSender.SendMultipartFormDataWithJsonAndFileRequest(
                method: HttpMethod.Post,
                requestUri: QuestionEndpointRoutes.CreateQuestionRoute,
                value: request,
                file: image,
                sentFromRole: Role.Owner);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Guid>();
        }

        public Task<IEnumerable<QuestionResponse>> GetAllQuestionAsync()
        {
            return _requestSender.GetFromJsonAsync<IEnumerable<QuestionResponse>>(
                requestUri: QuestionEndpointRoutes.GetAllQuestionsRoute,
                getFromRole: Role.Owner);
        }

        public Task<IEnumerable<TopicResponse>> GetQuestionTopicsAsync(Guid questionId)
        {
            return _requestSender.GetFromJsonAsync<IEnumerable<TopicResponse>>(
                requestUri: QuestionEndpointRoutes.GetQuestionTopicsRoute(questionId),
                getFromRole: Role.Owner);
        }
    }
}
