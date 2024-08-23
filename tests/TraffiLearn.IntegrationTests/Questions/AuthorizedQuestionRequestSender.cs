using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Helpers;
using TraffiLearn.IntegrationTests.Questions.CreateQuestion;

namespace TraffiLearn.IntegrationTests.Questions
{
    public sealed class AuthorizedQuestionRequestSender
    {
        private readonly RequestSender _requestSender;

        public AuthorizedQuestionRequestSender(RequestSender requestSender)
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

        public async Task<IEnumerable<QuestionResponse>> GetAllQuestionAsync()
        {
            return await _requestSender.GetFromJsonAsync<IEnumerable<QuestionResponse>>(
                requestUri: QuestionEndpointRoutes.GetAllQuestionsRoute,
                getFromRole: Role.Owner);
        }
    }
}
