using MediatR;
using Microsoft.AspNetCore.Http;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Questions;
using TraffiLearn.Domain.Errors.Topics;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Questions;
using TraffiLearn.Domain.ValueObjects.Topics;

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

        public async Task<Result> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(
                questionId: new QuestionId(request.QuestionId.Value),
                cancellationToken,
                includeExpressions: question => question.Topics);

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
                topicsIds: request.TopicsIds,
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
            List<Guid?>? topicsIds,
            Question question,
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

                if (!question.Topics.Contains(topic))
                {
                    var topicAddResult = question.AddTopic(topic);

                    if (topicAddResult.IsFailure)
                    {
                        return topicAddResult.Error;
                    }

                    var questionAddResult = topic.AddQuestion(question);

                    if (questionAddResult.IsFailure)
                    {
                        return questionAddResult.Error;
                    }
                }
            }

            var questionTopics = question.Topics.ToList();

            foreach (var topic in questionTopics)
            {
                if (!topicsIds.Contains(topic.Id.Value))
                {
                    var topicRemoveResult = question.RemoveTopic(topic);

                    if (topicRemoveResult.IsFailure)
                    {
                        return topicRemoveResult.Error;
                    }

                    var questionRemoveResult = topic.RemoveQuestion(question);

                    if (questionRemoveResult.IsFailure)
                    {
                        return questionRemoveResult.Error;
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
