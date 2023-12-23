using AutoMapper;


using FrameworkDev.Web.Areas.Management.Models;
using FrameworkDev.Web.Helpers;
using FrameworkDev.Web.Models;

using System.Linq;

namespace FrameworkDev.Web.Areas.Management.Repos
{
    public class RolesRepository : CustomRepository<VM_Role, int>
    {
        public override VM_Role Delete(int id)
        {
            Role entity = context.Roles.FirstOrDefault(p => p.RoleId == id);
            context.Roles.Remove(entity);
            Save();
            return Mapper.Map<Role, VM_Role>(entity);
        }

        public override VM_Role GetByID(int id)
        {
            Role entity = context.Roles.Find(id);

            VM_Role vm = new VM_Role()
            {
                RoleId = entity.RoleId,
                RoleName = entity.RoleName,
                RoleNameFa = entity.RoleNameFa,
            };

            return vm;
        }

        public override IQueryable<VM_Role> GetList()
        {
            IQueryable<VM_Role> roles = context.Roles.AsNoTracking().Select(role => new VM_Role
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                RoleNameFa = role.RoleNameFa,
            });

            return roles;
        }

        public override VM_Role Insert(VM_Role vm)
        {
            Role entity = new Role
            {
                RoleId = vm.RoleId,
                RoleName = vm.RoleName,
                RoleNameFa = vm.RoleNameFa,
            };

            context.Roles.Add(entity);
            Save();

            VM_Role vmResult = Mapper.Map<Role, VM_Role>(entity);

            return vmResult;
        }

        public IQueryable<string> GetPermissionsList(int roleId)
        {
            return context.RolePermissions.Where(x => x.RoleId == roleId).Select(x => x.PermissionKey);
        }

        public override IQueryable<VM_Lookup> GetLookupList()
        {
            IQueryable<VM_Lookup> result =
                            context.Roles
                            .Select(x => new VM_Lookup { Value = x.RoleId, Text = x.RoleNameFa })
                            .AsQueryable();

            return result;
        }

        public override VM_Role Update(VM_Role vm)
        {
            Role entity = context.Roles.Find(vm.RoleId);

            entity.RoleName = vm.RoleName;
            entity.RoleNameFa = vm.RoleNameFa;

            Save();

            return Mapper.Map<Role, VM_Role>(entity);
        }
    }
}
