using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using FrameworkDev.Web.Areas.Workflow.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
 

namespace FrameworkDev.Web.Areas.Workflow.Controllers
{
    public class WorkDeskParaphController : FrameworkDev.Web.Helpers.CustomController
    {
//        private readonly FrameworkDevEntities db = new FrameworkDevEntities();

//        public ActionResult Index()
//        {
//            return View();
//        }
//        public ActionResult _Index(int id)
//        {
//            return PartialView();
//        }

//        public ActionResult RequestParaphs_Read([DataSourceRequest]DataSourceRequest request, int ReqID)
//        {/*
//            try
//            {
//                db.sp_UpdWorkFlowParaphStatus(ReqID, User.Identity.Name);
//            }
//            catch (Exception ex)
//            {
//                Exception ex1 = ex;
//            }
//            var requestparaphs = (from r in db.RequestParaphs
//                                  where r.RequestID_fk == ReqID
//                                  join t in db.TBL_CitizenInfo on r.UserName equals t.CTZNationalNo
//                                  select (new { r.RequestParaphID, r.RequestID_fk, r.WorkFlowID, r.UserName, r.IsSeen, r.ParaphText, r.ParaphDate, t.CTZFirstName, t.CTZLastName })
//                                                         ).OrderByDescending(x => x.ParaphDate).ToList();
//            DataSourceResult result = requestparaphs.ToDataSourceResult(request, c => new VM_WorkDeskRequestParaph
//            {
//                RequestParaphID = c.RequestParaphID,
//                RequestID = c.RequestID_fk,
//                WorkFlowID = c.WorkFlowID,
//                ParapherFullName = c.CTZFirstName + " " + c.CTZLastName,
//                ParaphText = c.ParaphText,
//                Str_ParaphDate = c.ParaphDate.ToString(),
//                IsSeen = c.IsSeen ?? false,
//            });
//*/
//            return Json(null);
//        }

//        [AcceptVerbs(HttpVerbs.Post)]
//        public ActionResult RequestParaphs_Create([DataSourceRequest]DataSourceRequest request, VM_WorkDeskRequestParaph requestParaph, int ReqID)
//        {
//            /* if (ModelState.IsValid)
//             {
//                 RequestParaph entity = new RequestParaph
//                 {
//                     RequestID_fk = ReqID,
//                     WorkFlowID = requestParaph.WorkFlowID,
//                     UserName = User.Identity.Name,
//                     ParaphText = requestParaph.ParaphText,
//                     ParaphDate = DateTime.Now
//                 };

//                 db.RequestParaphs.Add(entity);
//                 db.SaveChanges();
//                 requestParaph.RequestParaphID = entity.RequestParaphID;
//                 requestParaph.Str_ParaphDate = entity.ParaphDate.ToLocalTime().ToShortDateString();
//                 TBL_CitizenInfo personel = db.TBL_CitizenInfo.FirstOrDefault(x => x.CTZNationalNo == entity.UserName);
//                 requestParaph.ParapherFullName = personel.CTZFirstName + " " + personel.CTZLastName;
//             }
// */
//            return null;// Json(new[] { requestParaph }.ToDataSourceResult(request, ModelState));
//        }

//        [AcceptVerbs(HttpVerbs.Post)]
//        public ActionResult RequestParaphs_Update([DataSourceRequest]DataSourceRequest request, VM_WorkDeskRequestParaph requestParaph)
//        {
//            if (ModelState.IsValid)
//            {
//                RequestParaph entity = new RequestParaph
//                {
//                    RequestParaphID = requestParaph.RequestParaphID,
//                    RequestID_fk = requestParaph.RequestID,
//                    WorkFlowID = requestParaph.WorkFlowID,
//                    UserName = requestParaph.UserName,
//                    ParaphText = requestParaph.ParaphText,
//                    ParaphDate = requestParaph.ParaphDate
//                };

//                db.RequestParaphs.Attach(entity);
//                db.Entry(entity).State = EntityState.Modified;
//                db.SaveChanges();
//            }

//            return Json(new[] { requestParaph }.ToDataSourceResult(request, ModelState));
//        }

//        [AcceptVerbs(HttpVerbs.Post)]
//        public ActionResult RequestParaphs_Destroy([DataSourceRequest]DataSourceRequest request, VM_WorkDeskRequestParaph requestParaph)
//        {
//            if (ModelState.IsValid)
//            {
//                RequestParaph entity = new RequestParaph
//                {
//                    RequestParaphID = requestParaph.RequestParaphID,
//                    RequestID_fk = requestParaph.RequestID,
//                    WorkFlowID = requestParaph.WorkFlowID,
//                    UserName = requestParaph.UserName,
//                    ParaphText = requestParaph.ParaphText,
//                    ParaphDate = requestParaph.ParaphDate
//                };

//                db.RequestParaphs.Attach(entity);
//                db.RequestParaphs.Remove(entity);
//                db.SaveChanges();
//            }

//            return Json(new[] { requestParaph }.ToDataSourceResult(request, ModelState));
//        }

//        protected override void Dispose(bool disposing) //TODO Remove
//        {
//            db.Dispose();
//            base.Dispose(disposing);
//        }
    }
}
