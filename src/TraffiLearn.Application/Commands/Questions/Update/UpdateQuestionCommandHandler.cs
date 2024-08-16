using MediatR;
using Microsoft.AspNetCore.Http;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.Errors;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Aggregates.Topics.Errors;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Questions.Update
{
    internal sealed class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, Result>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IBlobService _blobService;
        private readonly Mapper<UpdateQuestionCommand, Result<Question>> _commandMapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateQuestionCommandHandler(
            IQuestionRepository questionRepository,
            ITopicRepository topicRepository,
            IBlobService blobService,
            Mapper<UpdateQuestionCommand, Result<Question>> commandMapper,
            IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _topicRepository = topicRepository;
            _blobService = blobService;
            _commandMapper = commandMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            UpdateQuestionCommand request,
            CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdWithTopicsAsync(
                questionId: new QuestionId(request.QuestionId.Value),
                cancellationToken);

            if (question is null)
            {
                return QuestionErrors.NotFound;
            }

            var mappingResult = _commandMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return mappingResult.Error;
            }

            var newQuestion = mappingResult.Value;

            var updateResult = question.Update(
                newQuestion.Content,
                newQuestion.Explanation,
                newQuestion.QuestionNumber,
                newQuestion.Answers.ToList(),
                imageUri: null);

            if (updateResult.IsFailure)
            {
                return updateResult.Error;
            }

            var updateTopicsResult = await UpdateTopics(
                topicIds: request.TopicIds,
                question,
                cancellationToken);

            if (updateTopicsResult.IsFailure)
            {
                return updateTopicsResult.Error;
            }

            await _questionRepository.UpdateAsync(question);

            var removeOldImageIfNewImageMissing =
                request.RemoveOldImageIfNewImageMissing.Value;

            var imageUpdateResult = await HandleImageAsync(
                image: request.Image,
                question,
                removeOldImageIfNewImageMissing,
                cancellationToken);

            if (imageUpdateResult.IsFailure)
            {
                return imageUpdateResult.Error;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private async Task<Result> UpdateTopics(
            List<Guid>? topicIds,
            Question question,
            CancellationToken cancellationToken = default)
        {
            foreach (var topicId in topicIds)
            {
                var topic = await _topicRepository.GetByIdAsync(
                    topicId: new TopicId(topicId),
                    cancellationToken);

                if (topic is null)
                {
                    return TopicErrors.NotFound;
                }

                if (!question.TopicIds.Contains(topic.Id))
                {
                    var topicAddResult = question.AddTopic(topic.Id);

                    if (topicAddResult.IsFailure)
                    {
                        return topicAddResult.Error;
                    }
                }
            }

            var questionTopicsIds = question.TopicIds.ToList();

            foreach (var topicId in questionTopicsIds)
            {
                if (!topicIds.Contains(topicId.Value))
                {
                    var topicRemoveResult = question.RemoveTopic(topicId);

                    if (topicRemoveResult.IsFailure)
                    {
                        return topicRemoveResult.Error;
                    }
                }
            }

            return Result.Success();
        }

        private async Task<Result> HandleImageAsync(
            IFormFile? image,
            Question question,
            bool removeOldImageIfNewImageMissing,
            CancellationToken cancellationToken = default)
        {
            var blobUri = question.ImageUri.Value;

            if (image is null)
            {
                if (question.ImageUri is not null && removeOldImageIfNewImageMissing)
                {
                    await _blobService.DeleteAsync(
                        blobUri,
                        cancellationToken);

                    question.SetImageUri(null);
                }
            }
            else
            {
                if (question.ImageUri is not null)
                {
                    await _blobService.DeleteAsync(
                        blobUri,
                        cancellationToken);
                }

                using Stream stream = image.OpenReadStream();

                var uploadResponse = await _blobService.UploadAsync(
                    stream,
                    contentType: image.ContentType,
                    cancellationToken);

                var imageUriResult = ImageUri.Create(uploadResponse.BlobUri);

                if (imageUriResult.IsFailure)
                {
                    return Result.Failure(Error.InternalFailure());
                }

                question.SetImageUri(imageUriResult.Value);
            }

            return Result.Success();
        }
    }
}
