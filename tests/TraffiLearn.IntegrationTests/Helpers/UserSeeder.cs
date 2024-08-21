using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Users.Identity;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.IntegrationTests.Auth;

namespace TraffiLearn.IntegrationTests.Helpers
{
    internal sealed class UserSeeder
    {
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserSeeder(
            IIdentityService<ApplicationUser> identityService,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _identityService = identityService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Seed()
        {
            await SeedOwnerWithTestCredentialsAsync();
            await SeedAdminWithTestCredentialsAsync();
            await SeedRegularUserWithTestCredentialsAsync();
        }

        private Task SeedOwnerWithTestCredentialsAsync()
        {
            return SeedUserAsync(
                username: AuthConstants.OwnerUsername,
                email: AuthConstants.OwnerEmail,
                Role.Owner);
        }

        private Task SeedAdminWithTestCredentialsAsync()
        {
            return SeedUserAsync(
                username: AuthConstants.AdminUsername,
                email: AuthConstants.AdminEmail,
                Role.Admin);
        }

        private Task SeedRegularUserWithTestCredentialsAsync()
        {
            return SeedUserAsync(
                username: AuthConstants.Username,
                email: AuthConstants.Email,
                Role.RegularUser);
        }

        private async Task SeedUserAsync(
            string username,
            string email,
            Role role)
        {
            var usernameObject = Username.Create(username).Value;
            var emailObject = Email.Create(email).Value;

            var userExists = await _userRepository.ExistsAsync(
                username: usernameObject,
                email: emailObject);

            if (!userExists)
            {
                var id = new UserId(Guid.NewGuid());

                var user = User.Create(
                    id,
                    emailObject,
                    usernameObject,
                    role).Value;

                await _userRepository.AddAsync(user);

                var identityUser = new ApplicationUser
                {
                    UserName = username,
                    Email = email,
                    Id = id.Value.ToString()
                };

                await _identityService.CreateAsync(
                    identityUser,
                    password: AuthConstants.Password);

                await _identityService.AddToRoleAsync(
                    identityUser,
                    roleName: role.ToString());

                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
