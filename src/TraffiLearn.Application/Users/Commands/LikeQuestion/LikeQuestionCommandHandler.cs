using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.Errors;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Users.Commands.LikeQuestion
{
    internal sealed class LikeQuestionCommandHandler
        : IRequestHandler<LikeQuestionCommand, Result>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LikeQuestionCommandHandler(
            IUserContextService<Guid> userContextService,
            IQuestionRepository questionRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _userContextService = userContextService;
            _questionRepository = questionRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            LikeQuestionCommand request,
            CancellationToken cancellationToken)
        {
            var callerId = new UserId(_userContextService.FetchAuthenticatedUserId());

            var caller = await _userRepository.GetByIdWithLikedAndDislikedQuestionsIdsAsync(
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

            var likeQuestionResult = caller.LikeQuestion(question.Id);

            if (likeQuestionResult.IsFailure)
            {
                return likeQuestionResult.Error;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
