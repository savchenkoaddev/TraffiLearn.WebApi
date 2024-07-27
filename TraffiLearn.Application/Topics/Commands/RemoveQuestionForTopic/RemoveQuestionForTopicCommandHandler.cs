using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Topics.Commands.RemoveQuestionForTopic
{
    public sealed class RemoveQuestionForTopicCommandHandler : IRequestHandler<RemoveQuestionForTopicCommand>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveQuestionForTopicCommandHandler(
            ITopicRepository topicRepository,
            IUnitOfWork unitOfWork)
        {
            _topicRepository = topicRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(RemoveQuestionForTopicCommand request, CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetByIdAsync(
                request.TopicId,
                includeExpression: x => x.Questions);

            if (topic is null)
            {
                throw new TopicNotFoundException(request.TopicId);
            }

            topic.RemoveQuestion(request.TopicId);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
