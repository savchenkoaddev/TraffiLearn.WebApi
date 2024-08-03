using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Errors.Questions;
using TraffiLearn.Domain.Errors.Topics;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Questions.AddTopicToQuestion
{
    internal sealed class AddTopicToQuestionCommandHandler : IRequestHandler<AddTopicToQuestionCommand, Result>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddTopicToQuestionCommandHandler(
            IQuestionRepository questionRepository,
            ITopicRepository topicRepository,
            IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _topicRepository = topicRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            AddTopicToQuestionCommand request, 
            CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetByIdWithQuestionsAsync(request.TopicId.Value);

            if (topic is null)
            {
                return TopicErrors.NotFound;
            }

            var question = await _questionRepository.GetByIdWithTopicsAsync(
                request.QuestionId.Value);

            if (question is null)
            {
                return QuestionErrors.NotFound;
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

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
