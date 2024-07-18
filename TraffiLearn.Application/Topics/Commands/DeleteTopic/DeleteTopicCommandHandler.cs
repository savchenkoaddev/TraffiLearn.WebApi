using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Topics.Commands.DeleteTopic
{
    public sealed class DeleteTopicCommandHandler : IRequestHandler<DeleteTopicCommand>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTopicCommandHandler(ITopicRepository topicRepository, IUnitOfWork unitOfWork)
        {
            _topicRepository = topicRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteTopicCommand request, CancellationToken cancellationToken)
        {
            var found = await _topicRepository.GetByIdAsync(request.TopicId.Value);

            if (found is null)
            {
                throw new TopicNotFoundException(request.TopicId.Value);
            }

            await _topicRepository.DeleteAsync(found);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
