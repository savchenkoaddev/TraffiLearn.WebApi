using MediatR;
using TraffiLearn.Application.Data;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Questions.Commands.CreateQuestion
{
    public sealed class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly QuestionMapper _questionMapper = new();

        public CreateQuestionCommandHandler(
            IQuestionRepository questionRepository,
            ITopicRepository topicRepository,
            IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _topicRepository = topicRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {
            if (request.RequestObject.Answers.All(a => a.IsCorrect == false))
            {
                throw new AllAnswersAreIncorrectException();
            }

            var entity = _questionMapper.ToEntity(request.RequestObject);

            foreach (var topicId in request.RequestObject.TopicsIds)
            {
                var topic = await _topicRepository.GetByIdAsync(topicId.Value);

                if (topic is null)
                {
                    throw new TopicNotFoundException(topicId.Value);
                }

                entity.Topics.Add(topic);
            }

            await _questionRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
