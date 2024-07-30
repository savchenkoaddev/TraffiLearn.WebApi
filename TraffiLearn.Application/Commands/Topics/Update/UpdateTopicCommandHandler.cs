using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Errors.Topics;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.Commands.Topics.Update
{
    internal sealed class UpdateTopicCommandHandler : IRequestHandler<UpdateTopicCommand, Result>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTopicCommandHandler(
            ITopicRepository topicRepository,
            IUnitOfWork unitOfWork)
        {
            _topicRepository = topicRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            UpdateTopicCommand request, 
            CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetByIdAsync(request.TopicId.Value);

            if (topic is null)
            {
                return TopicErrors.NotFound;
            }

            Result<TopicNumber> topicNumberResult = TopicNumber.Create(request.TopicNumber.Value);

            if (topicNumberResult.IsFailure)
            {
                return topicNumberResult.Error;
            }

            Result<TopicTitle> topicTitleResult = TopicTitle.Create(request.Title);

            if (topicTitleResult.IsFailure)
            {
                return topicTitleResult.Error;
            }

            Result updateResult = topic.Update(
                number: topicNumberResult.Value,
                title: topicTitleResult.Value);

            if (updateResult.IsFailure)
            {
                return updateResult.Error;
            }

            await _topicRepository.UpdateAsync(topic);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
