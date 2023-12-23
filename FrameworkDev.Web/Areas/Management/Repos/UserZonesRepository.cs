using AutoMapper;


using FrameworkDev.Web.Areas.Management.Models;
using FrameworkDev.Web.Models;
using FrameworkDev.Web.Helpers;
using System.Data.Entity;
using System.Linq;
using FrameworkDev.Web.Areas.BaseInfo.Repos;

namespace FrameworkDev.Web.Areas.Management.Repos
{
    public class UserZonesRepository : CustomRepository<VM_UserZones, int>
    {
        public override IQueryable<VM_UserZones> GetList()
        {
            return context.TBL_UserZones.Select(UserZones => new VM_UserZones
            {
                UserZoneID = UserZones.UserZoneID,
                USZUserId_fk = UserZones.USZUserId_fk,
                USZZoneId_fk = UserZones.USZZoneId_fk,
                USZNote = UserZones.USZNote,
                USZActive = UserZones.USZActive,
                USZStatus = UserZones.USZStatus,
                USZType = UserZones.USZType,
                USZCreDate = UserZones.USZCreDate,
                USZCreateUserId_fk = UserZones.USZCreateUserId_fk,
                //ZONName = UserZones.TBL_Zone.ZONName,
                ZoneID = UserZones.USZZoneId_fk
            });
        }


        public IQueryable<VM_UserZones> GetAccessList(string _username, string codeFile_code)
        {
            var parentId = new ZonesRepository().GetList().First(p => p.Code == codeFile_code).ZoneId;

            return context.TBL_UserZones
                      .Where(x => x.User.Username == _username && x.Zone.ParentId == parentId)
                      .OrderBy(x => x.Zone.Code)
                      .Select(Z => new VM_UserZones
                      {
                          UserZoneID = Z.UserZoneID,
                          USZUserId_fk = Z.USZUserId_fk,
                          USZZoneId_fk = Z.USZZoneId_fk,
                          USZNote = Z.USZNote,
                          USZActive = Z.USZActive,
                          USZStatus = Z.USZStatus,
                          USZType = Z.USZType,
                          USZCreDate = Z.USZCreDate,
                          USZCreateUserId_fk = Z.USZCreateUserId_fk,
                          ZONName = Z.Zone.Name,
                          //ZONType = Z. Zone.Type,
                          ZoneID = Z.USZZoneId_fk,
                          Code = Z.Zone.Code
                      });

        }


        public IQueryable<VM_UserZones> GetByUserID(int _userid)
        {
            IQueryable<VM_UserZones> res = context.TBL_UserZones
                .Where(x => x.USZUserId_fk == _userid)
                .Select(Z => new VM_UserZones
                {
                    UserZoneID = Z.UserZoneID,
                    USZUserId_fk = Z.USZUserId_fk,
                    USZZoneId_fk = Z.USZZoneId_fk,
                    USZNote = Z.USZNote,
                    USZActive = Z.USZActive,
                    USZStatus = Z.USZStatus,
                    USZType = Z.USZType,
                    USZCreDate = Z.USZCreDate,
                    USZCreateUserId_fk = Z.USZCreateUserId_fk,
                });

            return res;
        }

        public override VM_UserZones GetByID(int id)
        {
            TBL_UserZones entity = context.TBL_UserZones.Find(id);
            VM_UserZones vm = Mapper.Map<TBL_UserZones, VM_UserZones>(entity);
            return vm;
        }

        public override VM_UserZones Insert(VM_UserZones vm)
        {
            TBL_UserZones entity = Mapper.Map<VM_UserZones, TBL_UserZones>(vm);

            context.TBL_UserZones.Add(entity);
            context.SaveChanges();

            VM_UserZones resultVM = Mapper.Map<TBL_UserZones, VM_UserZones>(entity);

            return resultVM;
        }

        public override VM_UserZones Update(VM_UserZones vm)
        {
            TBL_UserZones entity = Mapper.Map<VM_UserZones, TBL_UserZones>(vm);
            context.TBL_UserZones.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            Save();
            return Mapper.Map<TBL_UserZones, VM_UserZones>(entity);
        }

        public override VM_UserZones Delete(int id)
        {
            TBL_UserZones entity = context.TBL_UserZones.FirstOrDefault(p => p.UserZoneID == id);
            context.TBL_UserZones.Remove(entity);
            Save();
            return Mapper.Map<TBL_UserZones, VM_UserZones>(entity);
        }
    }
}
