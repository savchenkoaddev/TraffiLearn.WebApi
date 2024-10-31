﻿using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Questions;
using TraffiLearn.Domain.Users;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Users.Commands.MarkQuestion
{
    internal sealed class MarkQuestionCommandHandler
        : IRequestHandler<MarkQuestionCommand, Result>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MarkQuestionCommandHandler(
            IUserContextService<Guid> userContextService,
            IUserRepository userRepository,
            IQuestionRepository questionRepository,
            IUnitOfWork unitOfWork)
        {
            _userContextService = userContextService;
            _userRepository = userRepository;
            _questionRepository = questionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            MarkQuestionCommand request,
            CancellationToken cancellationToken)
        {
            var callerId = new UserId(_userContextService.GetAuthenticatedUserId());

            var caller = await _userRepository.GetByIdWithMarkedQuestionsAsync(
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

            var markQuestionResult = caller.MarkQuestion(question);

            if (markQuestionResult.IsFailure)
            {
                return markQuestionResult.Error;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
