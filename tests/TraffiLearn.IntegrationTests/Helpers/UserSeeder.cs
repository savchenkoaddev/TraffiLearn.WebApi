using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.UseCases.Users.Identity;
using TraffiLearn.Domain.Users;
using TraffiLearn.Domain.Users.Emails;
using TraffiLearn.Domain.Users.Roles;
using TraffiLearn.Domain.Users.Usernames;
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
                AuthTestCredentials.Owner,
                Role.Owner);
        }

        private Task SeedAdminWithTestCredentialsAsync()
        {
            return SeedUserAsync(
                AuthTestCredentials.Admin,
                Role.Admin);
        }

        private Task SeedRegularUserWithTestCredentialsAsync()
        {
            return SeedUserAsync(
                AuthTestCredentials.RegularUser,
                Role.RegularUser);
        }

        private async Task SeedUserAsync(
            RoleCredentials roleCredentials,
            Role role)
        {
            var username = roleCredentials.Username;
            var email = roleCredentials.Email;
            var password = roleCredentials.Password;

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

                user.ConfirmEmail();

                await _userRepository.InsertAsync(user);

                var identityUser = new ApplicationUser
                {
                    UserName = username,
                    Email = email,
                    Id = id.Value.ToString(),
                    EmailConfirmed = true
                };

                await _identityService.CreateAsync(
                    identityUser,
                    password: password);

                await _identityService.AddToRoleAsync(
                    identityUser,
                    roleName: role.ToString());

                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
