using System;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using FrameworkDev.Web.Areas.BaseInfo.Models;
using FrameworkDev.Web.Helpers;
using FrameworkDev.Web.Models;
using Zone = FrameworkDev.Web.Models.Zone;
using VM_Zone = FrameworkDev.Web.Areas.BaseInfo.Models.VM_ZoneInfo;

namespace FrameworkDev.Web.Areas.BaseInfo.Repos
{
    public class ZonesRepository : CustomRepository<VM_Zone, int>
    {
        #region CRUD

        public override IQueryable<VM_Zone> GetList()
        {
            IQueryable<VM_Zone> result =
                context.Zones
                .Select(x => new VM_Zone
                {
                    ZoneId = x.ZoneId,
                    Name = x.Name,
                    ParentId = x.ParentId,
                    Code = x.Code,
                    TelNo = x.TelNo,
                    Address = x.Address,
                    WorkShopNo = x.WorkShopNo,
                    HasChildren = x.Zones1.Any()
                }).AsQueryable();

            return result;
        }
        public  IQueryable<VM_Zone> GetListByParentID(int parentId)
        {
            IQueryable<VM_Zone> result =
                context.Zones.Where(x => x.ParentId== parentId)
                .Select(x => new VM_Zone
                {
                    ZoneId = x.ZoneId,
                    Name = x.Name,
                    ParentId = x.ParentId,
                    Code = x.Code,
                    TelNo = x.TelNo,
                    Address = x.Address,
                    WorkShopNo = x.WorkShopNo,
                    HasChildren = x.Zones1.Any()
                }).AsQueryable();

            return result;
        }
     
        public override IQueryable<VM_Zone> GetListByRelId(int _relId)
        {
            IQueryable<VM_Zone> result =
                            //context.Zones.Where(x => x.TBL_UserZones.Any(uz => uz.USZUserId_fk == _relId))
                            context.Zones
                            .Select(x => new VM_Zone
                            {
                                ZoneId = x.ZoneId,
                                Name = x.Name,


                               
                        ParentId= x.ParentId,
                        Code= x.Code,
                        TelNo= x.TelNo,
                        Address= x.Address,
                        WorkShopNo= x.WorkShopNo,





                                HasChildren = x.Zones1.Any()
                            }).AsQueryable();
            var t = result.ToList();
            return result;

            //return null;
        }

        public override VM_Zone GetByID(int id)
        {
            Zone entity = context.Zones.Find(id);
            VM_Zone vm = Mapper.Map<Zone, VM_Zone>(entity);
            return vm;
        }

        public override IQueryable<VM_Lookup> GetLookupList()
        {
            IQueryable<VM_Lookup> result =
                            context.Zones
                            .Select(x => new VM_Lookup { Value = x.ZoneId, Text = x.Name })
                            .AsQueryable();

            return result;
        }

        public override VM_Zone Insert(VM_Zone vm)
        {
            Zone entity = Mapper.Map<VM_Zone, Zone>(vm);
           
            context.Zones.Add(entity);
            context.SaveChanges();

            VM_Zone resultVM = Mapper.Map<Zone, VM_Zone>(entity);

            return resultVM;
        }

        public override VM_Zone Update(VM_Zone vm)
        {
            Zone entity = Mapper.Map<VM_Zone, Zone>(vm);
            try
            {
                context.Zones.Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
                Save();
            }
            catch (Exception ex)
            {
                string m = ex.ToString();
                entity = null;
            }
            return Mapper.Map<Zone, VM_Zone>(entity);
        }

        public override VM_Zone Delete(int id)
        {
            Zone entity = context.Zones.FirstOrDefault(p => p.ZoneId == id);
            try
            {
                context.Zones.Remove(entity);
                Save();
            }
            catch (Exception ex)
            {
                string m = ex.ToString();
                entity.ZoneId = 0;
            }
            return Mapper.Map<Zone, VM_Zone>(entity);
        }

        #endregion CRUD

        public IQueryable<VM_Zone> GetDiffList(int userId)
        {
            IQueryable<VM_Zone> zones = GetList();

            IQueryable<VM_Zone> user_zones = GetListByRelId(userId);
            var a =  zones.ToList();
            var b = user_zones.ToList();
            IQueryable<VM_Zone> result = zones.Where(p => !user_zones.Any(p2 => p2.ZoneId == p.ZoneId));

            var d = result.ToList();


            return result;
        }

        public IQueryable<VM_ZoneInfo> GetListZone()
        {
            return context.Zones.Select(TBL_Zone => new VM_ZoneInfo
            {
                ZoneId = TBL_Zone.ZoneId,
                 Name = TBL_Zone.Name,
            });
        }

        public IQueryable<VM_ZoneTree> GetZonesTree(int? parentId)
        {
            IQueryable<VM_ZoneTree> result =
                context.Zones
                .Where(x => parentId.HasValue ? x.ParentId == parentId : x.ParentId == null)
                .Select(x => new VM_ZoneTree()
                {
                    Id = x.ZoneId,
                    Name = x.Name,
                    HasChildren = x.Zones1.Any()
                }).AsQueryable();

            return result;
        }
    }
}
