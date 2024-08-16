using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Identity;

namespace TraffiLearn.Application.Mappers.Auth
{
    internal sealed class UserToApplicationUserMapper
        : Mapper<UserId, ApplicationUser>
    {
        public override ApplicationUser Map(UserId source)
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
