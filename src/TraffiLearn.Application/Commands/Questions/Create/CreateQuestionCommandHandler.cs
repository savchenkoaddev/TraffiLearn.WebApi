using MediatR;
using Microsoft.AspNetCore.Http;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Topics;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Questions;
using TraffiLearn.Domain.ValueObjects.Topics;

namespace TraffiLearn.Application.Commands.Questions.Create
{
    internal sealed class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, Result>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IBlobService _blobService;
        private readonly Mapper<CreateQuestionCommand, Result<Question>> _questionMapper;
        private readonly IUserManagementService _userManagementService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateQuestionCommandHandler(
            IQuestionRepository questionRepository,
            ITopicRepository topicRepository,
            IBlobService blobService,
            IUserManagementService userManagementService,
            Mapper<CreateQuestionCommand, Result<Question>> questionMapper,
            IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _topicRepository = topicRepository;
            _blobService = blobService;
            _userManagementService = userManagementService;
            _questionMapper = questionMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
        CreateQuestionCommand request,
            CancellationToken cancellationToken)
        {
            var authorizationResult = await _userManagementService.EnsureCallerCanModifyDomainObjects(
                cancellationToken);

            if (authorizationResult.IsFailure)
            {
                return authorizationResult.Error;
            }

            var mappingResult = _questionMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return mappingResult.Error;
            }

            var question = mappingResult.Value;

            var addResult = await AddTopics(
                question,
                topicsIds: request.TopicsIds,
                cancellationToken);

            if (addResult.IsFailure)
            {
                return addResult.Error;
            }

            var imageResult = await HandleImage(
                question,
                image: request.Image,
                cancellationToken);

            if (imageResult.IsFailure)
            {
                return imageResult.Error;
            }

            await _questionRepository.AddAsync(
                question,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private async Task<Result> AddTopics(
            Question question,
            List<Guid?> topicsIds,
            CancellationToken cancellationToken = default)
        {
            foreach (var topicId in topicsIds)
            {
                var topic = await _topicRepository.GetByIdAsync(
                    topicId: new TopicId(topicId.Value),
                    cancellationToken);

                if (topic is null)
                {
                    return TopicErrors.NotFound;
                }

                Result topicAddResult = question.AddTopic(topic);

                if (topicAddResult.IsFailure)
                {
                    return topicAddResult.Error;
                }

                Result questionAddResult = topic.AddQuestion(question);

                if (questionAddResult.IsFailure)
                {
                    return questionAddResult.Error;
                }
            }

            return Result.Success();
        }

        private async Task<Result> HandleImage(
            Question question,
            IFormFile? image,
            CancellationToken cancellationToken)
        {
            if (image is not null)
            {
                using Stream stream = image.OpenReadStream();

                var uploadResponse = await _blobService.UploadAsync(
                    stream,
                    contentType: image.ContentType,
                    cancellationToken);

                Result<ImageUri> imageUriResult = ImageUri.Create(uploadResponse.BlobUri);

                if (imageUriResult.IsFailure)
                {
                    return imageUriResult.Error;
                }

                question.SetImageUri(imageUriResult.Value);
            }

            return Result.Success();
        }
    }
}
