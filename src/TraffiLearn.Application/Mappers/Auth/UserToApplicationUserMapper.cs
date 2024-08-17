using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Identity;
using TraffiLearn.Domain.Aggregates.Users;

namespace TraffiLearn.Application.Mappers.Auth
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
