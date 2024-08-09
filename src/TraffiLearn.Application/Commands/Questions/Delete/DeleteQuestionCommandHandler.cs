using MediatR;
using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Application.Options;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Questions;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Questions;

namespace TraffiLearn.Application.Commands.Questions.Delete
{
    internal sealed class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, Result>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IBlobService _blobService;
        private readonly IUserManagementService _userManagementService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteQuestionCommandHandler(
            IQuestionRepository questionRepository,
            IBlobService blobService,
            IUserManagementService userManagementService,
            IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _blobService = blobService;
            _userManagementService = userManagementService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            DeleteQuestionCommand request, 
            CancellationToken cancellationToken)
        {
            var authorizationResult = await _userManagementService.EnsureCallerCanModifyDomainObjects(
                cancellationToken);

            if (authorizationResult.IsFailure)
            {
                return authorizationResult.Error;
            }

            var found = await _questionRepository.GetByIdAsync(
                questionId: new QuestionId(request.QuestionId.Value),
                cancellationToken);

            if (found is null)
            {
                return QuestionErrors.NotFound;
            }

            if (found.ImageUri is not null)
            {
                var blobUri = found.ImageUri.Value;

                await _blobService.DeleteAsync(
                    blobUri,
                    cancellationToken);
            }

            await _questionRepository.DeleteAsync(found);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
