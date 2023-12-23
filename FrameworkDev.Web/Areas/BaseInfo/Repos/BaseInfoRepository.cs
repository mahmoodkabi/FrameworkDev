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
    public class BaseInfoRepository : CustomRepository<VM_BaseInfo, int>
    {
        public override VM_BaseInfo Delete(int id)
        {
            Web.Models.BaseInfo entity = context.BaseInfoes.FirstOrDefault(p => p.BaseID == id);
            try
            {
                context.BaseInfoes.Remove(entity);
                Save();
            }
            catch (Exception ex)
            {
                string m = ex.ToString();
                entity.BaseID = 0;
            }
            return Mapper.Map<Web.Models.BaseInfo, VM_BaseInfo>(entity);
        }

        public override VM_BaseInfo GetByID(int id)
        {
            Web.Models.BaseInfo entity = context.BaseInfoes.Find(id);
            VM_BaseInfo vm = Mapper.Map<Web.Models.BaseInfo, VM_BaseInfo>(entity);
            return vm;
        }

        public override IQueryable<VM_BaseInfo> GetList()
        {
            IQueryable<VM_BaseInfo> result =
                context.BaseInfoes
                .Select(x => new VM_BaseInfo
                {
                    BaseID = x.BaseID,
                    BaseName = x.BaseName,
                    BaseCode = x.BaseCode,
                    ParentID=x.ParentID,
                    HasChildren = x.BaseInfo1.Any()
                }).AsQueryable();

            return result;
        }

        public VM_BaseInfo Get‌ByCodeName(string code)
        {
            VM_BaseInfo result =
                context.BaseInfoes.Where(x=>x.BaseCode==code)
                .Select(x => new VM_BaseInfo
                {
                    BaseID = x.BaseID,
                    BaseName = x.BaseName,
                    BaseCode = x.BaseCode,
                    HasChildren = x.BaseInfo1.Any()
                }).FirstOrDefault();
            return result;
        }

        /// <summary>
        /// پر کردن کمبو 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public IQueryable<VM_BaseInfo> GetByParentCode(string code)
        {
            Web.Models.BaseInfo baseInfo = context.BaseInfoes.FirstOrDefault(x => x.BaseCode == code);
            List<VM_BaseInfo> result = new List<VM_BaseInfo>();

            List<Web.Models.BaseInfo> dlResult = context.BaseInfoes.Where(x => x.ParentID == baseInfo.BaseID).ToList();

            foreach (Web.Models.BaseInfo item in dlResult)
            {
                result.Add(Mapper.Map<Web.Models.BaseInfo, VM_BaseInfo>(item));
            }

            return result.AsQueryable();
        }


        public IQueryable<VM_CODTree> GetBaseInfoTree1(int? parentId = null)
        {
            IQueryable<VM_CODTree> result =
                context.BaseInfoes
                .Where(x => parentId.HasValue ? x.ParentID == parentId : x.ParentID == null)
                .Select(x => new VM_CODTree()
                {
                    Id = x.BaseID,
                    Name = x.BaseName,
                    HasChildren = x.BaseInfo1.Any()
                }).AsQueryable();

            return result;
        }

        public override VM_BaseInfo Insert(VM_BaseInfo vm)
        {
            FrameworkDevEntities db = new FrameworkDevEntities();
            Web.Models.BaseInfo entity = Mapper.Map<VM_BaseInfo, Web.Models.BaseInfo>(vm);

            db.BaseInfoes.Add(entity);
            db.SaveChanges();

            VM_BaseInfo resultVM = Mapper.Map<Web.Models.BaseInfo, VM_BaseInfo>(entity);

            return resultVM;
        }

        public override VM_BaseInfo Update(VM_BaseInfo vm)
        {
            Web.Models.BaseInfo entity = Mapper.Map<VM_BaseInfo, Web.Models.BaseInfo>(vm);

            try
            {
                context.BaseInfoes.Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
                Save();
            }
            catch (Exception ex)
            {
                string m = ex.ToString();
                entity = null;
            }
            return Mapper.Map<Web.Models.BaseInfo, VM_BaseInfo>(entity);
        }

        internal VM_BaseInfo GetByCode(string code)
        {
            Web.Models.BaseInfo baseInfo = context.BaseInfoes.First(x => x.BaseCode == code);
            VM_BaseInfo resultVM = Mapper.Map<Web.Models.BaseInfo, VM_BaseInfo>(baseInfo);
            return resultVM;



            //List<VM_BaeInfo> result = new List<VM_BaeInfo>();

            //List<Web.Models.BaseInfo> dlResult = context.BaseInfoes.Where(x => x.ParentId == baseInfo.ParentId).ToList();

            //foreach (Web.Models.BaseInfo item in dlResult)
            //{
            //    result.Add(Mapper.Map<Web.Models.BaseInfo, VM_BaeInfo>(item));
            //}

            //return result.AsQueryable();
        }
    }
}
