﻿using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.UseCases.Users.Identity;
using TraffiLearn.Domain.Users;
using TraffiLearn.Domain.Users.Emails;
using TraffiLearn.Domain.Users.Roles;
using TraffiLearn.Domain.Users.Usernames;
using TraffiLearn.WebAPI.Options;

namespace TraffiLearn.WebAPI.Extensions
{
    public static class UserExtensions
    {
        public async static Task SeedSuperUserIfNotSeeded(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var superUserSettings = scope.ServiceProvider.GetRequiredService<IOptions<SuperUserSettings>>()
                .Value;

            var superUser = User.Create(new UserId(Guid.NewGuid()),
                email: Email.Create(superUserSettings.Email).Value,
                username: Username.Create(superUserSettings.Username).Value,
                role: Role.Owner).Value;

            var existingUser = await userRepository.GetByUsernameAsync(
                superUser.Username);

            if (existingUser is not null)
            {
                if (!existingUser.IsEmailConfirmed)
                {
                    existingUser.ConfirmEmail();

                    await unitOfWork.SaveChangesAsync();
                }

                return;
            }

            superUser.ConfirmEmail();

            await userRepository.InsertAsync(superUser);

            await unitOfWork.SaveChangesAsync();

            var identityService = scope.ServiceProvider.GetRequiredService<IIdentityService<ApplicationUser>>();

            var userMapper = scope.ServiceProvider.GetRequiredService<Mapper<User, ApplicationUser>>();

            var identityUser = userMapper.Map(superUser);

            identityUser.EmailConfirmed = true;

            await identityService.CreateAsync(
                identityUser,
                password: superUserSettings.Password);

            await identityService.AddToRoleAsync(
                identityUser,
                roleName: Role.Owner.ToString());
        }
    }
}
