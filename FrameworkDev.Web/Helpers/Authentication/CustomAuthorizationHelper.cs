using FrameworkDev.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkDev.Web.Helpers.Authentication
{
    public static class CustomAuthorizationHelper
    {
        public static List<string> GetAllPermissionKeys(int userId)
        {
            using (FrameworkDevEntities data = new FrameworkDevEntities())
            {
                User userObj = data.Users.Include("Roles").Include("Roles.RolePermissions").Where(x => x.UserId == userId).FirstOrDefault();

                List<string> pkList = userObj.Permissions.Select(r => r.PermissionKey).ToList();

                List<string> pkListRoles = userObj.Roles.SelectMany(x => x.RolePermissions.Select(r => r.PermissionKey)).ToList();

                pkList.InsertRange(0, pkListRoles);

                return pkList.Distinct().ToList();
            }
        }

        public static bool FullAuthorize(CustomPrincipal user, string users = "", string roles = "", string permissionKey = "")
        {
            if (string.IsNullOrEmpty(users) && string.IsNullOrEmpty(roles) && string.IsNullOrEmpty(permissionKey))
            {
                return user != null;
            }
            else if (!string.IsNullOrEmpty(users) && users == "*")
            {
                return user != null;
            }

            bool userAuthorized = string.Equals(user.Identity.Name, "admin", System.StringComparison.CurrentCultureIgnoreCase);
            if (!userAuthorized)
            {
                if (!string.IsNullOrEmpty(users))
                {
                    userAuthorized = users.Split(',').Any(x => x == user.Identity.Name);
                }
            }

            bool roleAuthorized = user.IsInRole("admin");
            if (!userAuthorized && !roleAuthorized)
            {
                if (!string.IsNullOrEmpty(roles))
                {
                    foreach (string role in roles.Split(',').ToList())
                    {
                        if (user.IsInRole(role))
                        {
                            roleAuthorized = true;
                        }
                    }
                }
            }

            bool permAuthorized = false;
            if (!userAuthorized && !roleAuthorized)
            {
                permAuthorized = user.HasPermission(permissionKey);
            }

            return userAuthorized || roleAuthorized || permAuthorized;
        }
    }
}
