using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using TraffiLearn.Application.Abstractions.AI;
using TraffiLearn.Application.AI.DTO;
using TraffiLearn.Infrastructure.Exceptions;
using TraffiLearn.Infrastructure.External.GroqAI.DTO;
using TraffiLearn.Infrastructure.External.GroqAI.Options;

namespace TraffiLearn.Infrastructure.External.GroqAI
{
    internal sealed class GroqAIService : IAIService
    {
        private const string ApiName = "GroqApi";
        private readonly HttpClient _httpClient;
        private readonly GroqAISettings _settings;


        public GroqAIService(
            HttpClient httpClient,
            IOptions<GroqAISettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<AITextResponse> SendTextQueryAsync(
            AITextRequest? request,
            CancellationToken cancellationToken = default)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.Value))
            {
                throw new ArgumentException("Request value cannot be null or whitespace.", nameof(request));
            }

            GroqAIRequest groqAIRequest = GenerateTextRequest(request);

            var httpResponse = await _httpClient.PostAsJsonAsync(
                _settings.CreateChatCompletionUri,
                groqAIRequest,
                cancellationToken);

            httpResponse.EnsureSuccessStatusCode();

            var apiResponse = await httpResponse.Content.ReadFromJsonAsync<GroqAIResponse>(cancellationToken);

            if (ApiResponseStructureHasNoNullValues(apiResponse))
            {
                throw new ApiBadResponseException(ApiName);
            }

            return ParseTextResponse(apiResponse!);
        }

        private static bool ApiResponseStructureHasNoNullValues(GroqAIResponse? apiResponse)
        {
            return apiResponse is null ||
                   apiResponse.Choices is null ||
                   apiResponse.Choices.Exists(x => x.Message is null);
        }

        private static AITextResponse ParseTextResponse(GroqAIResponse apiResponse)
        {
            return new AITextResponse(apiResponse.Choices[0].Message.Content);
        }

        private GroqAIRequest GenerateTextRequest(AITextRequest request)
        {
            return new(
                Messages: [
                    new(
                        Role: "user",
                        Content: request.Value)
                ],
                Model: _settings.Model);
        }
    }
}