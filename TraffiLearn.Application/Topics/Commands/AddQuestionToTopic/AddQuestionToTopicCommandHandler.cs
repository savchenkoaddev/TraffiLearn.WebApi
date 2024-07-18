using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Topics.Commands.AddQuestionToTopic
{
    public sealed class AddQuestionToTopicCommandHandler : IRequestHandler<AddQuestionToTopicCommand>
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

        public async Task Handle(AddQuestionToTopicCommand request, CancellationToken cancellationToken)
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

            if (topic.Questions.Any(x => x.Id == question.Id))
            {
                throw new QuestionAlreadyInTopicException(
                    questionId: question.Id,
                    topicId: topic.Id);
            }

            topic.Questions.Add(question);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
