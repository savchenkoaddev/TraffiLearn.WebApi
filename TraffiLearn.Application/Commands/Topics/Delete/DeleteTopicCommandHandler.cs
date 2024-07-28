using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Commands.Topics.Delete
{
    public sealed class DeleteTopicCommandHandler : IRequestHandler<DeleteTopicCommand>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTopicCommandHandler(
            ITopicRepository topicRepository,
            IUnitOfWork unitOfWork)
        {
            _topicRepository = topicRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteTopicCommand request, CancellationToken cancellationToken)
        {
            var found = await _topicRepository.GetByIdAsync(request.TopicId);

            if (found is null)
            {
                throw new ArgumentException("Topic has not been found");
            }

            await _topicRepository.DeleteAsync(found);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
