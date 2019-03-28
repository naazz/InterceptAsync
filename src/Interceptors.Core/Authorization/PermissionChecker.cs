using Abp.Authorization;
using Interceptors.Authorization.Roles;
using Interceptors.Authorization.Users;

namespace Interceptors.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
