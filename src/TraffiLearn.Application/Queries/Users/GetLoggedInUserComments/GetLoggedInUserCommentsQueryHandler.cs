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
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly Mapper<Comment, CommentResponse> _commentMapper;
        private readonly ILogger<GetLoggedInUserCommentsQueryHandler> _logger;

        public GetLoggedInUserCommentsQueryHandler(
            IUserContextService<Guid> userContextService,
            IUserRepository userRepository,
            Mapper<Comment, CommentResponse> commentMapper,
            ILogger<GetLoggedInUserCommentsQueryHandler> logger)
        {
            _userContextService = userContextService;
            _userRepository = userRepository;
            _commentMapper = commentMapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<CommentResponse>>> Handle(
            GetLoggedInUserCommentsQuery request,
            CancellationToken cancellationToken)
        {
            var userId = new UserId(_userContextService.FetchAuthenticatedUserId());

            var user = await _userRepository.GetUserWithCommentsWithRepliesAsync(
                userId,
                cancellationToken);

            if (user is null)
            {
                throw new InvalidOperationException("Authenticated user not found.");
            }

            _logger.LogInformation(
                "Succesfully fetched authenticated user comments. Comments fetched: {count}",
                user.Comments.Count);

            return Result.Success(_commentMapper.Map(user.Comments));
        }
    }
}
