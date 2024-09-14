namespace TraffiLearn.Infrastructure.Authentication
{
    public enum Permission
    {
        AccessSpecificUserData,
        AccessSpecificAdminData,
        AccessData,
        DowngradeAccounts,
        RegisterAdmins,
        RemoveAdmins,
        ModifyData,
        ModifyNonSensitiveData
    }
}
