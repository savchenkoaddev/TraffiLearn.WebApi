using TraffiLearn.Domain.Enums;

namespace TraffiLearn.Application.Options
{
    public sealed class AuthSettings
    {
        public const string SectionName = nameof(AuthSettings);

        public Role MinAllowedRoleToCreateAdminAccounts { get; set; } = Role.Owner;

        public Role MinAllowedRoleToRemoveAdminAccounts { get; set; } = Role.Owner;

        public Role MinAllowedRoleToDowngradeAccounts { get; set; } = Role.Owner;

        public Role MinAllowedRoleToDeleteComments { get; set; } = Role.Admin;

        public Role MinAllowedRoleToGetUserComments { get; set; } = Role.Admin;

        public Role MinAllowedRoleToModifyDomainObjects { get; set; } = Role.Admin;

        public Role MinRoleForDowngrade { get; set; } = Role.Admin;
    }
}