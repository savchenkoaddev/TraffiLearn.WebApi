using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Domain.Errors.Questions;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Questions.Delete
{
    public sealed class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, Result>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IBlobService _blobService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteQuestionCommandHandler(
            IQuestionRepository questionRepository,
            IBlobService blobService,
            IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _blobService = blobService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            var found = await _questionRepository.GetByIdAsync(request.QuestionId.Value);

            if (found is null)
            {
                return QuestionErrors.NotFound;
            }

            if (found.ImageUri is not null)
            {
                await _blobService.DeleteAsync(
                    blobUri: found.ImageUri.Value,
                    cancellationToken);
            }

            await _questionRepository.DeleteAsync(found);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
