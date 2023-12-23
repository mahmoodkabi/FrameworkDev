using AutoMapper;


using FrameworkDev.Web.Areas.Management.Models;
using FrameworkDev.Web.Models; using FrameworkDev.Web.Helpers;

using System.Linq;

namespace FrameworkDev.Web.Areas.Management.Repos
{
    public class RolePermissionsRepository : CustomRepository<VM_RolePermission, int>
    {
        public override VM_RolePermission Delete(int id)
        {
            RolePermission entity = context.RolePermissions.FirstOrDefault(p => p.RPKId == id);
            context.RolePermissions.Remove(entity);
            Save();
            return Mapper.Map<RolePermission, VM_RolePermission>(entity);
        }

        public override VM_RolePermission GetByID(int id)
        {
            RolePermission entity = context.RolePermissions.Find(id);

            VM_RolePermission vm = new VM_RolePermission()
            {
                RPKId = entity.RPKId,
                PermissionKey = entity.PermissionKey,
                RoleId = entity.RoleId,
            };

            return vm;
        }

        public override IQueryable<VM_RolePermission> GetList()
        {
            IQueryable<VM_RolePermission> RolePermissions = context.RolePermissions.AsNoTracking().Select(RolePermission => new VM_RolePermission
            {
                RPKId = RolePermission.RPKId,
                PermissionKey = RolePermission.PermissionKey,
                RoleId = RolePermission.RoleId,
            });

            return RolePermissions;
        }

        public IQueryable<VM_RolePermission> GetList(int roleId)
        {
            IQueryable<VM_RolePermission> RolePermissions = context.RolePermissions.AsNoTracking().Where(x => x.RoleId == roleId).Select(RolePermission => new VM_RolePermission
            {
                RPKId = RolePermission.RPKId,
                PermissionKey = RolePermission.PermissionKey,
                RoleId = RolePermission.RoleId,
            });

            return RolePermissions;
        }

        public override VM_RolePermission Insert(VM_RolePermission vm)
        {
            RolePermission entity = new RolePermission
            {
                RPKId = vm.RPKId,
                PermissionKey = vm.PermissionKey,
                RoleId = vm.RoleId,
            };

            context.RolePermissions.Add(entity);
            Save();

            VM_RolePermission vmResult = Mapper.Map<RolePermission, VM_RolePermission>(entity);

            return vmResult;
        }

        public override VM_RolePermission Update(VM_RolePermission vm)
        {
            if (vm.@checked)
            {
                RolePermission entity = context.RolePermissions.FirstOrDefault(x => x.PermissionKey == vm.PermissionKey && x.RoleId == vm.RoleId);

                if (entity != null)
                {
                    entity.PermissionKey = vm.PermissionKey;
                    entity.RoleId = vm.RoleId;

                    Save();

                    return Mapper.Map<RolePermission, VM_RolePermission>(entity);
                }
                else
                {
                    return Insert(vm);
                }
            }
            else
            {
                RolePermission entity = context.RolePermissions.FirstOrDefault(x => x.PermissionKey == vm.PermissionKey && x.RoleId == vm.RoleId);

                if (entity != null)
                {
                    return Delete(entity.RPKId);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
