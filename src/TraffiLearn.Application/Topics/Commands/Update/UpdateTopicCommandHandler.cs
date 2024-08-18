using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Aggregates.Topics.Errors;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Topics.Commands.Update
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
                topicNumber: mappingResult.Value.Number,
                topicTitle: mappingResult.Value.Title);

            if (updateResult.IsFailure)
            {
                return updateResult.Error;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
