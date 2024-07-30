using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Errors.Questions;
using TraffiLearn.Domain.Errors.Topics;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Topics.RemoveQuestionForTopic
{
    internal sealed class RemoveQuestionForTopicCommandHandler : IRequestHandler<RemoveQuestionForTopicCommand, Result>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveQuestionForTopicCommandHandler(
            ITopicRepository topicRepository,
            IQuestionRepository questionRepository,
            IUnitOfWork unitOfWork)
        {
            _topicRepository = topicRepository;
            _questionRepository = questionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            RemoveQuestionForTopicCommand request, 
            CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetByIdAsync(
                request.TopicId.Value,
                includeExpression: x => x.Questions);

            if (topic is null)
            {
                return TopicErrors.NotFound;
            }

            var question = await _questionRepository.GetByIdAsync(
                request.QuestionId.Value,
                includeExpression: x => x.Topics);

            if (question is null)
            {
                return QuestionErrors.NotFound;
            }

            Result removeResult = topic.RemoveQuestion(question);

            if (removeResult.IsFailure)
            {
                return removeResult.Error;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
