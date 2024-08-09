using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Enums;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Queries.Users.GetUserDislikedQuestions
{
    internal sealed class GetUserDislikedQuestionsQueryHandler
        : IRequestHandler<GetUserDislikedQuestionsQuery, Result<IEnumerable<QuestionResponse>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserManagementService _userManagementService;
        private readonly Mapper<Question, QuestionResponse> _questionMapper;

        public GetUserDislikedQuestionsQueryHandler(
            IUserRepository userRepository,
            IUserManagementService userManagementService,
            Mapper<Question, QuestionResponse> questionMapper)
        {
            _userRepository = userRepository;
            _userManagementService = userManagementService;
            _questionMapper = questionMapper;
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> Handle(
            GetUserDislikedQuestionsQuery request, 
            CancellationToken cancellationToken)
        {
            var callingUserResult = await _userManagementService.GetAuthenticatedUserAsync(
                cancellationToken);

            if (callingUserResult.IsFailure)
            {
                return Result.Failure<IEnumerable<QuestionResponse>>(callingUserResult.Error);
            }

            var callingUser = callingUserResult.Value;

            UserId userId = new(request.UserId.Value);

            if (IsNotAllowedToGet(userId, callingUser))
            {
                return Result.Failure<IEnumerable<QuestionResponse>>(
                    UserErrors.NotAllowedToPerformAction);
            }

            var user = await _userRepository.GetByIdAsync(
                userId: new UserId(request.UserId.Value),
                cancellationToken,
                includeExpressions: user => user.DislikedQuestions);

            if (user is null)
            {
                return Result.Failure<IEnumerable<QuestionResponse>>(UserErrors.NotFound);
            }

            return Result.Success(_questionMapper.Map(user.DislikedQuestions));
        }

        private static bool IsNotAllowedToGet(UserId userId, User callingUser)
        {
            return callingUser.Role < Role.Admin &&
                   callingUser.Id != userId;
        }
    }
}
