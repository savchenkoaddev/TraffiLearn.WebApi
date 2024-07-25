using MediatR;
using TraffiLearn.Application.Abstractions;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Questions.Commands.AddTopicToQuestion
{
    public sealed class AddTopicToQuestionCommandHandler : IRequestHandler<AddTopicToQuestionCommand>
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

        public async Task Handle(AddTopicToQuestionCommand request, CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetByIdAsync(request.TopicId.Value);

            if (topic is null)
            {
                throw new TopicNotFoundException(request.TopicId.Value);
            }

            var question = await _questionRepository.GetByIdAsync(
                request.QuestionId.Value,
                includeExpression: x => x.Topics);

            if (question is null)
            {
                throw new QuestionNotFoundException(request.QuestionId.Value);
            }

            if (question.Topics.Any(x => x.Id == topic.Id))
            {
                throw new TopicAlreadyInQuestionException(
                    topicId: topic.Id,
                    questionId: question.Id);
            }

            question.Topics.Add(topic);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
