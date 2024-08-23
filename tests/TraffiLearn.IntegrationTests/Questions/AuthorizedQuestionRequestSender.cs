using Microsoft.AspNetCore.Http;
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

            var content = await response.Content.ReadAsStringAsync();

            if (content is null)
            {
                throw new InvalidOperationException("API didn't send the question id as the response.");
            }

            return Guid.Parse(content);
        }

        public async Task<IEnumerable<QuestionResponse>> GetAllQuestionAsync()
        {
            return await _requestSender.GetFromJsonAsync<IEnumerable<QuestionResponse>>(
                requestUri: QuestionEndpointRoutes.GetAllQuestionsRoute,
                getFromRole: Role.Owner);
        }
    }
}
