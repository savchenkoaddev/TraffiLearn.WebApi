using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Commands.Topics.RemoveQuestionForTopic
{
    public sealed class RemoveQuestionForTopicCommandHandler : IRequestHandler<RemoveQuestionForTopicCommand>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveQuestionForTopicCommandHandler(
            ITopicRepository topicRepository,
            IQuestionRepository questionRepository,
            IUnitOfWork unitOfWork)
        {
            _topicRepository = topicRepository;
            _questionRepository = questionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(RemoveQuestionForTopicCommand request, CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetByIdAsync(
                request.TopicId,
                includeExpression: x => x.Questions);

            if (topic is null)
            {
                throw new ArgumentException("Topic has not been found");
            }

            var question = await _questionRepository.GetByIdAsync(
                request.QuestionId,
                includeExpression: x => x.Topics);

            if (question is null)
            {
                throw new ArgumentException("Question has not been found");
            }

            topic.RemoveQuestion(question);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
