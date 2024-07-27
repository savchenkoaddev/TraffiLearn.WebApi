using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Topics.Request;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Topics.Commands.CreateTopic
{
    public sealed class CreateTopicCommandHandler : IRequestHandler<CreateTopicCommand>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly Mapper<TopicRequest, Topic> _topicMapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTopicCommandHandler(
            ITopicRepository topicRepository,
            Mapper<TopicRequest, Topic> topicMapper,
            IUnitOfWork unitOfWork)
        {
            _topicRepository = topicRepository;
            _unitOfWork = unitOfWork;
            _topicMapper = topicMapper;
        }

        public async Task Handle(CreateTopicCommand request, CancellationToken cancellationToken)
        {
            var topic = _topicMapper.Map(request.RequestObject);

            await _topicRepository.AddAsync(topic);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
