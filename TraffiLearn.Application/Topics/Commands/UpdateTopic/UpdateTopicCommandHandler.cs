using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Topics.Request;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Topics.Commands.UpdateTopic
{
    public sealed class UpdateTopicCommandHandler : IRequestHandler<UpdateTopicCommand>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Mapper<TopicRequest, Topic> _topicMapper;

        public UpdateTopicCommandHandler(
            ITopicRepository topicRepository,
            IUnitOfWork unitOfWork,
            Mapper<TopicRequest, Topic> topicMapper)
        {
            _topicRepository = topicRepository;
            _unitOfWork = unitOfWork;
            _topicMapper = topicMapper;
        }

        public async Task Handle(UpdateTopicCommand request, CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetByIdAsync(request.TopicId);

            if (topic is null)
            {
                throw new TopicNotFoundException(request.TopicId);
            }

            topic.UpdateTopic(
                request.RequestObject.Number,
                request.RequestObject.Title);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
