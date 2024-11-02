using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Domain.Questions;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Questions.Commands.Delete
{
    internal sealed class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, Result>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IImageService _imageService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteQuestionCommandHandler(
            IQuestionRepository questionRepository,
            IImageService imageService,
            IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _imageService = imageService;
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

            Func<Task> transactionAction = async () =>
            {
                await _questionRepository.DeleteAsync(question);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                if (question.ImageUri is not null)
                {
                    await _imageService.DeleteAsync(
                        imageUri: question.ImageUri,
                        cancellationToken);
                }
            };

            await _unitOfWork.ExecuteInTransactionAsync(
                transactionAction,
                cancellationToken: cancellationToken);

            return Result.Success();
        }
    }
}
