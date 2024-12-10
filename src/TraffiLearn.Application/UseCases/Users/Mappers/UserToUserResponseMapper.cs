using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.Users.DTO;
using TraffiLearn.Domain.Users;

namespace TraffiLearn.Application.UseCases.Users.Mappers
{
    internal sealed class UserToUserResponseMapper
        : Mapper<User, UserResponse>
    {
        public override UserResponse Map(User source)
        {
            return new UserResponse(
                Id: source.Id.Value,
                Email: source.Email.Value,
                Username: source.Username.Value);
        }
    }
}
