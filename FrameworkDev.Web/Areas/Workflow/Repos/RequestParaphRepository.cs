using AutoMapper;
using FrameworkDev.Web.Areas.Workflow.Models;
using FrameworkDev.Web.Helpers;
using FrameworkDev.Web.Models;
using System.Data.Entity;
using System.Linq;

namespace FrameworkDev.Web.Areas.Workflow.Repos
{
    /// <summary>
    ///
    /// </summary>
    public class RequestParaphRepository : CustomRepository<VM_RequestParaph, int>
    {
        //======================================================نمایش لیست کلی
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override IQueryable<VM_RequestParaph> GetList()
        {/*
            IQueryable<VM_RequestParaph> dataset = context.RequestParaphs.Select(RequestParaphs => new VM_RequestParaph
            {
                RequestParaphID = RequestParaphs.RequestParaphID,
                RequestID_fk = RequestParaphs.RequestID_fk,
                WorkFlowID = RequestParaphs.WorkFlowID,
                UserName = RequestParaphs.UserName,
                ParaphText = RequestParaphs.ParaphText,
                ParaphDate = RequestParaphs.ParaphDate,
                IsSeen = RequestParaphs.IsSeen,
                WorkFlowTypeID_fk = RequestParaphs.WorkFlowTypeID_fk
            });
            */
               return null;//urn dataset;
        }   

        //======================================================نمایش اطلاعات بر اساس شناسه
        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override VM_RequestParaph GetByID(int id)
        {
            RequestParaph entity = context.RequestParaphs.Find(id);
            VM_RequestParaph vm = Mapper.Map<RequestParaph, VM_RequestParaph>(entity);
            return vm;
        }

        public VM_RequestParaph GetForReport(int pRequestID
                                            ,string pUserName
                                            ,int pWorkFlowTypeID_fk
                                            )
        {/*
            VM_RequestParaph vm = null;
            try
            {
                 vm = context.RequestParaphs.Select(RequestParaphs => new VM_RequestParaph
                            {
                                RequestParaphID = RequestParaphs.RequestParaphID,
                                RequestID_fk = RequestParaphs.RequestID_fk,
                                WorkFlowID = RequestParaphs.WorkFlowID,
                                UserName = RequestParaphs.UserName,
                                ParaphText = RequestParaphs.ParaphText,
                                ParaphDate = RequestParaphs.ParaphDate,
                                IsSeen = RequestParaphs.IsSeen,
                                WorkFlowTypeID_fk = RequestParaphs.WorkFlowTypeID_fk
                            }).Where(p=>p.RequestID_fk==(pRequestID)
                                    &&  p.UserName.Contains(pUserName)
                                    &&  p.WorkFlowTypeID_fk==(pWorkFlowTypeID_fk))
                                    .OrderByDescending(x=>x.ParaphDate).FirstOrDefault();
            }
            catch (System.Exception ex)
            {
                var ex1 = ex;
            }            
            */
            return null;// vm;
        }

        //======================================================ایجاد اطلاعات
        /// <summary>
        ///
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public override VM_RequestParaph Insert(VM_RequestParaph vm)
        {
            RequestParaph entity = Mapper.Map<VM_RequestParaph, RequestParaph>(vm);

            context.RequestParaphs.Add(entity);
            context.SaveChanges();

            VM_RequestParaph resultVM = Mapper.Map<RequestParaph, VM_RequestParaph>(entity);

            return resultVM;
        }

        //======================================================ویرایش اطلاعات
        /// <summary>
        ///
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public override VM_RequestParaph Update(VM_RequestParaph vm)
        {
            RequestParaph entity = Mapper.Map<VM_RequestParaph, RequestParaph>(vm);
            context.RequestParaphs.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            Save();
            return Mapper.Map<RequestParaph, VM_RequestParaph>(entity);
        }

        //======================================================حذف اطلاعات
        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override VM_RequestParaph Delete(int id)
        {
            RequestParaph entity = context.RequestParaphs.FirstOrDefault(p => p.RequestParaphID == id);
            context.RequestParaphs.Remove(entity);
            Save();
            return Mapper.Map<RequestParaph, VM_RequestParaph>(entity);
        }
    }
}
