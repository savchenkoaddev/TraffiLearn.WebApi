﻿using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Users.DTO;
using TraffiLearn.Domain.Aggregates.Users;

namespace TraffiLearn.Application.Users.Mappers
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
