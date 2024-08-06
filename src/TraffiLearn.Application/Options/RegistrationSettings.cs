using TraffiLearn.Domain.Enums;

namespace TraffiLearn.Application.Options
{
    public sealed class RegistrationSettings
    {
        public const string SectionName = nameof(RegistrationSettings);

        public Role MinimumAllowedRoleToCreateAdminAccounts { get; set; } = Role.Owner;
    }
}
