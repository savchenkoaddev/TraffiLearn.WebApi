using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Commands.Auth.RegisterAdmin;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Enums;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Mappers.Auth
{
    internal sealed class RegisterAdminCommandMapper
        : Mapper<RegisterAdminCommand, Result<User>>
    {
        public override Result<User> Map(RegisterAdminCommand source)
        {
            var userId = Guid.NewGuid();

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
                id: userId,
                email: emailResult.Value,
                username: usernameResult.Value,
                role: Role.Admin);
        }
    }
}
