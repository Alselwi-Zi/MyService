using Microsoft.AspNetCore.Authorization;

namespace MyService.Permission
{
    public class PermissionRequirement :IAuthorizationRequirement
    {
        public string Permission { get;private set; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
}
