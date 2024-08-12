using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Topics;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Topics;

namespace TraffiLearn.Application.Commands.Topics.Update
{
    internal sealed class UpdateTopicCommandHandler : IRequestHandler<UpdateTopicCommand, Result>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly Mapper<UpdateTopicCommand, Result<Topic>> _commandMapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTopicCommandHandler(
            ITopicRepository topicRepository,
            Mapper<UpdateTopicCommand, Result<Topic>> commandMapper,
            IUnitOfWork unitOfWork)
        {
            _topicRepository = topicRepository;
            _commandMapper = commandMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            UpdateTopicCommand request,
            CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetByIdAsync(
                topicId: new TopicId(request.TopicId.Value),
                cancellationToken);

            if (topic is null)
            {
                return TopicErrors.NotFound;
            }

            var mappingResult = _commandMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return mappingResult.Error;
            }

            Result updateResult = topic.Update(
                topic: mappingResult.Value);

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
