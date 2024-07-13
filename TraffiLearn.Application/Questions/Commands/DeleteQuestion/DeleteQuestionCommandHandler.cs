using MediatR;
using TraffiLearn.Application.Data;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Questions.Commands.DeleteQuestion
{
    public sealed class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteQuestionCommandHandler(
            IQuestionRepository questionRepository,
            IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            var found = await _questionRepository.GetByIdAsync(request.QuestionId.Value);

            if (found is null)
            {
                throw new QuestionNotFoundException(request.QuestionId.Value);
            }

            await _questionRepository.DeleteAsync(found);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
