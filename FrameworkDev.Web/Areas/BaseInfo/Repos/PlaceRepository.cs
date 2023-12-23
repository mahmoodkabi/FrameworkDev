using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using FrameworkDev.Web.Areas.BaseInfo.Models;
using FrameworkDev.Web.Helpers;
using FrameworkDev.Web.Models;

namespace FrameworkDev.Web.Areas.BaseInfo.Repos
{
    public class PlaceRepository : CustomRepository<VM_PlaceInfo, int>
    {
        public override VM_PlaceInfo Delete(int id)
        {
            Place entity = context.Places.FirstOrDefault(p => p.PlaceId == id);
            try
            {
                context.Places.Remove(entity);
                Save();
            }
            catch (Exception ex)
            {
                string m = ex.ToString();
                entity.PlaceId = 0;
            }
            return Mapper.Map<Place, VM_PlaceInfo>(entity);
        }

        public override VM_PlaceInfo GetByID(int id)
        {
            Place entity = context.Places.Find(id);
            VM_PlaceInfo vm = Mapper.Map<Place, VM_PlaceInfo>(entity);
            return vm;
        }

        public override IQueryable<VM_PlaceInfo> GetList()
        {
            IQueryable<VM_PlaceInfo> result =
               context.Places

                .Select(x => new VM_PlaceInfo
                {
                    PlaceId = x.PlaceId,
                    Name = x.Name,
                    ParentId = x.ParentId,
                    HasChildren = x.Places1.Any()
                }).AsQueryable();

            return result;
        }

        public override IQueryable<VM_Lookup> GetLookupList()
        {
            IQueryable<VM_Lookup> result =
                            context.Places
                            .Select(x => new VM_Lookup { Value = x.PlaceId, Text = x.Name })
                            .AsQueryable();

            return result;
        }

        public IQueryable<VM_PLCTree> GetPLCTree(int? parentId = null)
        {
            IQueryable<VM_PLCTree> result =
                context.Places
                .Where(x => parentId.HasValue ? x.PlaceId == parentId : x.PlaceId == null)
                .Select(x => new VM_PLCTree()
                {
                    Id = x.PlaceId,
                    Name = x.Name,
                    HasChildren = x.Places1.Any()
                }).AsQueryable();

            return result;
        }

        public IQueryable<VM_PlaceInfo> GetByParent(int? parentId = null)
        {
            IQueryable<Place> result =
               context.Places
               .Where(x => parentId.HasValue ? x.ParentId == parentId : x.ParentId == null);

            List<VM_PlaceInfo> dresult = new List<VM_PlaceInfo>();

            foreach (Place item in result)
            {
                dresult.Add(Mapper.Map<Place, VM_PlaceInfo>(item));
            }

            return dresult.AsQueryable();
        }

        public override VM_PlaceInfo Insert(VM_PlaceInfo vm)
        {
            Place entity = Mapper.Map<Place>(vm);
            {
                context.Places.Add(entity);
                context.SaveChanges();
                VM_PlaceInfo resultVM = Mapper.Map<Place, VM_PlaceInfo>(entity);

                return resultVM;
            }
            //  PLCCode = vm.PLCCode,
            //  PLCName = vm.PLCName,
            // PLCParentID_fk = vm.PLCParentID_fk,
            // PLCX = vm.PLCX,
            //  PLCY = vm.PLCY,
            // PLCTelCode = vm.PLCTelCode,
            //  PLCNote = vm.PLCNote,
            //  PLCType = vm.PLCType,
            // PLCActive = vm.PLCActive,
            //  PLCStatus = vm.PLCStatus,
            // CreDate = vm.CreDate,
            // PLCUserID_fk = vm.PLCUserID_fk,






        }

        public override VM_PlaceInfo Update(VM_PlaceInfo vm)
        {
            Place entity = Mapper.Map<VM_PlaceInfo, Place>(vm);
            try
            {
                context.Places.Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
                Save();
            }
            catch (Exception ex)
            {
                string m = ex.ToString();
                entity = null;
            }
            return Mapper.Map<Place, VM_PlaceInfo>(entity);
        }

        public IQueryable<VM_PLCTree> GetPlaceTree1(int? parentId = null)
        {
            IQueryable<VM_PLCTree> result =
                context.Places
                .Where(x => parentId.HasValue ? x.ParentId == parentId : x.ParentId == null)
                .Select(x => new VM_PLCTree()
                {
                    Id = x.PlaceId,
                    Name = x.Name,
                    HasChildren = x.Places1.Any()
                }).AsQueryable();

            return result;
        }

        public IQueryable<VM_PlaceInfo> GetListByParentID(int parentId)
        {
            IQueryable<VM_PlaceInfo> result =

                context.Places.Where(x => x.ParentId == parentId)
                .Select(x => new VM_PlaceInfo
                {
                    PlaceId = x.PlaceId,
                    Name = x.Name,
                    ParentId = x.ParentId,
                    HasChildren = x.Places1.Any()
                }).AsQueryable();

            return result;
        }


    }
}
