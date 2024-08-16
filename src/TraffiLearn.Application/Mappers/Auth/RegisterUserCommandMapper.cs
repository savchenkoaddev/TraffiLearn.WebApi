using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Commands.Auth.RegisterUser;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Mapper.Auth
{
    internal sealed class RegisterUserCommandMapper
        : Mapper<RegisterUserCommand, Result<Domain.Aggregates.Users.UserId>>
    {
        public override Result<Domain.Aggregates.Users.UserId> Map(RegisterUserCommand source)
        {
            Result<Email> emailCreateResult = Email.Create(source.Email);

            if (emailCreateResult.IsFailure)
            {
                return Result.Failure<Domain.Aggregates.Users.UserId>(emailCreateResult.Error);
            }

            Result<Username> usernameCreateResult = Username.Create(source.Username);

            if (usernameCreateResult.IsFailure)
            {
                return Result.Failure<Domain.Aggregates.Users.UserId>(usernameCreateResult.Error);
            }

            Domain.Aggregates.Users.ValueObjects.UserId userId = new(Guid.NewGuid());

            return UserId.Create(
                userId: userId,
                email: emailCreateResult.Value,
                username: usernameCreateResult.Value,
                role: Role.RegularUser);
        }
    }
}
