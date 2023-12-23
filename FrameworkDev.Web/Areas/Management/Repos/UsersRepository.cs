using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FrameworkDev.Web.Areas.Management.Models;
using FrameworkDev.Web.Helpers;
using FrameworkDev.Web.Helpers.Authentication;
using FrameworkDev.Web.Models;

namespace FrameworkDev.Web.Areas.Management.Repos
{
    public class UsersRepository : CustomRepository<VM_User, int>
    {
        private readonly string DefaultPassword = @"********";

        public override VM_User Delete(int id)
        {
            User entity = context.Users.FirstOrDefault(p => p.UserId == id);
            entity.IsActive = false;
            //context.Users.Remove(entity);
            Save();
            return Mapper.Map<User, VM_User>(entity);
        }

        public override VM_User GetByID(int id)
        {
            List<VM_Permission> permissions = CustomPermissionKeyHelper.GetAllPermissionKeys();

            User entity = context.Users
                .Include("Roles")
                .Include("Permissions")
                .Include("Profiles")
                .FirstOrDefault(x => x.UserId == id);

            VM_User vm = null;

            Dictionary<string, string> _profiles = new Dictionary<string, string>();

            foreach (Web.Models.Profile pf in entity.Profiles)
            {
                _profiles.Add(pf.ProfileKey, pf.ProfileValue);
            }

            if (entity != null)
            {
                vm = new VM_User()
                {
                    UserId = entity.UserId,
                    UserName = entity.Username,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Email = entity.Email,
                    Password = DefaultPassword,
                    PasswordConfirm = DefaultPassword,
                    IsActive = entity.IsActive,
                    //CitizenID = entity.CitizenID,
                    Permissions = entity.Permissions.Select(x => x.PermissionKey).ToList(),
                    Roles = entity.Roles.Select(x => x.RoleId).ToList(),
                    Profiles = _profiles
                };
            }

            return vm;
        }

        public override IQueryable<VM_User> GetList()
        {
            List<VM_Permission> permissions = CustomPermissionKeyHelper.GetAllPermissionKeys();

            return context.Users.AsNoTracking().Select(user => new VM_User
            {
                UserId = user.UserId,
                UserName = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = DefaultPassword,
                PasswordConfirm = DefaultPassword,
                IsActive = user.IsActive,
                //CitizenID = user.CitizenID,
                Permissions = user.Permissions.Select(x => x.PermissionKey).ToList(),
                Roles = user.Roles.Select(x => x.RoleId).ToList()
            });
        }

        public IQueryable<VM_User> GetUsersListByRoleId(int roleid)
        {
            Role role = context.Roles.Include("Users").FirstOrDefault(x => x.RoleId == roleid);

            return role.Users.Select(user => new VM_User
            {
                UserId = user.UserId,
                UserName = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = DefaultPassword,
                PasswordConfirm = DefaultPassword,
                IsActive = user.IsActive,
                //CitizenID = user.CitizenID,
                Permissions = user.Permissions.Select(x => x.PermissionKey).ToList(),
                Roles = user.Roles.Select(x => x.RoleId).ToList()
            }).AsQueryable();
        }

        public VM_User Insert(VM_User vm, List<RolePermission> rolePermissions)
        {
            User entity = new User
            {
                Username = vm.UserName,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Email = vm.Email,
                IsActive = vm.IsActive,
            };

            if (vm.Password != DefaultPassword)
            {
                entity.Salt = CustomAuthenticationHelpers.GeneratePassword(100);
                entity.Password = CustomAuthenticationHelpers.EncodePassword(vm.Password, entity.Salt);
            }

            entity.Roles.Clear();

            if (vm.Roles != null)
            {
                foreach (int role in vm.Roles)
                {
                    entity.Roles.Add(context.Roles.Find(role));
                }
            }

            entity.Permissions.Clear();

            if (vm.Permissions != null)
            {
                foreach (string pk in vm.Permissions)
                {
                    entity.Permissions.Add(context.Permissions.FirstOrDefault(x => x.PermissionKey == pk));
                }
            }

            context.Users.Add(entity);
            context.RolePermissions.AddRange(rolePermissions);
            Save();

            return Mapper.Map<User, VM_User>(entity);
        }

        public override VM_User Update(VM_User vm)
        {
            User entity = context.Users.Include("Roles").FirstOrDefault(x => x.UserId == vm.UserId);

            entity.FirstName = vm.FirstName == null ? "" : vm.FirstName;
            entity.LastName = vm.LastName;
            entity.Email = vm.Email;
            entity.IsActive = vm.IsActive;

            if (vm.Password != DefaultPassword)
            {
                entity.Salt = CustomAuthenticationHelpers.GeneratePassword(100);
                entity.Password = CustomAuthenticationHelpers.EncodePassword(vm.Password, entity.Salt);
            }

            if (vm.Roles != null)
            {
                List<int> exRoles = entity.Roles.Select(x => x.RoleId).ToList();

                foreach (int roleId in exRoles)
                {
                    if (!vm.Roles.Any(x => x == roleId))
                    {
                        entity.Roles.Remove(entity.Roles.First(x => x.RoleId == roleId));
                    }
                }

                foreach (int role in vm.Roles)
                {
                    if (!entity.Roles.Any(x => x.RoleId == role))
                    {
                        entity.Roles.Add(context.Roles.Find(role));
                    }
                }
            }
            else
            {
                entity.Roles.Clear();
            }

            entity.Permissions.Clear();

            if (vm.Permissions != null)
            {
                foreach (string pk in vm.Permissions)
                {
                    entity.Permissions.Add(context.Permissions.FirstOrDefault(x => x.PermissionKey == pk));
                }
            }

            Save();

            return Mapper.Map<User, VM_User>(entity);
        }
    }
}
