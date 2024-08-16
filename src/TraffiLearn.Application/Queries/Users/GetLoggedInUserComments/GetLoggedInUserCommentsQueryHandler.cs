using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.DTO.Comments;
using TraffiLearn.Domain.Aggregates.Comments;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Users.GetLoggedInUserComments
{
    internal sealed class GetLoggedInUserCommentsQueryHandler
        : IRequestHandler<GetLoggedInUserCommentsQuery, Result<IEnumerable<CommentResponse>>>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly Mapper<Comment, CommentResponse> _commentMapper;
        private readonly ILogger<GetLoggedInUserCommentsQueryHandler> _logger;

        public GetLoggedInUserCommentsQueryHandler(
            IUserContextService<Guid> userContextService,
            IUserRepository userRepository,
            ICommentRepository commentRepository,
            Mapper<Comment, CommentResponse> commentMapper,
            ILogger<GetLoggedInUserCommentsQueryHandler> logger)
        {
            _userContextService = userContextService;
            _userRepository = userRepository;
            _commentRepository = commentRepository;
            _commentMapper = commentMapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<CommentResponse>>> Handle(
            GetLoggedInUserCommentsQuery request,
            CancellationToken cancellationToken)
        {
            var userId = new UserId(_userContextService.FetchAuthenticatedUserId());

            var userExists = await _userRepository.ExistsAsync(
                userId,
                cancellationToken);

            if (!userExists)
            {
                throw new InvalidOperationException("Authenticated user is not found.");
            }

            var userComments = await _commentRepository.GetUserCommentsAsync(
                userId,
                cancellationToken);

            return Result.Success(_commentMapper.Map(userComments));
        }
    }
}
