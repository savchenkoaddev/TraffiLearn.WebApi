using Microsoft.AspNetCore.Authorization;
using TraffiLearn.Domain.Users.Roles;
using TraffiLearn.Infrastructure.Authentication;

namespace TraffiLearn.WebAPI.Extensions.DI
{
    internal static class AuthorizationExtensions
    {
        public static IServiceCollection ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.ConfigurePolicy(
                    name: Permission.AuthenticatedUser.ToString(),
                    roles: [
                        Role.RegularUser.ToString(),
                        Role.Admin.ToString(),
                        Role.Owner.ToString(),
                    ]);

                options.ConfigurePolicy(
                    name: Permission.ViewUsersData.ToString(),
                    roles: [
                        Role.Admin.ToString(),
                        Role.Owner.ToString()
                    ]);

                options.ConfigurePolicy(
                    name: Permission.ManageAccountStatuses.ToString(),
                    roles: Role.Owner.ToString());

                options.ConfigurePolicy(
                    name: Permission.ModifyApplicationData.ToString(),
                    roles: [
                        Role.Admin.ToString(),
                        Role.Owner.ToString()
                    ]);

                options.ConfigurePolicy(
                    name: Permission.ManageAdmins.ToString(),
                    roles: Role.Owner.ToString());

                options.ConfigurePolicy(
                    name: Permission.ViewAdminsData.ToString(),
                    roles: Role.Owner.ToString());

                options.ConfigurePolicy(
                    name: Permission.ManageSubscriptionPlans.ToString(),
                    roles: Role.Owner.ToString());
            });

            return services;
        }

        private static AuthorizationOptions ConfigurePolicy(
            this AuthorizationOptions options,
            string name,
            params string[] roles)
        {
            options.AddPolicy(name,
                options =>
                {
                    options.RequireRole(roles);
                });

            return options;
        }
    }
}
