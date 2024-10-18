﻿using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Exceptions;
using TraffiLearn.Application.Users.Identity;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.Domain.Aggregates.Users.Errors;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Auth.Commands.RegisterAdmin
{
    internal sealed class RegisterAdminCommandHandler
        : IRequestHandler<RegisterAdminCommand, Result>
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IUserRepository _userRepository;
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly Mapper<RegisterAdminCommand, Result<User>> _commandMapper;
        private readonly Mapper<User, ApplicationUser> _userMapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegisterAdminCommandHandler> _logger;

        public RegisterAdminCommandHandler(
            IAuthenticatedUserService authenticatedUserService,
            IUserRepository userRepository,
            IIdentityService<ApplicationUser> identityService,
            Mapper<RegisterAdminCommand, Result<User>> commandMapper,
            Mapper<User, ApplicationUser> userMapper,
            IUnitOfWork unitOfWork,
            ILogger<RegisterAdminCommandHandler> logger)
        {
            _authenticatedUserService = authenticatedUserService;
            _userRepository = userRepository;
            _identityService = identityService;
            _commandMapper = commandMapper;
            _userMapper = userMapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(
            RegisterAdminCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling RegisterAdminCommand");

            var mappingResult = _commandMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return mappingResult.Error;
            }

            var caller = await _authenticatedUserService
                .GetAuthenticatedUserAsync(cancellationToken);

            if (IsNotAllowedToCreateAdmins(caller))
            {
                _logger.LogCritical("Caller tried to create an admin account not having enough permissions. Caller role: {role}", caller.Role.ToString());

                throw new AuthorizationFailureException();
            }

            var newUser = mappingResult.Value;

            var existsSameUser = await _userRepository.ExistsAsync(
                newUser.Username,
                newUser.Email,
                cancellationToken);

            if (existsSameUser)
            {
                return UserErrors.AlreadyRegistered;
            }

            var identityUser = _userMapper.Map(newUser);

            Func<Task> transactionAction = async () =>
            {
                await _userRepository.InsertAsync(
                    newUser,
                    cancellationToken);

                await _identityService.CreateAsync(
                    identityUser,
                    password: request.Password);

                await _identityService.AddToRoleAsync(
                    identityUser,
                    roleName: newUser.Role.ToString());

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            };

            await _unitOfWork.ExecuteInTransactionAsync(
                transactionAction,
                cancellationToken: cancellationToken);

            _logger.LogInformation(
                "Succesfully registered a new admin. Username: {username}",
                newUser.Username.Value);

            return Result.Success();
        }

        private bool IsNotAllowedToCreateAdmins(User user)
        {
            return user.Role < Role.Owner;
        }
    }
}
