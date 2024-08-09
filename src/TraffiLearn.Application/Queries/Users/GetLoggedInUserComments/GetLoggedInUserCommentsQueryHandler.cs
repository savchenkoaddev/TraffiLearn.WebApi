using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.DTO.Comments;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Queries.Users.GetLoggedInUserComments
{
    internal sealed class GetLoggedInUserCommentsQueryHandler
        : IRequestHandler<GetLoggedInUserCommentsQuery, Result<IEnumerable<CommentResponse>>>
    {
        private readonly IUserManagementService _userManagementService;
        private readonly IUserRepository _userRepository;
        private readonly Mapper<Comment, CommentResponse> _commentMapper;
        private readonly ILogger<GetLoggedInUserCommentsQueryHandler> _logger;

        public GetLoggedInUserCommentsQueryHandler(
            IUserManagementService userManagementService,
            IUserRepository userRepository,
            Mapper<Comment, CommentResponse> commentMapper,
            ILogger<GetLoggedInUserCommentsQueryHandler> logger)
        {
            _userManagementService = userManagementService;
            _userRepository = userRepository;
            _commentMapper = commentMapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<CommentResponse>>> Handle(
            GetLoggedInUserCommentsQuery request,
            CancellationToken cancellationToken)
        {
            var userResult = await _userManagementService.GetAuthenticatedUserAsync(
                cancellationToken);

            if (userResult.IsFailure)
            {
                return Result.Failure<IEnumerable<CommentResponse>>(userResult.Error);
            }

            UserId userId = userResult.Value.Id;

            var user = await _userRepository.GetUserWithCommentsWithRepliesAsync(
                userId,
                cancellationToken);

            _logger.LogInformation(
                "Succesfully fetched authenticated user comments. Comments fetched: {count}",
                user!.Comments.Count);

            return Result.Success(_commentMapper.Map(user!.Comments));
        }
    }
}
