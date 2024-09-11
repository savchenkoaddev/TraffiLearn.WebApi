using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Users.Identity;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.WebAPI.Options;

namespace TraffiLearn.WebAPI.Extensions
{
    public static class UserExtensions
    {
        public async static Task SeedSuperUserIfNotSeeded(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var superUserSettings = scope.ServiceProvider.GetRequiredService<IOptions<SuperUserSettings>>()
                .Value;

            var superUser = User.Create(new UserId(Guid.NewGuid()),
                email: Email.Create(superUserSettings.Email).Value,
                username: Username.Create(superUserSettings.Username).Value,
                role: Role.Owner).Value;

            if (await userRepository.ExistsAsync(
                    superUser.Username,
                    superUser.Email))
            {
                return;
            }

            await userRepository.AddAsync(superUser);

            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            await unitOfWork.SaveChangesAsync();

            var identityService = scope.ServiceProvider.GetRequiredService<IIdentityService<ApplicationUser>>();

            var userMapper = scope.ServiceProvider.GetRequiredService<Mapper<User, ApplicationUser>>();

            var identityUser = userMapper.Map(superUser);

            await identityService.CreateAsync(
                identityUser,
                password: superUserSettings.Password);

            await identityService.AddToRoleAsync(
                identityUser,
                roleName: Role.Owner.ToString());
        }
    }
}
