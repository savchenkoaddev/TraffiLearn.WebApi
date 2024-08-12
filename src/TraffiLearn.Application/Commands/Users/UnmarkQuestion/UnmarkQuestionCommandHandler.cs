using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Commands.Users.MarkQuestion;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Questions;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Commands.Users.UnmarkQuestion
{
    internal sealed class UnmarkQuestionCommandHandler
        : IRequestHandler<UnmarkQuestionCommand, Result>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MarkQuestionCommandHandler> _logger;

        public UnmarkQuestionCommandHandler(
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
            UnmarkQuestionCommand request,
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

            var question = await _questionRepository.GetByIdAsync(
                questionId: new QuestionId(request.QuestionId.Value),
                cancellationToken);

            if (question is null)
            {
                return UserErrors.QuestionNotFound;
            }

            var markResult = caller.UnmarkQuestion(question);

            if (markResult.IsFailure)
            {
                return markResult.Error;
            }

            await _userRepository.UpdateAsync(caller);
            await _questionRepository.UpdateAsync(question);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Succesfully unmarked question. User's username: {username}", caller.Username.Value);

            return Result.Success();
        }
    }
}
