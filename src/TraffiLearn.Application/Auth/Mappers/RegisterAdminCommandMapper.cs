using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Auth.Commands.RegisterAdmin;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Auth.Mappers
{
    internal sealed class RegisterAdminCommandMapper
        : Mapper<RegisterAdminCommand, Result<User>>
    {
        public override Result<User> Map(RegisterAdminCommand source)
        {
            UserId userId = new(Guid.NewGuid());

            var emailResult = Email.Create(source.Email);

            if (emailResult.IsFailure)
            {
                return Result.Failure<User>(emailResult.Error);
            }

            var usernameResult = Username.Create(source.Username);

            if (usernameResult.IsFailure)
            {
                return Result.Failure<User>(usernameResult.Error);
            }

            return User.Create(
                userId: userId,
                email: emailResult.Value,
                username: usernameResult.Value,
                role: Role.Admin);
        }
    }
}
