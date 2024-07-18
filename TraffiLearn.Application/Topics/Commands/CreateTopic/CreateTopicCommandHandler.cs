using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Topics.Commands.CreateTopic
{
    public sealed class CreateTopicCommandHandler : IRequestHandler<CreateTopicCommand>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TopicMapper _topicMapper;

        public CreateTopicCommandHandler(
            ITopicRepository topicRepository,
            IUnitOfWork unitOfWork)
        {
            _topicRepository = topicRepository;
            _unitOfWork = unitOfWork;
            _topicMapper = new TopicMapper();
        }

        public async Task Handle(CreateTopicCommand request, CancellationToken cancellationToken)
        {
            var topic = _topicMapper.ToEntity(request.RequestObject);

            await _topicRepository.AddAsync(topic);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
