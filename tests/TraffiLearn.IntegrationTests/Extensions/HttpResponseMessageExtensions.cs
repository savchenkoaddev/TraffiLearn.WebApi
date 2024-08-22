using FluentAssertions;
using System.Net;

namespace TraffiLearn.IntegrationTests.Extensions
{
    internal static class HttpResponseMessageExtensions
    {
        public static void AssertStatusCode(
            this HttpResponseMessage responseMessage, 
            HttpStatusCode statusCode)
        {
            responseMessage.StatusCode.Should().Be(statusCode);
        }

        public static void AssertStatusCodeOneOf(
            this HttpResponseMessage responseMessage, 
            params HttpStatusCode[] expectedStatusCodes)
        {
            responseMessage.StatusCode.Should().BeOneOf(expectedStatusCodes);
        }

        public static void AssertForbiddenStatusCode(
            this HttpResponseMessage responseMessage)
        {
            responseMessage.AssertStatusCode(HttpStatusCode.Forbidden);
        }

        public static void AssertBadRequestStatusCode(
            this HttpResponseMessage responseMessage)
        {
            responseMessage.AssertStatusCode(HttpStatusCode.BadRequest);
        }

        public static void AssertUnauthorizedStatusCode(
            this HttpResponseMessage responseMessage)
        {
            responseMessage.AssertStatusCode(HttpStatusCode.Unauthorized);
        }

        public static void AssertCreatedOrNoContentStatusCode(
            this HttpResponseMessage responseMessage)
        {
            responseMessage.AssertStatusCodeOneOf(
                HttpStatusCode.Created,
                HttpStatusCode.NoContent);
        }

        public static void AssertNoContentStatusCode(
            this HttpResponseMessage responseMessage)
        {
            responseMessage.AssertStatusCode(HttpStatusCode.NoContent);
        }

        public static void AssertNotFoundStatusCode(
            this HttpResponseMessage responseMessage)
        {
            responseMessage.AssertStatusCode(HttpStatusCode.NotFound);
        }
    }
}
