using FrameworkDev.Web.Models; using FrameworkDev.Web.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FrameworkDev.Web.Helpers.Authentication
{
    public static class CustomPermissionKeyHelper
    {
        public static List<VM_PermissionTree> GetAllPermissionKeysTree()
        {
            List<VM_PermissionTree> result = new List<VM_PermissionTree>();

            List<VM_Permission> ops = CustomPermissionKeyHelper.GetAllPermissionKeys();

            foreach (VM_Permission pk in ops)
            {
                string pkpk = pk.PermissionKey;

                int lio = pkpk.LastIndexOf(':');

                int len = pkpk.Length;

                string parentKey = "";

                if (lio > 0)
                {
                    parentKey = pkpk.Remove(lio, len - lio);
                }

                bool hasChildren = ops.Count(x => x.PermissionKey.StartsWith(pkpk + ":")) > 0;

                VM_PermissionTree newPK = new VM_PermissionTree() { PermissionKey = pkpk, PermissionName = pk.PermissionName, ParentKey = parentKey, HasChildren = hasChildren };

                if (!result.Contains(newPK))
                {
                    result.Add(newPK);
                }
            }

            return result;
        }

        public static List<VM_Permission> GetAllPermissionKeys()
        {
            List<VM_Permission> permissions = new List<VM_Permission>();
            permissions = permissions.Concat(GetControllersPKList()).ToList();
            permissions = permissions.Concat(GetApiControllersPKList()).ToList();
            permissions = permissions.Where(x => x.PermissionKey != null).ToList();

            List<VM_Permission> permissionsFinal = new List<VM_Permission>();

            foreach (VM_Permission p in permissions)
            {
                if (!permissionsFinal.Any(x => x.PermissionKey == p.PermissionKey))
                {
                    permissionsFinal.Add(p);
                }
            }

            return permissionsFinal;
        }

        public static List<VM_Permission> GetControllersPKList()
        {
            List<VM_Permission> permissions = new List<VM_Permission>();

            try
            {
                Assembly asm = Assembly.GetExecutingAssembly();

                IEnumerable<Type> controllers = asm.GetTypes().Where(type => (typeof(System.Web.Mvc.Controller).IsAssignableFrom(type) && type.CustomAttributes.Any(attr => attr.AttributeType == typeof(CustomAuthorizeAttribute)))).Select(x => x);

                foreach (Type controller in controllers)
                {
                    foreach (CustomAttributeData attr in controller.CustomAttributes)
                    {
                        permissions.Add(GetPermission(attr));
                    }

                    IEnumerable<MethodInfo> methods = controller.GetMethods().Where(method => method.IsPublic && method.CustomAttributes.Any() &&
                      (
                          method.CustomAttributes.Any(attr => attr.AttributeType == typeof(CustomAuthorizeAttribute)))
                      );

                    foreach (MethodInfo method in methods)
                    {
                        foreach (CustomAttributeData attr in method.CustomAttributes)
                        {
                            permissions.Add(GetPermission(attr));
                        }
                    }
                }
            }
            catch { }

            return permissions;
        }

        public static List<VM_Permission> GetApiControllersPKList()
        {
            List<VM_Permission> permissions = new List<VM_Permission>();

            try
            {
                Assembly asm = Assembly.GetExecutingAssembly();

                IEnumerable<Type> controllers = asm.GetTypes().Where(type => (type.CustomAttributes.Any(attr => attr.AttributeType == typeof(CustomAuthorizeAttribute)))).Select(x => x);

                foreach (Type controller in controllers)
                {
                    foreach (CustomAttributeData attr in controller.CustomAttributes)
                    {
                        permissions.Add(GetPermission(attr));
                    }

                    IEnumerable<MethodInfo> methods = controller.GetMethods().Where(method => method.IsPublic && method.CustomAttributes.Any() &&
                      (
                          method.CustomAttributes.Any(attr => attr.AttributeType == typeof(CustomAuthorizeAttribute)))
                      );

                    foreach (MethodInfo method in methods)
                    {
                        foreach (CustomAttributeData attr in method.CustomAttributes)
                        {
                            permissions.Add(GetPermission(attr));
                        }
                    }
                }
            }
            catch { }

            return permissions;
        }

        public static VM_Permission GetPermission(CustomAttributeData attr)
        {
            VM_Permission permission = new VM_Permission();

            try
            {
                CustomAttributeNamedArgument pkArgs = attr.NamedArguments.FirstOrDefault(x => x.MemberName == "PermissionKey");
                if (pkArgs != null && pkArgs.MemberInfo != null)
                {
                    permission.PermissionKey = pkArgs.TypedValue.Value.ToString();
                }
            }
            catch { }
            try
            {
                CustomAttributeNamedArgument pnArgs = attr.NamedArguments.FirstOrDefault(x => x.MemberName == "PermissionName");
                if (pnArgs != null && pnArgs.MemberInfo != null)
                {
                    permission.PermissionName = pnArgs.TypedValue.Value.ToString();
                }
            }
            catch { }

            return permission;
        }
    }
}
