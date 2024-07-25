using MediatR;
using TraffiLearn.Application.Abstractions;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Topics.Request;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Topics.Commands.CreateTopic
{
    public sealed class CreateTopicCommandHandler : IRequestHandler<CreateTopicCommand>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper<TopicRequest, Topic> _topicMapper;

        public CreateTopicCommandHandler(
            ITopicRepository topicRepository,
            IUnitOfWork unitOfWork)
        {
            _topicRepository = topicRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CreateTopicCommand request, CancellationToken cancellationToken)
        {
            var topic = _topicMapper.Map(request.RequestObject);

            await _topicRepository.AddAsync(topic);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
