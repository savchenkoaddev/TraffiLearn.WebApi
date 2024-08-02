using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Errors.Questions;
using TraffiLearn.Domain.Errors.Topics;
using TraffiLearn.Domain.RepositoryContracts;
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
            var question = await _questionRepository.GetByIdAsync(request.QuestionId.Value);

            if (question is null)
            {
                return QuestionErrors.NotFound;
            }

            var topic = await _topicRepository.GetByIdAsync(
                request.TopicId.Value,
                includeExpression: x => x.Questions);

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
