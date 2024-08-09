using MediatR;
using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Options;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Questions;
using TraffiLearn.Domain.Errors.Topics;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Questions;
using TraffiLearn.Domain.ValueObjects.Topics;

namespace TraffiLearn.Application.Commands.Questions.AddTopicToQuestion
{
    internal sealed class AddTopicToQuestionCommandHandler : IRequestHandler<AddTopicToQuestionCommand, Result>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IUserManagementService _userManagementService;
        private readonly IUnitOfWork _unitOfWork;

        public AddTopicToQuestionCommandHandler(
            IQuestionRepository questionRepository,
            ITopicRepository topicRepository,
            IUserManagementService userManagementService,
            IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _topicRepository = topicRepository;
            _userManagementService = userManagementService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            AddTopicToQuestionCommand request, 
            CancellationToken cancellationToken)
        {
            var authorizationResult = await _userManagementService.EnsureCallerCanModifyDomainObjects(
                cancellationToken);

            if (authorizationResult.IsFailure)
            {
                return authorizationResult.Error;
            }

            var topic = await _topicRepository.GetByIdAsync(
                topicId: new TopicId(request.TopicId.Value), 
                cancellationToken,
                includeExpressions: topic => topic.Questions);

            if (topic is null)
            {
                return TopicErrors.NotFound;
            }

            var question = await _questionRepository.GetByIdAsync(
                questionId: new QuestionId(request.QuestionId.Value),
                cancellationToken,
                includeExpressions: question => question.Topics);

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

            await _questionRepository.UpdateAsync(question);
            await _topicRepository.UpdateAsync(topic);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
