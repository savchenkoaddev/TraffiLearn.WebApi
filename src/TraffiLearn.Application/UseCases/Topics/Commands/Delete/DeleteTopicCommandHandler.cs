﻿using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Domain.Topics;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Topics.Commands.Delete
{
    internal sealed class DeleteTopicCommandHandler : IRequestHandler<DeleteTopicCommand, Result>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IImageService _imageService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTopicCommandHandler(
            ITopicRepository topicRepository,
            IImageService imageService,
            IUnitOfWork unitOfWork)
        {
            _topicRepository = topicRepository;
            _imageService = imageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            DeleteTopicCommand request,
            CancellationToken cancellationToken)
        {
            var found = await _topicRepository.GetByIdAsync(
                topicId: new TopicId(request.TopicId),
                cancellationToken);

            if (found is null)
            {
                return TopicErrors.NotFound;
            }

            Func<Task> transactionAction = async () =>
            {
                await _topicRepository.DeleteAsync(found);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                if (found.ImageUri is not null)
                {
                    await _imageService.DeleteAsync(
                        found.ImageUri, cancellationToken);
                }
            };

            await _unitOfWork.ExecuteInTransactionAsync(
                transactionAction,
                cancellationToken: cancellationToken);

            return Result.Success();
        }
    }
}
