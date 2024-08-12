using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Questions;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Commands.Users.MarkQuestion
{
    internal sealed class MarkQuestionCommandHandler
        : IRequestHandler<MarkQuestionCommand, Result>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MarkQuestionCommandHandler> _logger;

        public MarkQuestionCommandHandler(
            IUserContextService<Guid> userContextService,
            IUserRepository userRepository,
            IQuestionRepository questionRepository,
            IUnitOfWork unitOfWork,
            ILogger<MarkQuestionCommandHandler> logger)
        {
            _userContextService = userContextService;
            _userRepository = userRepository;
            _questionRepository = questionRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(
            MarkQuestionCommand request,
            CancellationToken cancellationToken)
        {
            var callerId = new UserId(_userContextService.FetchAuthenticatedUserId());

            var caller = await _userRepository.GetByIdAsync(
                callerId,
                cancellationToken,
                includeExpressions: [
                    user => user.MarkedQuestions
                ]);

            if (caller is null)
            {
                throw new InvalidOperationException("Authenticated user not found.");
            }

            var questionBeingMarked = await _questionRepository.GetByIdAsync(
                questionId: new QuestionId(request.QuestionId.Value),
                cancellationToken);

            if (questionBeingMarked is null)
            {
                return UserErrors.QuestionNotFound;
            }

            var markResult = caller.MarkQuestion(questionBeingMarked);

            if (markResult.IsFailure)
            {
                return markResult.Error;
            }

            await _userRepository.UpdateAsync(caller);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Succesfully marked question. User's username: {username}", caller.Username.Value);

            return Result.Success();
        }
    }
}
