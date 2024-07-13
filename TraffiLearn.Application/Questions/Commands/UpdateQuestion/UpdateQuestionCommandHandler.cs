using MediatR;
using TraffiLearn.Application.Data;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Questions.Commands.UpdateQuestion
{
    public sealed class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly QuestionMapper _questionMapper = new();

        public UpdateQuestionCommandHandler(
            IQuestionRepository questionRepository,
            IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var oldEntityObject = await _questionRepository.GetByIdAsync(request.QuestionId.Value);

            if (oldEntityObject is null)
            {
                throw new QuestionNotFoundException(request.QuestionId.Value);
            }

            var newEntityObject = _questionMapper.ToEntity(request.RequestObject);
            newEntityObject.Id = oldEntityObject.Id;

            await _questionRepository.UpdateAsync(oldEntityObject, newEntityObject);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
