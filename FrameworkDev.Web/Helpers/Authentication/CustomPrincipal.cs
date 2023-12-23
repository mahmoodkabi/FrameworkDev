using System.Linq;
using System.Security.Principal;

namespace FrameworkDev.Web.Helpers.Authentication
{
    public class CustomPrincipal : IPrincipal
    {
        #region Identity Properties

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string[] Roles { get; set; }

        public string[] PermissionKeys { get; set; }

        #endregion Identity Properties

        public IIdentity Identity
        {
            get; private set;
        }

        public bool IsInRole(string role)
        {
            if (Roles.Any(r => role.Contains(r)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HasPermission(string permissionKey)
        {
            if (PermissionKeys != null && PermissionKeys.Any(pk => pk == permissionKey))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HasSubSystemPermission(string ssName)
        {
            bool result = false;

            if (Identity.Name == "admin")
            {
                result = true;
            }
            else if (IsInRole("admin"))
            {
                result = true;
            }
            else
            {
                result = PermissionKeys != null && PermissionKeys.Any(pk => pk.StartsWith(ssName));
            }
            return result;
        }


        public bool HasPermissionUI(string permissionKey)
        {
            bool result = false;
            if (Identity.Name == "admin")
            {
                result = true;
            }
            else if (IsInRole("admin"))
            {
                result = true;
            }
            else { result = HasPermission(permissionKey); }
            return result;
        }

        public string HasPermissionUIStr(string permissionKey)
        {
            return "Get" + HasPermissionUI(permissionKey).ToString();
        }

        public CustomPrincipal()
        {
        }

        public CustomPrincipal(string username)
        {
            Identity = new GenericIdentity(username);
        }
    }
}
