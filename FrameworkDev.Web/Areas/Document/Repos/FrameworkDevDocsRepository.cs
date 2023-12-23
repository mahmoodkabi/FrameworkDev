using AutoMapper;


using FrameworkDev.Web.Areas.Document.Models;
using FrameworkDev.Web.Models;
using FrameworkDev.Web.Helpers;

using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace FrameworkDev.Web.Areas.Document.Repos
{
    public class FrameworkDevDocsRepository : CustomRepository<VM_FrameworkDevDocs, int>
    {
        //public IQueryable<VM_FrameworkDevDocs> SelectAll()
        //{
        //    List<VM_FrameworkDevDocs> result = new List<VM_FrameworkDevDocs>();
        //    try
        //    {
        //        List<VIW_TISDocsView> dlResult = context.VIW_TISDocsView.ToList();

        //        foreach (VIW_TISDocsView item in dlResult)
        //        {
        //            result.Add(Mapper.Map<VIW_TISDocsView, VM_FrameworkDevDocs>(item));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Exception ex2 = ex;
        //    }
        //    return result.AsQueryable();
        //}

        public VM_FrameworkDevDocs SelectById(string id)
        {
            //VM_FrameworkDevDocs result = new VM_FrameworkDevDocs();
            //try
            //{
            //    VIW_TISDocsView dlResult = context.VIW_TISDocsView.Where(h => h.stream_id.ToString() == id).First();
            //    result = Mapper.Map<VIW_TISDocsView, VM_FrameworkDevDocs>(dlResult);
            //}
            //catch (Exception ex)
            //{
            //    Exception ex2 = ex;
            //}

            //return result;

            return null;
        }

        //public VM_FrameworkDevDocs SelectByName(string Name)
        //{
        //    VM_FrameworkDevDocs result = new VM_FrameworkDevDocs();
        //    try
        //    {
        //        VIW_TISDocsView dlResult = context.VIW_TISDocsView.Where(h => h.name.Contains(Name)).First();
        //        result = Mapper.Map<VIW_TISDocsView, VM_FrameworkDevDocs>(dlResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        Exception ex2 = ex;
        //    }

        //    return result;
        //}

        public string Create(VM_FrameworkDevDocs File)
        {
            ObjectResult<string> res = context.sp_DocsAdd(File.name, File.file_stream);
            return res.FirstOrDefault();
        }

        public bool Delete(string docId)
        {
            try
            {
                return context.sp_DocsDelete(new Guid(docId)) != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
