using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Commands.Questions.Delete
{
    public sealed class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand>
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

        public async Task Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            var found = await _questionRepository.GetByIdAsync(request.QuestionId.Value);

            if (found is null)
            {
                throw new ArgumentException("Question has not been found");
            }

            if (found.ImageUri is not null)
            {
                await _blobService.DeleteAsync(
                    blobUri: found.ImageUri.Value,
                    cancellationToken);
            }

            await _questionRepository.DeleteAsync(found);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
