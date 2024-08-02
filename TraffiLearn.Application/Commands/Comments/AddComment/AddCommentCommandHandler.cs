using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Identity;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Comments;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Comments;

namespace TraffiLearn.Application.Commands.Comments.AddComment
{
    internal sealed class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, Result>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AddCommentCommandHandler> _logger;

        public AddCommentCommandHandler(
            ICommentRepository commentRepository,
            IUserRepository userRepository,
            IQuestionRepository questionRepository,
            SignInManager<ApplicationUser> signInManager,
            IUnitOfWork unitOfWork,
            ILogger<AddCommentCommandHandler> logger)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _questionRepository = questionRepository;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(
            AddCommentCommand request, 
            CancellationToken cancellationToken)
        {
            var userAuthenticated = _signInManager.Context.User.Identity.IsAuthenticated;

            if (!userAuthenticated)
            {
                _logger.LogWarning("The user is not authenticated. This is probably due to some authorization failures.");

                return Error.InternalFailure();
            }

            var email = _signInManager.Context.User.FindFirst(ClaimTypes.Email).Value;

            if (email is null)
            {
                _logger.LogWarning("Couldn't fetch the email from http context. This is probably due to the token generation issues.");

                return Error.InternalFailure();
            }

            var user = await _userRepository.GetByEmailAsync(email);

            if (user is null)
            {
                _logger.LogError("User with the context provided email has not been found.");

                return Error.InternalFailure();
            }

            var question = await _questionRepository.GetByIdAsync(request.QuestionId.Value);

            if (question is null)
            {
                return CommentErrors.QuestionNotFound;
            }

            var contentResult = CommentContent.Create(request.Content);

            if (contentResult.IsFailure)
            {
                return contentResult.Error;
            }

            var commentId = Guid.NewGuid();
            
            var commentResult = Comment.Create(
                commentId,
                contentResult.Value,
                leftBy: user,
                question);

            if (commentResult.IsFailure)
            {
                return commentResult.Error;
            }

            var comment = commentResult.Value;

            var userAddCommentResult = user.AddComment(comment);

            if (userAddCommentResult.IsFailure)
            {
                return userAddCommentResult.Error;
            }

            var questionAddCommentResult = question.AddComment(comment);

            if (questionAddCommentResult.IsFailure)
            {
                return questionAddCommentResult.Error;
            }

            await _commentRepository.AddAsync(comment);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Added comment succesfully.");

            return Result.Success();
        }
    }
}
