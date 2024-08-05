using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Errors.Questions;
using TraffiLearn.Domain.Errors.Topics;
using TraffiLearn.Domain.RepositoryContracts;
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
                request.TopicId.Value,
                cancellationToken,
                includeExpressions: topic => topic.Questions);

            if (topic is null)
            {
                return TopicErrors.NotFound;
            }

            var question = await _questionRepository.GetByIdAsync(
                request.QuestionId.Value,
                cancellationToken,
                includeExpressions: question => question.Topics);

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

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
