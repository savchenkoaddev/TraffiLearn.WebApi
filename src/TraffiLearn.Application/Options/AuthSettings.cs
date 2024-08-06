using TraffiLearn.Domain.Enums;

namespace TraffiLearn.Application.Options
{
    public sealed class AuthSettings
    {
        public const string SectionName = nameof(AuthSettings);

        public Role MinimumAllowedRoleToCreateAdminAccounts { get; set; } = Role.Owner;

        public Role MinimumAllowedRoleToRemoveAdminAccounts { get; set; } = Role.Owner;
    }
}
