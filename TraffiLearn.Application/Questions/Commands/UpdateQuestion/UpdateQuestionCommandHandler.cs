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

            var image = request.RequestObject.Image;

            if (image is not null)
            {
                if (oldEntityObject.ImageName is not null)
                {
                    await _blobService.DeleteAsync(
                        oldEntityObject.ImageName,
                        cancellationToken);
                }

                using Stream stream = image.OpenReadStream();

                var imageName = await _blobService.UploadAsync(
                    stream,
                    image.ContentType,
                    cancellationToken);

                newEntityObject.ImageName = imageName;
            }

            await _questionRepository.UpdateAsync(oldEntityObject, newEntityObject);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
