using MediatR;
using TraffiLearn.Application.Data;
using TraffiLearn.Application.Questions.Commands.RemoveTopicForQuestion;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Topics.Commands.RemoveQuestionForTopic
{
    public sealed class RemoveQuestionForTopicCommandHandler : IRequestHandler<RemoveQuestionForTopicCommand>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveQuestionForTopicCommandHandler(
            IQuestionRepository questionRepository,
            ITopicRepository topicRepository,
            IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _topicRepository = topicRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(RemoveQuestionForTopicCommand request, CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(request.QuestionId.Value);

            if (question is null)
            {
                throw new QuestionNotFoundException(request.QuestionId.Value);
            }

            var topic = await _topicRepository.GetByIdAsync(
                request.TopicId.Value, 
                includeExpression: x => x.Questions);

            if (topic is null)
            {
                throw new TopicNotFoundException(request.TopicId.Value);
            }

            if (!topic.Questions.Any(x => x.Id == question.Id))
            {
                throw new QuestionAlreadyRemovedFromTopicException(
                    topicId: topic.Id,
                    questionId: question.Id);
            }

            topic.Questions.Remove(question);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
