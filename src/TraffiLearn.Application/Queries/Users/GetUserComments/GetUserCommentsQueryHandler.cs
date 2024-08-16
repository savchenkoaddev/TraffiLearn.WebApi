using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Comments;
using TraffiLearn.Domain.Aggregates.Comments;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.Errors;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Users.GetUserComments
{
    internal sealed class GetUserCommentsQueryHandler
        : IRequestHandler<GetUserCommentsQuery, Result<IEnumerable<CommentResponse>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly Mapper<Comment, CommentResponse> _commentMapper;

        public GetUserCommentsQueryHandler(
            IUserRepository userRepository,
            Mapper<Comment, CommentResponse> commentMapper)
        {
            _userRepository = userRepository;
            _commentMapper = commentMapper;
        }

        public async Task<Result<IEnumerable<CommentResponse>>> Handle(
            GetUserCommentsQuery request,
            CancellationToken cancellationToken)
        {
            var userId = new UserId(request.UserId.Value);

            var user = await _userRepository.GetUserWithCommentsWithRepliesAsync(
                userId,
                cancellationToken);

            if (user is null)
            {
                return Result.Failure<IEnumerable<CommentResponse>>(
                    UserErrors.NotFound);
            }

            return Result.Success(_commentMapper.Map(user.Comments));
        }
    }
}
