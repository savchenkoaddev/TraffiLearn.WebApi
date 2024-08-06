using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Commands.Auth.RegisterUser;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Enums;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Mapper.Auth
{
    internal sealed class RegisterUserCommandMapper
        : Mapper<RegisterUserCommand, Result<User>>
    {
        public override Result<User> Map(RegisterUserCommand source)
        {
            Result<Email> emailCreateResult = Email.Create(source.Email);

            if (emailCreateResult.IsFailure)
            {
                return Result.Failure<User>(emailCreateResult.Error);
            }

            Result<Username> usernameCreateResult = Username.Create(source.Username);

            if (usernameCreateResult.IsFailure)
            {
                return Result.Failure<User>(usernameCreateResult.Error);
            }

            var userId = Guid.NewGuid();

            return User.Create(
                id: userId,
                email: emailCreateResult.Value,
                username: usernameCreateResult.Value,
                role: Role.RegularUser);
        }
    }
}
