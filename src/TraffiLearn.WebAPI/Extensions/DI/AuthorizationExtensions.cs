using Microsoft.AspNetCore.Authorization;
using TraffiLearn.Domain.Aggregates.Users.Enums;
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
                    name: Permission.AccessSpecificUserData.ToString(),
                    roles: [
                        Role.Admin.ToString(),
                        Role.Owner.ToString()
                    ]);

                options.ConfigurePolicy(
                    name: Permission.AccessData.ToString(),
                    roles: [
                        Role.RegularUser.ToString(),
                        Role.Admin.ToString(),
                        Role.Owner.ToString(),
                    ]);

                options.ConfigurePolicy(
                    name: Permission.ModifyNonSensitiveData.ToString(),
                    roles: [
                        Role.RegularUser.ToString(),
                        Role.Admin.ToString(),
                        Role.Owner.ToString(),
                    ]);

                options.ConfigurePolicy(
                    name: Permission.DowngradeAccounts.ToString(),
                    roles: Role.Owner.ToString());

                options.ConfigurePolicy(
                    name: Permission.RegisterAdmins.ToString(),
                    roles: Role.Owner.ToString());

                options.ConfigurePolicy(
                    name: Permission.RemoveAdmins.ToString(),
                    roles: Role.Owner.ToString());

                options.ConfigurePolicy(
                    name: Permission.ModifyData.ToString(),
                    roles: [
                        Role.Admin.ToString(),
                        Role.Owner.ToString()
                    ]);

                options.ConfigurePolicy(
                    name: Permission.AccessSpecificAdminData.ToString(),
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
