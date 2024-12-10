using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.Users.Identity;
using TraffiLearn.Domain.Users;

namespace TraffiLearn.Application.UseCases.Auth.Mappers
{
    internal sealed class UserToApplicationUserMapper
        : Mapper<User, ApplicationUser>
    {
        public override ApplicationUser Map(User source)
        {
            return new ApplicationUser()
            {
                Id = source.Id.Value.ToString(),
                Email = source.Email.Value,
                UserName = source.Username.Value
            };
        }
    }
}
