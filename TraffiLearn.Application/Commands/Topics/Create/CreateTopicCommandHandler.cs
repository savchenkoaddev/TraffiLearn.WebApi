using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Commands.Topics.Create
{
    public sealed class CreateTopicCommandHandler : IRequestHandler<CreateTopicCommand>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly Mapper<CreateTopicCommand, Topic> _topicMapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTopicCommandHandler(
            ITopicRepository topicRepository,
            Mapper<CreateTopicCommand, Topic> topicMapper,
            IUnitOfWork unitOfWork)
        {
            _topicRepository = topicRepository;
            _unitOfWork = unitOfWork;
            _topicMapper = topicMapper;
        }

        public async Task Handle(CreateTopicCommand request, CancellationToken cancellationToken)
        {
            var topic = _topicMapper.Map(request);

            await _topicRepository.AddAsync(topic);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
