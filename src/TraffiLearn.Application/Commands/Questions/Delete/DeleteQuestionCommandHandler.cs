using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Domain.Errors.Questions;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Questions;

namespace TraffiLearn.Application.Commands.Questions.Delete
{
    internal sealed class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, Result>
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

        public async Task<Result> Handle(
            DeleteQuestionCommand request,
            CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(
                questionId: new QuestionId(request.QuestionId.Value),
                cancellationToken);

            if (question is null)
            {
                return QuestionErrors.NotFound;
            }

            if (question.ImageUri is not null)
            {
                var blobUri = question.ImageUri.Value;

                await _blobService.DeleteAsync(
                    blobUri,
                    cancellationToken);
            }

            await _questionRepository.DeleteAsync(question);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
