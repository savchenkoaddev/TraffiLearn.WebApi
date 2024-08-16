using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.Errors;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Aggregates.Topics.Errors;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Topics.AddQuestionToTopic
{
    internal sealed class AddQuestionToTopicCommandHandler : IRequestHandler<AddQuestionToTopicCommand, Result>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddQuestionToTopicCommandHandler(
            IQuestionRepository questionRepository,
            ITopicRepository topicRepository,
            IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _topicRepository = topicRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            AddQuestionToTopicCommand request,
            CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(
                questionId: new QuestionId(request.QuestionId.Value),
                cancellationToken,
                includeExpressions: question => question.Topics);

            if (question is null)
            {
                return QuestionErrors.NotFound;
            }

            var topic = await _topicRepository.GetByIdAsync(
                topicId: new TopicId(request.TopicId.Value),
                cancellationToken,
                includeExpressions: topic => topic.Questions);

            if (topic is null)
            {
                return TopicErrors.NotFound;
            }

            Result questionAddResult = topic.AddQuestion(question);

            if (questionAddResult.IsFailure)
            {
                return questionAddResult.Error;
            }

            Result topicAddResult = question.AddTopic(topic);

            if (topicAddResult.IsFailure)
            {
                return topicAddResult.Error;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
