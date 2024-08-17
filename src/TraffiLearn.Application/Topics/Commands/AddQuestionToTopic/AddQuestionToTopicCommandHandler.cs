using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.Errors;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Aggregates.Topics.Errors;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Topics.Commands.AddQuestionToTopic
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
            var topic = await _topicRepository.GetByIdWithQuestionsIdsAsync(
                topicId: new TopicId(request.TopicId.Value),
                cancellationToken);

            if (topic is null)
            {
                return TopicErrors.NotFound;
            }

            var question = await _questionRepository.GetByIdAsync(
                questionId: new QuestionId(request.QuestionId.Value),
                cancellationToken);

            if (question is null)
            {
                return QuestionErrors.NotFound;
            }

            var questionAddResult = topic.AddQuestion(question.Id);

            if (questionAddResult.IsFailure)
            {
                return questionAddResult.Error;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
