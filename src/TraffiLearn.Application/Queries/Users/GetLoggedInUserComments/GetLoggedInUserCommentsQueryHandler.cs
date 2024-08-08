using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Auth;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Comments;
using TraffiLearn.Application.Errors;
using TraffiLearn.Application.Identity;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Queries.Users.GetLoggedInUserComments
{
    internal sealed class GetLoggedInUserCommentsQueryHandler
        : IRequestHandler<GetLoggedInUserCommentsQuery, Result<IEnumerable<CommentResponse>>>
    {
        private readonly IAuthService<ApplicationUser> _authService;
        private readonly IUserRepository _userRepository;
        private readonly Mapper<Comment, CommentResponse> _commentMapper;
        private readonly ILogger<GetLoggedInUserCommentsQueryHandler> _logger;

        public GetLoggedInUserCommentsQueryHandler(
            IAuthService<ApplicationUser> authService,
            IUserRepository userRepository,
            Mapper<Comment, CommentResponse> commentMapper,
            ILogger<GetLoggedInUserCommentsQueryHandler> logger)
        {
            _authService = authService;
            _userRepository = userRepository;
            _commentMapper = commentMapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<CommentResponse>>> Handle(
            GetLoggedInUserCommentsQuery request,
            CancellationToken cancellationToken)
        {
            var userIdResult = _authService.GetAuthenticatedUserId();

            if (userIdResult.IsFailure)
            {
                return Result.Failure<IEnumerable<CommentResponse>>(userIdResult.Error);
            }

            UserId userId = new(userIdResult.Value);

            var userExists = await _userRepository.ExistsAsync(
                userId,
                cancellationToken);

            if (!userExists)
            {
                _logger.LogCritical(InternalErrors.AuthenticatedUserNotFound.Description);

                return Result.Failure<IEnumerable<CommentResponse>>(InternalErrors.AuthenticatedUserNotFound);
            }

            var comments = await _userRepository.GetUserCommentsWithRepliesAsync(
                userId,
                cancellationToken);

            _logger.LogInformation("Succesfully fetched authenticated user comments. Comments fetched: {count}", comments.Count());

            return Result.Success(_commentMapper.Map(comments));
        }
    }
}
