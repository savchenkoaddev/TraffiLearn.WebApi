using Microsoft.AspNetCore.Authorization;

namespace TraffiLearn.Infrastructure.Authentication
{
    public sealed class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(Permission permission)
            : base(permission.ToString())
        { }
    }
}
