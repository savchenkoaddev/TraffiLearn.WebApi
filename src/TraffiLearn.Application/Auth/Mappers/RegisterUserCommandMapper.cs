using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Auth.Commands.RegisterUser;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.Emails;
using TraffiLearn.Domain.Aggregates.Users.Roles;
using TraffiLearn.Domain.Aggregates.Users.Usernames;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Auth.Mappers
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

            UserId userId = new(Guid.NewGuid());

            return User.Create(
                userId: userId,
                email: emailCreateResult.Value,
                username: usernameCreateResult.Value,
                role: Role.RegularUser);
        }
    }
}
