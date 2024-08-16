using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Topics.Create
{
    internal sealed class CreateTopicCommandHandler : IRequestHandler<CreateTopicCommand, Result>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly Mapper<CreateTopicCommand, Result<Topic>> _topicMapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTopicCommandHandler(
            ITopicRepository topicRepository,
            Mapper<CreateTopicCommand, Result<Topic>> topicMapper,
            IUnitOfWork unitOfWork)
        {
            _topicRepository = topicRepository;
            _topicMapper = topicMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            CreateTopicCommand request,
            CancellationToken cancellationToken)
        {
            var mappingResult = _topicMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return mappingResult.Error;
            }

            var topic = mappingResult.Value;

            await _topicRepository.AddAsync(
                topic,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
