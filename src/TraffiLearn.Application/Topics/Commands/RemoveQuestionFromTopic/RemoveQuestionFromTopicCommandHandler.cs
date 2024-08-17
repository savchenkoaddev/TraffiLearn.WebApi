using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Tickets.Errors;
using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Topics.Commands.RemoveQuestionFromTopic
{
    internal sealed class RemoveQuestionFromTopicCommandHandler
        : IRequestHandler<RemoveQuestionFromTopicCommand, Result>
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
            var topic = await _topicRepository.GetByIdWithQuestionsIdsAsync(
                topicId: new TopicId(request.TopicId.Value),
                cancellationToken);

            if (topic is null)
            {
                return TicketErrors.NotFound;
            }

            var question = await _questionRepository.GetByIdAsync(
                questionId: new QuestionId(request.QuestionId.Value),
                cancellationToken);

            if (question is null)
            {
                return TicketErrors.QuestionNotFound;
            }

            var questionRemoveResult = topic.RemoveQuestion(question.Id);

            if (questionRemoveResult.IsFailure)
            {
                return questionRemoveResult.Error;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
