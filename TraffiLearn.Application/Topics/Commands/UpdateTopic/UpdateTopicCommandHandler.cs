using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Topics.Commands.UpdateTopic
{
    public sealed class UpdateTopicCommandHandler : IRequestHandler<UpdateTopicCommand>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TopicMapper _topicMapper;

        public UpdateTopicCommandHandler(
            ITopicRepository topicRepository, 
            IUnitOfWork unitOfWork)
        {
            _topicRepository = topicRepository;
            _unitOfWork = unitOfWork;
            _topicMapper = new TopicMapper();
        }

        public async Task Handle(UpdateTopicCommand request, CancellationToken cancellationToken)
        {
            var oldTopicObject = await _topicRepository.GetByIdAsync(request.TopicId.Value);

            if (oldTopicObject is null)
            {
                throw new TopicNotFoundException(request.TopicId.Value);
            }

            var newTopicObject = _topicMapper.ToEntity(request.RequestObject);
            newTopicObject.Id = request.TopicId.Value;

            await _topicRepository.UpdateAsync(oldTopicObject, newTopicObject);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
