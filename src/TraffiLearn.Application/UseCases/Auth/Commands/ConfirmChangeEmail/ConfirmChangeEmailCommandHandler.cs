﻿using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Exceptions;
using TraffiLearn.Application.UseCases.Users.Identity;
using TraffiLearn.Domain.Users;
using TraffiLearn.Domain.Users.Emails;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Auth.Commands.ConfirmChangeEmail
{
    internal sealed class ConfirmChangeEmailCommandHandler
        : IRequestHandler<ConfirmChangeEmailCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmChangeEmailCommandHandler(
            IUserRepository userRepository,
            IIdentityService<ApplicationUser> identityService,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _identityService = identityService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            ConfirmChangeEmailCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(
                new UserId(request.UserId),
                cancellationToken);

            if (user is null)
            {
                return UserErrors.NotFound;
            }

            var email = user.Email;

            var identityUser = await _identityService.GetByEmailAsync(
                email);

            if (identityUser is null)
            {
                throw new DataInconsistencyException();
            }

            var result = await _identityService.ChangeEmailAsync(
                identityUser,
                newEmail: request.NewEmail,
                token: request.Token);

            if (result.IsFailure)
            {
                return result.Error;
            }

            var newEmailResult = Email.Create(request.NewEmail);

            if (newEmailResult.IsFailure)
            {
                return newEmailResult.Error;
            }

            var newEmail = newEmailResult.Value;

            user.ChangeEmail(newEmail);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
