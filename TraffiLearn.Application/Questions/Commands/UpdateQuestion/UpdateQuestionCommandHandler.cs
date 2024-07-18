using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Questions.Commands.UpdateQuestion
{
    public sealed class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand>
    {
        private readonly IBlobService _blobService;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly QuestionMapper _questionMapper = new();

        public UpdateQuestionCommandHandler(
            IQuestionRepository questionRepository,
            IUnitOfWork unitOfWork,
            IBlobService blobService)
        {
            _questionRepository = questionRepository;
            _unitOfWork = unitOfWork;
            _blobService = blobService;
        }

        public async Task Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            if (request.RequestObject.Answers.All(a => a.IsCorrect == false))
            {
                throw new AllAnswersAreIncorrectException();
            }

            var oldEntityObject = await _questionRepository.GetByIdAsync(request.QuestionId.Value);

            if (oldEntityObject is null)
            {
                throw new QuestionNotFoundException(request.QuestionId.Value);
            }

            var newEntityObject = _questionMapper.ToEntity(request.RequestObject);
            newEntityObject.Id = oldEntityObject.Id;

            oldEntityObject.Answers = newEntityObject.Answers;

            var image = request.Image;

            if (image is not null)
            {
                if (oldEntityObject.ImageUri is not null)
                {
                    await _blobService.DeleteAsync(
                        oldEntityObject.ImageUri,
                        cancellationToken);
                }

                using Stream stream = image.OpenReadStream();

                var uploadResponse = await _blobService.UploadAsync(
                    stream,
                    image.ContentType,
                    cancellationToken);

                newEntityObject.ImageUri = uploadResponse.BlobUri;
            }
            else
            {
                if (oldEntityObject.ImageUri is not null)
                {
                    await _blobService.DeleteAsync(
                        oldEntityObject.ImageUri,
                        cancellationToken);
                }
            }

            await _questionRepository.UpdateAsync(oldEntityObject, newEntityObject);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
