using Microsoft.Extensions.Caching.Memory;
using TraffiLearn.Domain.Aggregates.Users.Roles;

namespace TraffiLearn.IntegrationTests.Extensions
{
    internal static class MemoryCacheExtensions
    {
        private static readonly TimeSpan
            DefaultAbsoluteExpirationRelativeToNowTimeSpan =
                TimeSpan.FromMinutes(5);

        public static bool TryGetAccessTokenForRole(
            this IMemoryCache cache,
            Role role,
            out string? accessToken)
        {
            var cacheKey = GenerateAccessTokenCacheKey(role);

            if (cache.TryGetValue(cacheKey, out string? cachedToken))
            {
                accessToken = cachedToken;

                return true;
            }

            accessToken = null;

            return false;
        }

        public static void SetAccessToken(
            this IMemoryCache cache,
            Role role,
            string accessToken,
            MemoryCacheEntryOptions? options = null!)
        {
            var cacheKey = GenerateAccessTokenCacheKey(role);

            if (options is null)
            {
                options = GenerateDefaultEntryOptions();
            }

            cache.Set(
                key: cacheKey,
                value: accessToken,
                options: options);
        }

        private static MemoryCacheEntryOptions GenerateDefaultEntryOptions()
        {
            return new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow =
                        DefaultAbsoluteExpirationRelativeToNowTimeSpan,
            };
        }

        private static string GenerateAccessTokenCacheKey(Role role)
        {
            return $"AccessToken_{role}";
        }
    }
}
