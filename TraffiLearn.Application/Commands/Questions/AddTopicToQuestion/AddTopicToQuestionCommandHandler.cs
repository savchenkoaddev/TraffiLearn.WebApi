using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Topics;
using TraffiLearn.Domain.Errors.Questions;
using TraffiLearn.Domain.Errors.Topics;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Questions.AddTopicToQuestion
{
    public sealed class AddTopicToQuestionCommandHandler : IRequestHandler<AddTopicToQuestionCommand, Result>
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

        public async Task<Result> Handle(AddTopicToQuestionCommand request, CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetByIdAsync(request.TopicId.Value);

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

            question.AddTopic(topic);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
