using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Topics.Create
{
    internal sealed class CreateTopicCommandHandler : IRequestHandler<CreateTopicCommand, Result>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IUserManagementService _userManagementService;
        private readonly Mapper<CreateTopicCommand, Result<Topic>> _topicMapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTopicCommandHandler(
            ITopicRepository topicRepository,
            IUserManagementService userManagementService,
            Mapper<CreateTopicCommand, Result<Topic>> topicMapper,
            IUnitOfWork unitOfWork)
        {
            _topicRepository = topicRepository;
            _userManagementService = userManagementService;
            _topicMapper = topicMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            CreateTopicCommand request, 
            CancellationToken cancellationToken)
        {
            var authorizationResult = await _userManagementService.EnsureCallerCanModifyDomainObjects(
    cancellationToken);

            if (authorizationResult.IsFailure)
            {
                return authorizationResult.Error;
            }

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
