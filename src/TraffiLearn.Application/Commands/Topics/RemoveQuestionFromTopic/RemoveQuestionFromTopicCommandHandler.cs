﻿using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.Errors;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Aggregates.Topics.Errors;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Topics.RemoveQuestionFromTopic
{
    internal sealed class RemoveQuestionFromTopicCommandHandler : IRequestHandler<RemoveQuestionFromTopicCommand, Result>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveQuestionFromTopicCommandHandler(
            ITopicRepository topicRepository,
            IQuestionRepository questionRepository,
            IUnitOfWork unitOfWork)
        {
            _topicRepository = topicRepository;
            _questionRepository = questionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            RemoveQuestionFromTopicCommand request,
            CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetByIdAsync(
                topicId: new TopicId(request.TopicId.Value),
                cancellationToken,
                includeExpressions: topic => topic.Questions);

            if (topic is null)
            {
                return TopicErrors.NotFound;
            }

            var question = await _questionRepository.GetByIdAsync(
                questionId: new QuestionId(request.QuestionId.Value),
                cancellationToken,
                includeExpressions: question => question.TopicIds);

            if (question is null)
            {
                return QuestionErrors.NotFound;
            }

            Result questionRemoveResult = topic.RemoveQuestion(question);

            if (questionRemoveResult.IsFailure)
            {
                return questionRemoveResult.Error;
            }

            Result topicRemoveResult = question.RemoveTopic(topic);

            if (topicRemoveResult.IsFailure)
            {
                return topicRemoveResult.Error;
            }

            await _questionRepository.UpdateAsync(question);
            await _topicRepository.UpdateAsync(topic);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
