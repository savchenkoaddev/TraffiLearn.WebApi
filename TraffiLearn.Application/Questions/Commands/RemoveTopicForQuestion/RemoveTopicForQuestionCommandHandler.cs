using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Questions.Commands.RemoveTopicForQuestion
{
    public sealed class RemoveTopicForQuestionCommandHandler : IRequestHandler<RemoveTopicForQuestionCommand>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveTopicForQuestionCommandHandler(
            IQuestionRepository questionRepository,
            ITopicRepository topicRepository,
            IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _topicRepository = topicRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(RemoveTopicForQuestionCommand request, CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetByIdAsync(request.TopicId);

            if (topic is null)
            {
                throw new TopicNotFoundException(request.TopicId);
            }

            var question = await _questionRepository.GetByIdAsync(
                request.QuestionId,
                includeExpression: x => x.Topics);

            if (question is null)
            {
                throw new QuestionNotFoundException(request.QuestionId);
            }

            question.RemoveTopic(topic);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
