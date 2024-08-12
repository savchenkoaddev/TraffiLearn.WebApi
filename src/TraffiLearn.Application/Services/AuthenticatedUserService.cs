using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Services
{
    internal sealed class AuthenticatedUserService : IAuthenticatedUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContextService<Guid> _userContextService;
        private readonly ILogger<AuthenticatedUserService> _logger;

        public AuthenticatedUserService(
            IUserRepository userRepository,
            IUserContextService<Guid> userContextService,
            ILogger<AuthenticatedUserService> logger)
        {
            _userRepository = userRepository;
            _userContextService = userContextService;
            _logger = logger;
        }

        public async Task<User> GetAuthenticatedUserAsync(
            CancellationToken cancellationToken = default)
        {
            var callerId = _userContextService.FetchAuthenticatedUserId();

            _logger.LogInformation("Succesfully fetched caller id. Caller ID: {CallerId}", callerId);

            var user = await _userRepository.GetByIdAsync(
                new UserId(callerId),
                cancellationToken);

            if (user is null)
            {
                throw new InvalidOperationException("User authenticated succesfully, but is not found in the storage. ");
            }

            return user;
        }
    }
}
