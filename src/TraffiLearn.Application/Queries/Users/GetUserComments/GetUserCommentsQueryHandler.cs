using MediatR;
using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.DTO.Comments;
using TraffiLearn.Application.Options;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Queries.Users.GetUserComments
{
    internal sealed class GetUserCommentsQueryHandler
        : IRequestHandler<GetUserCommentsQuery, Result<IEnumerable<CommentResponse>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly Mapper<Comment, CommentResponse> _commentMapper;
        private readonly IUserManagementService _userManagementService;
        private readonly AuthSettings _authSettings;

        public GetUserCommentsQueryHandler(
            IUserRepository userRepository,
            Mapper<Comment, CommentResponse> commentMapper,
            IUserManagementService userManagementService,
            IOptions<AuthSettings> authSettings)
        {
            _userRepository = userRepository;
            _commentMapper = commentMapper;
            _userManagementService = userManagementService;
            _authSettings = authSettings.Value;
        }

        public async Task<Result<IEnumerable<CommentResponse>>> Handle(
            GetUserCommentsQuery request,
            CancellationToken cancellationToken)
        {
            var userResult = await _userManagementService.GetAuthenticatedUserAsync(
                cancellationToken);

            if (userResult.IsFailure)
            {
                return Result.Failure<IEnumerable<CommentResponse>>(userResult.Error);
            }

            UserId userId = new(request.UserId.Value);

            var callingUser = userResult.Value;

            if (IsNotAllowedToGet(userId, callingUser))
            {
                return Result.Failure<IEnumerable<CommentResponse>>(
                    UserErrors.NotAllowedToPerformAction);
            }

            var userBeingLooked = await _userRepository.GetUserWithCommentsWithRepliesAsync(
                userId,
                cancellationToken);

            return Result.Success(_commentMapper.Map(userBeingLooked!.Comments));
        }

        private bool IsNotAllowedToGet(UserId userBeingLookedId, User callingUser)
        {
            return callingUser.Role < _authSettings.MinAllowedRoleToGetUserComments &&
                callingUser.Id != userBeingLookedId;
        }
    }
}
