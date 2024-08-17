using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Users.Commands.MarkQuestion;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.Errors;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Users.Commands.UnmarkQuestion
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

            var caller = await _userRepository.GetByIdWithMarkedQuestionsIdsAsync(
                callerId,
                cancellationToken);

            if (caller is null)
            {
                throw new InvalidOperationException("Authenticated user is not found.");
            }

            var question = await _questionRepository.GetByIdAsync(
                questionId: new QuestionId(request.QuestionId.Value),
                cancellationToken);

            if (question is null)
            {
                return QuestionErrors.NotFound;
            }

            var unmarkQuestionResult = caller.UnmarkQuestion(question.Id);

            if (unmarkQuestionResult.IsFailure)
            {
                return unmarkQuestionResult.Error;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
