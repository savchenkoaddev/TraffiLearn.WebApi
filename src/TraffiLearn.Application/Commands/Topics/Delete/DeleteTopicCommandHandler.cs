using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Errors.Topics;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Topics;

namespace TraffiLearn.Application.Commands.Topics.Delete
{
    internal sealed class DeleteTopicCommandHandler : IRequestHandler<DeleteTopicCommand, Result>
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

        public async Task<Result> Handle(
            DeleteTopicCommand request,
            CancellationToken cancellationToken)
        {
            var found = await _topicRepository.GetByIdAsync(
                topicId: new TopicId(request.TopicId.Value),
                cancellationToken);

            if (found is null)
            {
                return TopicErrors.NotFound;
            }

            await _topicRepository.DeleteAsync(found);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
