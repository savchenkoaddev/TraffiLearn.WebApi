using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
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
        private readonly ICommentRepository _commentRepository;
        private readonly Mapper<Comment, CommentResponse> _commentMapper;

        public GetUserCommentsQueryHandler(
            IUserRepository userRepository,
            ICommentRepository commentRepository,
            Mapper<Comment, CommentResponse> commentMapper)
        {
            _userRepository = userRepository;
            _commentRepository = commentRepository;
            _commentMapper = commentMapper;
        }

        public async Task<Result<IEnumerable<CommentResponse>>> Handle(
            GetUserCommentsQuery request,
            CancellationToken cancellationToken)
        {
            var userId = new UserId(request.UserId.Value);

            var userExists = await _userRepository.ExistsAsync(
                userId,
                cancellationToken);

            if (!userExists)
            {
                return Result.Failure<IEnumerable<CommentResponse>>(UserErrors.NotFound);
            }

            var userComments = await _commentRepository.GetUserCreatedCommentsAsync(
                userId,
                cancellationToken);

            return Result.Success(_commentMapper.Map(userComments));
        }
    }
}
