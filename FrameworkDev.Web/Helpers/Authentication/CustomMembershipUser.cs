
using FrameworkDev.Web.Models; using FrameworkDev.Web.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;

namespace FrameworkDev.Web.Helpers.Authentication
{
    public class CustomMembershipUser : MembershipUser
    {
        #region User Properties

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<Role> Roles { get; set; }

        public ICollection<VM_Permission> Permissions { get; set; }

        #endregion User Properties

        public CustomMembershipUser(User user) : base("CustomMembership", user.Username, user.UserId, user.Email, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            List<VM_Permission> sysPermissions = CustomPermissionKeyHelper.GetAllPermissionKeys();
            UserId = user.UserId;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Roles = user.Roles;
            Permissions = user.Permissions.Select(x =>
            new VM_Permission()
            {
                PermissionKey = x.PermissionKey,
                PermissionName = sysPermissions.FirstOrDefault(p => p.PermissionKey == x.PermissionKey)?.PermissionName
            }).ToList();
        }
    }
}
