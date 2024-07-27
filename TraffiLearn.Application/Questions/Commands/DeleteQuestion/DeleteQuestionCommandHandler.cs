using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Questions.Commands.DeleteQuestion
{
    public sealed class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand>
    {
        private readonly IBlobService _blobService;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteQuestionCommandHandler(
            IQuestionRepository questionRepository,
            IUnitOfWork unitOfWork,
            IBlobService blobService)
        {
            _questionRepository = questionRepository;
            _unitOfWork = unitOfWork;
            _blobService = blobService;
        }

        public async Task Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            var found = await _questionRepository.GetByIdAsync(request.QuestionId);

            if (found is null)
            {
                throw new QuestionNotFoundException(request.QuestionId);
            }

            if (found.ImageUri is not null)
            {
                await _blobService.DeleteAsync(
                    found.ImageUri,
                    cancellationToken);
            }

            await _questionRepository.DeleteAsync(found);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
