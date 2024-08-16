﻿using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Commands.Auth.RegisterAdmin;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Mappers.Auth
{
    internal sealed class RegisterAdminCommandMapper
        : Mapper<RegisterAdminCommand, Result<Domain.Aggregates.Users.UserId>>
    {
        public override Result<Domain.Aggregates.Users.UserId> Map(RegisterAdminCommand source)
        {
            Domain.Aggregates.Users.ValueObjects.UserId userId = new(Guid.NewGuid());

            var emailResult = Email.Create(source.Email);

            if (emailResult.IsFailure)
            {
                return Result.Failure<Domain.Aggregates.Users.UserId>(emailResult.Error);
            }

            var usernameResult = Username.Create(source.Username);

            if (usernameResult.IsFailure)
            {
                return Result.Failure<Domain.Aggregates.Users.UserId>(usernameResult.Error);
            }

            return UserId.Create(
                userId: userId,
                email: emailResult.Value,
                username: usernameResult.Value,
                role: Role.Admin);
        }
    }
}
