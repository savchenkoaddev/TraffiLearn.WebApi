using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using TraffiLearn.Application.AI.DTO;
using TraffiLearn.Infrastructure.Exceptions;
using TraffiLearn.Infrastructure.External.GroqAI;
using TraffiLearn.Infrastructure.External.GroqAI.Options;

namespace TraffiLearn.UnitTests.Services
{
    public sealed class GroqAIServiceTests
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly GroqApiSettings _settings;
        private readonly GroqApiService _service;

        public GroqAIServiceTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>();

            _settings = new GroqApiSettings
            {
                Model = "model",
                ApiKey = "key",
                BaseUri = "https://example.com",
                CreateChatCompletionUri = "completion"
            };

            var httpClient = new HttpClient(_handlerMock.Object);

            httpClient.BaseAddress = new(_settings.BaseUri);

            _service = new GroqApiService(
                httpClient,
                Options.Create(_settings));
        }

        [Fact]
        public async Task SendTextQueryAsync_ShouldSendRequestOnValidUri()
        {
            // Arrange
            var requestUri = $"{_settings.BaseUri}/{_settings.CreateChatCompletionUri}";
            var requestText = new AITextRequest("Hello.");
            var apiResponse = """
                {
                  "choices": [
                    {
                      "message": {
                        "role": "assistant",
                        "content": "Hello sir."
                      }
                    }
                  ]
                }
                """;
            var expectedContent = "Hello sir.";

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(apiResponse)
            };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    nameof(HttpClient.SendAsync),
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString() == requestUri),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _service.SendTextQueryAsync(requestText);

            // Assert
            result.Value.Should().Be(expectedContent);
        }

        [Fact]
        public async Task SendTextQueryAsync_IfApiReturnedNotSuccessfulStatusCode_ShouldThrowHttpRequestException()
        {
            // Arrange
            var requestText = new AITextRequest("Hello.");

            var response = new HttpResponseMessage(HttpStatusCode.Forbidden);

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    nameof(HttpClient.SendAsync),
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var action = async () => await _service.SendTextQueryAsync(requestText);

            // Assert
            await action.Should().ThrowAsync<HttpRequestException>();
        }

        [Fact]
        public async Task SendTextQueryAsync_IfRequestParameterIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            var action = async () => await _service.SendTextQueryAsync(null!);

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task SendTextQueryAsync_IfRequestParameterValueIsNull_ShouldThrowArgumentException()
        {
            // Arrange
            var requestText = new AITextRequest(null!);

            // Act
            var action = async () => await _service.SendTextQueryAsync(requestText);

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task SendTextQueryAsync_IfRequestParameterValueIsWhitespace_ShouldThrowArgumentException()
        {
            // Arrange
            var requestText = new AITextRequest("   ");

            // Act
            var action = async () => await _service.SendTextQueryAsync(requestText);

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task SendTextQueryAsync_IfApiReturnedEmptyJSONResponse_ShouldThrowApiBadResponseException()
        {
            // Arrange
            var requestText = new AITextRequest("Hello.");
            var apiResponse = """
                {}
                """;

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(apiResponse)
            };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    nameof(HttpClient.SendAsync),
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var action = async () => await _service.SendTextQueryAsync(requestText);

            // Assert
            await action.Should().ThrowAsync<ApiBadResponseException>();
        }

        [Fact]
        public async Task SendTextQueryAsync_IfPositiveScenario_ShouldNotReturnNull()
        {
            // Arrange
            var requestText = new AITextRequest("Hello.");
            var apiResponse = """
                {
                  "choices": [
                    {
                      "message": {
                        "role": "assistant",
                        "content": "Hello sir."
                      }
                    }
                  ]
                }
                """;

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(apiResponse)
            };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    nameof(HttpClient.SendAsync),
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _service.SendTextQueryAsync(requestText);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task SendTextQueryAsync_IfPositiveScenario_ShouldReturnNonNullOrNonWhitespaceResponse()
        {
            // Arrange
            var requestText = new AITextRequest("Hello.");
            var apiResponse = """
                {
                  "choices": [
                    {
                      "message": {
                        "role": "assistant",
                        "content": "Hello sir."
                      }
                    }
                  ]
                }
                """;

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(apiResponse)
            };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    nameof(HttpClient.SendAsync),
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _service.SendTextQueryAsync(requestText);

            // Assert
            result.Value.Should().NotBeNullOrWhiteSpace();
        }
    }
}
