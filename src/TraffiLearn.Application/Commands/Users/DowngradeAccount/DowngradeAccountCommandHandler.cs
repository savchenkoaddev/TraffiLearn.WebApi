using MediatR;
using Microsoft.Extensions.Logging;
using System.Transactions;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Exceptions;
using TraffiLearn.Application.Identity;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Enums;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Commands.Users.DowngradeAccount
{
    internal sealed class DowngradeAccountCommandHandler
        : IRequestHandler<DowngradeAccountCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DowngradeAccountCommandHandler> _logger;

        public DowngradeAccountCommandHandler(
            IUserRepository userRepository,
            IIdentityService<ApplicationUser> identityService,
            IUnitOfWork unitOfWork,
            ILogger<DowngradeAccountCommandHandler> logger)
        {
            _userRepository = userRepository;
            _identityService = identityService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(
            DowngradeAccountCommand request,
            CancellationToken cancellationToken)
        {
            UserId affectedUserId = new(request.UserId.Value);

            var affectedUser = await _userRepository.GetByIdAsync(
                userId: affectedUserId,
                cancellationToken);

            if (affectedUser is null)
            {
                return UserErrors.NotFound;
            }

            if (CannotBeDowngraded(affectedUser))
            {
                return UserErrors.AccountCannotBeDowngraded;
            }

            string previousRole = affectedUser.Role.ToString();
            var downgradeResult = affectedUser.DowngradeRole();
            string newRole = affectedUser.Role.ToString();

            if (downgradeResult.IsFailure)
            {
                return downgradeResult.Error;
            }

            var identityUser = await _identityService.GetByEmailAsync(affectedUser.Email);

            if (identityUser is null)
            {
                _logger.LogCritical("User has not been found in repository for email: {Email}. Critical data consistency issue.", affectedUser.Email);

                throw new DataInconsistencyException();
            }

            using (var transaction = new TransactionScope(
                TransactionScopeAsyncFlowOption.Enabled))
            {
                await _identityService.RemoveFromRoleAsync(
                    identityUser,
                    previousRole);

                await _identityService.AddToRoleAsync(
                    identityUser,
                    newRole);

                await _userRepository.UpdateAsync(affectedUser);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                transaction.Complete();
            }

            _logger.LogInformation(
                    "Successfully downgraded an account with email: {Email}. " +
                    "Previous role: {PreviousRole}. " +
                    "New role: {NewRole}",
                    affectedUser.Email.Value,
                    previousRole,
                    affectedUser.Role);

            return Result.Success();
        }

        private bool CannotBeDowngraded(User user)
        {
            return user.Role == Role.RegularUser;
        }
    }
}
