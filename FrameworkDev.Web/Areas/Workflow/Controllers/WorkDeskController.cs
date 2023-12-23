using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

using FrameworkDev.Web.Areas.Management.Controllers;

using FrameworkDev.Web.Areas.Workflow.Models;
using FrameworkDev.Web.Areas.Workflow.Repos;
using FrameworkDev.Web.Helpers.Authentication;
using FrameworkDev.Web.Helpers.Menus;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace FrameworkDev.Web.Areas.Workflow.Controllers
{
    [MenuItem(Title = "كارتابل", Order = 1, IsClickable = false, CssIcon = "fa fas fa-book fa-lg fa-fw")]
    [CustomAuthorize(PermissionKey = "STP:WKF", PermissionName = "مدیریت کارتابل")]
    public class WorkDeskController : Helpers.CustomController
    {/*
        private readonly FrameworkDevEntities db = new FrameworkDevEntities();
        private readonly WorkDeskRepository repo = new WorkDeskRepository();

        // نتایج بخش جستجوی کارتابل --------------------------------------
        //static List<VM_EncourageAcademic> EncourageAcademicSearchResult = null;
        //static List<VM_RulInfo> RulingSearchResult = null;
        //static List<VM_Eblagh> EblaghSearchResult = null;


        //-------------------------------------------------------

        [CustomAuthorize(PermissionKey = "STP:WKF:MReq", PermissionName = "درخواست های من")]
        [MenuItem(Title = "درخواست های من", Order = 1, ParentController = typeof(WorkDeskController), CssIcon = "fa fa-group fa-lg fa-fw")]
        public ActionResult MyReq()
        {
            ViewBag.Type = "All";
            ViewBag.Kind = "درخواست های من";
            return View("Index");
        }

        [MenuItem(Title = "جهت بررسی", Order = 2, ParentController = typeof(WorkDeskController), CssIcon = "fa fa-group fa-lg fa-fw")]
        [CustomAuthorize(PermissionKey = "STP:WKF:TDoReq", PermissionName = "جهت بررسی")]
        public ActionResult ToDoReq()
        {
            ViewBag.Type = "WorkDesk,InitialUserAndNotSend";
            ViewBag.Kind = "جهت بررسی";
            return View("Index");
        }

        [MenuItem(Title = "تایید شده", Order = 3, ParentController = typeof(WorkDeskController), CssIcon = "fa fa-group fa-lg fa-fw")]
        [CustomAuthorize(PermissionKey = "STP:WKF:CReq", PermissionName = "تایید شده")]
        public ActionResult ConfirmedReq()
        {
            ViewBag.Type = "ConfirmedByStatusID";
            ViewBag.Kind = "تایید شده";
            return View("Index");
        }

        [MenuItem(Title = "رد شده", Order = 4, ParentController = typeof(WorkDeskController), CssIcon = "fa fa-group fa-lg fa-fw")]
        [CustomAuthorize(PermissionKey = "STP:WKF:FReq", PermissionName = "رد شده")]
        public ActionResult FailedReq()
        {
            ViewBag.Type = "FailedByStatusID";
            ViewBag.Kind = "رد شده";
            return View("Index");
        }

        [CustomAuthorize(PermissionKey = "STP:WKF:A", PermissionName = "تایید درخواست")]
        public bool ApproveAction()
        {
            return true;
        }

        [CustomAuthorize(PermissionKey = "STP:WKF:D", PermissionName = "رد و اتمام درخواست")]
        public bool DeleteAction()
        {
            return true;
        }

        [CustomAuthorize(PermissionKey = "STP:WKF:E", PermissionName = "رد جهت ویرایش درخواست")]
        public bool RejectToEditAction()
        {
            return true;
        }


       public ActionResult MyWorkDesk([DataSourceRequest]DataSourceRequest request, string type, string fromDate, string toDate, string wrokflowType)
        {
            DataSourceResult resultResWorkDesk = null;
         /*   try
            {
                IList<sp_WorkDesk_Result> resWorkDesk = repo.WorkDesk(User.Identity.Name, type, fromDate, toDate).ToList();

                //IList<USP_EncourageAcademicReport_Result> resEncourageAcademicReport = repo.USP_EncourageAcademicReport().ToList();

                IEnumerable<VM_WorkDesk> res = null;

                switch (wrokflowType)
                {
                    case "1": //EncourageAcademic
                        res = from w in resWorkDesk
                              join e in EncourageAcademicSearchResult on w.EntityID equals e.EncourageAcademicID
                              join s in db.WorkFlowSteps on w.WorkFlowStepID equals s.WorkFlowStepID
                              where s.WorkFlowTypeID_fk == 1 //EncourageAcademic
                              select new VM_WorkDesk()
                              {
                                  EntityID = w.EntityID,
                                  FinalConfirmationDate = w.FinalConfirmationDate,
                                  FromUserName = w.FromUserName,
                                  FullNameFromUserName = w.FromUserName,
                                  InitialUserName = w.InitialUserName,
                                  FullNameInitialUserName = w.InitialUserName,
                                  ID = w.ID,
                                  RequestID = w.RequestID,
                                  RequestDate = w.RequestDate,
                                  RequestResultName = new PubBaseRepository().GetByID((int)w.RequestResultID).BaseName,
                                  RequestResultID = w.RequestResultID,
                                  WorkFlowStepID = w.WorkFlowStepID,
                                  WorkFlowTypeID = w.WorkFlowTypeID,
                                  WorkFlowType = new WorkFlowTypeRepository().GetByID((int)w.WorkFlowTypeID)
                              };
                        EncourageAcademicSearchResult = null;
                        break;

                    case "2": //Ruling
                        res = from w in resWorkDesk
                              join r in RulingSearchResult on w.EntityID equals r.RUlID
                              join s in db.WorkFlowSteps on w.WorkFlowStepID equals s.WorkFlowStepID
                              where s.WorkFlowTypeID_fk == 2 //Ruling
                              select new VM_WorkDesk()
                              {
                                  EntityID = w.EntityID,
                                  FinalConfirmationDate = w.FinalConfirmationDate,
                                  FromUserName = w.FromUserName,
                                  FullNameFromUserName = w.FromUserName,
                                  InitialUserName = w.InitialUserName,
                                  FullNameInitialUserName = w.InitialUserName,
                                  ID = w.ID,
                                  RequestID = w.RequestID,
                                  RequestDate = w.RequestDate,
                                  RequestResultName = new PubBaseRepository().GetByID((int)w.RequestResultID).BaseName,
                                  RequestResultID = w.RequestResultID,
                                  WorkFlowStepID = w.WorkFlowStepID,
                                  WorkFlowTypeID = w.WorkFlowTypeID,
                                  WorkFlowType = new WorkFlowTypeRepository().GetByID((int)w.WorkFlowTypeID)
                              };
                        RulingSearchResult = null;
                        break;

                    case "3": //Eblagh
                        res = from w in resWorkDesk
                              join e in EblaghSearchResult on w.EntityID equals e.EblaghID
                              join s in db.WorkFlowSteps on w.WorkFlowStepID equals s.WorkFlowStepID
                              where s.WorkFlowTypeID_fk == 3 //Eblagh
                              select new VM_WorkDesk()
                              {
                                  EntityID = w.EntityID,
                                  FinalConfirmationDate = w.FinalConfirmationDate,
                                  FromUserName = w.FromUserName,
                                  FullNameFromUserName = w.FromUserName,
                                  InitialUserName = w.InitialUserName,
                                  FullNameInitialUserName = w.InitialUserName,
                                  ID = w.ID,
                                  RequestID = w.RequestID,
                                  RequestDate = w.RequestDate,
                                  RequestResultName = new PubBaseRepository().GetByID((int)w.RequestResultID).BaseName,
                                  RequestResultID = w.RequestResultID,
                                  WorkFlowStepID = w.WorkFlowStepID,
                                  WorkFlowTypeID = w.WorkFlowTypeID,
                                  WorkFlowType = new WorkFlowTypeRepository().GetByID((int)w.WorkFlowTypeID)
                              };
                        EblaghSearchResult = null;
                        break;
                    default:
                        res = from w in resWorkDesk
                              select new VM_WorkDesk()
                              {
                                  EntityID = w.EntityID,
                                  FinalConfirmationDate = w.FinalConfirmationDate,
                                  FromUserName = w.FromUserName,
                                  FullNameFromUserName = w.FromUserName,
                                  InitialUserName = w.InitialUserName,
                                  FullNameInitialUserName = w.InitialUserName,
                                  ID = w.ID,
                                  RequestID = w.RequestID,
                                  RequestDate = w.RequestDate,
                                  RequestResultName = new PubBaseRepository().GetByID((int)w.RequestResultID).BaseName,
                                  RequestResultID = w.RequestResultID,
                                  WorkFlowStepID = w.WorkFlowStepID,
                                  WorkFlowTypeID = w.WorkFlowTypeID,
                                  WorkFlowType = new WorkFlowTypeRepository().GetByID((int)w.WorkFlowTypeID)
                              };
                        break;
                }

                resultResWorkDesk = res.ToDataSourceResult(request);
                foreach (object item in resultResWorkDesk.Data)
                {
                    ((VM_WorkDesk)item).RequestDateJalali = Utility.ToPersianDateString(((VM_WorkDesk)item).RequestDate);
                }
            }
            catch (Exception ex)
            {
                Exception ex1 = ex;

            return Json(resultResWorkDesk, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [CustomAuthorize(PermissionKey = "STP:WKF:R", PermissionName = "نمایش درخواست")]
        public async Task<ActionResult> SendToWorkFlow(string workFlowType, string workFlowStepType, int requestID, int currentStepID, string sendOrMessage, string externalCondition, string paraph)
        {
           /* var req = (from r in db.Requests
                       where r.RequestID == requestID
                       select new { r.EntityID }
                             ).FirstOrDefault();

            List<VM_RepoWorkDesk> result = repo.SendToWorkFlow(workFlowStepType, User.Identity.Name, requestID, currentStepID, sendOrMessage, externalCondition, paraph);

            if (!result[0].ExceptionMessage.Equals("ExceptionMessage") && sendOrMessage == "Send")
            {
                switch (workFlowType)
                {
                    case "1": //EncourageAcademic
                        Welfare.Models.VM_EncourageAcademic EAInfo = new Welfare.Repos.EncourageAcademicRepository().GetByID(req.EntityID);
                        try
                        {
                            int? reqstatte = db.Requests.FirstOrDefault(x => x.RequestID == requestID).RequestResultID;
                            string SmsText = string.Empty;
                            switch (reqstatte)
                            {
                                case 20: //Approved
                                    SmsText = "درخواست تشویق تحصیلی فرزند شما " + (EAInfo.ChildCitizenInfo.CTZFirstName + ' ' + EAInfo.ChildCitizenInfo.CTZLastName) + " تایید گردید";
                                    break;

                                case 21: //Rejected
                                    SmsText = "درخواست تشویق تحصیلی فزند شما " + (EAInfo.ChildCitizenInfo.CTZFirstName + ' ' + EAInfo.ChildCitizenInfo.CTZLastName) + " تایید نشد";
                                    break;

                                case 22: //InProgress
                                    SmsText = "درخواست تشویق تحصیلی فزند شما " + (EAInfo.ChildCitizenInfo.CTZFirstName + ' ' + EAInfo.ChildCitizenInfo.CTZLastName) + " ثبت گردید. منتظر تایید مسئول رفاه باشید";
                                    break;

                                case 23: //RejectedForEdit
                                    SmsText = "درخواست تشویق تحصیلی فزند شما " + (EAInfo.ChildCitizenInfo.CTZFirstName + ' ' + EAInfo.ChildCitizenInfo.CTZLastName) + " ناقص می باشد. جهت ویرایش آن اقدام فرمایید";
                                    break;

                                case 24: //Canceled
                                    SmsText = "درخواست تشویق تحصیلی فزند شما " + (EAInfo.ChildCitizenInfo.CTZFirstName + ' ' + EAInfo.ChildCitizenInfo.CTZLastName) + " لغو گردید";
                                    break;
                            }
                            if (!SmsText.Equals(string.Empty) && !EAInfo.ParentCitizenInfo.CTZMobileNo.Equals(string.Empty))
                            {
                                new SMSController().SendSMS(EAInfo.ParentCitizenInfo.CTZMobileNo, SmsText);
                            }
                        }
                        catch (Exception ex)
                        {
                            Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                        }
                        break;
                    case "2": //Ruling
                        var RULRepos = new Rulings.Repos.RulRepository();
                        Rulings.Models.VM_RulInfo RULInfo = RULRepos.GetByID(req.EntityID);
                        try
                        {
                            int? reqstatte = db.Requests.FirstOrDefault(x => x.RequestID == requestID).RequestResultID;
                            switch (reqstatte)
                            {
                                case 20: //Approved
                                    string CurrentStepUser2 = (from s in db.WorkFlowSteps
                                                               where s.WorkFlowStepID == currentStepID && s.ReciverType == "User"
                                                               select s.ReciverTypeValue
                                                    ).FirstOrDefault().ToString();
                                    if (User.Identity.Name == CurrentStepUser2)
                                        RULInfo.RULStateId_fk = 502;
                                    break;
                                case 22: //InProgress
                                    string CurrentStepUser1 = (from s in db.WorkFlowSteps
                                                               where s.WorkFlowStepID == currentStepID && s.ReciverType == "User"
                                                               select s.ReciverTypeValue
                                                    ).FirstOrDefault().ToString();
                                    if (User.Identity.Name == CurrentStepUser1)
                                        RULInfo.RULStateId_fk = 501;
                                    break;
                                default:
                                    break;
                            }
                            RULRepos.Update(RULInfo);
                        }
                        catch (Exception ex)
                        {
                            var ex1 = ex;
                        }
                        break;
                    case "3": //Eblagh
                        var EBLRepos = new Rulings.Repos.EblaghRepository();
                        Rulings.Models.VM_Eblagh EBLInfo = EBLRepos.GetByID(req.EntityID);
                        try
                        {
                            int? reqstatte = db.Requests.FirstOrDefault(x => x.RequestID == requestID).RequestResultID;
                            switch (reqstatte)
                            {
                                case 20: //Approved
                                    string CurrentStepUser2 = (from s in db.WorkFlowSteps
                                                               where s.WorkFlowStepID == currentStepID && s.ReciverType == "User"
                                                               select s.ReciverTypeValue
                                                    ).FirstOrDefault().ToString();
                                    if (User.Identity.Name == CurrentStepUser2)
                                        EBLInfo.EBLStateId_FK = 502;
                                    break;
                                case 22: //InProgress
                                    string CurrentStepUser1 = (from s in db.WorkFlowSteps
                                                               where s.WorkFlowStepID == currentStepID && s.ReciverType == "User"
                                                               select s.ReciverTypeValue
                                                    ).FirstOrDefault().ToString();
                                    if (User.Identity.Name == CurrentStepUser1)
                                        EBLInfo.EBLStateId_FK = 501;
                                    break;
                                default:
                                    break;
                            }
                            EBLRepos.Update(EBLInfo);
                        }
                        catch (Exception ex)
                        {
                            var ex1 = ex;
                        }
                        break;

                    default:
                        break;
                }

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateRequest([DataSourceRequest]DataSourceRequest request, VM_WorkDesk vm_WorkDesk)
        {
            return Json("");
        }

        [CustomAuthorize(PermissionKey = "STP:WKF:C", PermissionName = "لغو درخواست")]
        public JsonResult CancelRequest(int id, string des)
        {
            TBL_EncourageAcademic Tbl_EncourageAcademic = db.TBL_EncourageAcademic.Where(e => e.EncourageAcademicID == id).FirstOrDefault();
            Tbl_EncourageAcademic.EADeactiveDate = DateTime.Now;
            Tbl_EncourageAcademic.EADeactiveDesc = des;
            Request request = db.Requests.Where(x => x.EntityID == id).FirstOrDefault();
            request.DelDate = DateTime.Now;
            request.RequestResultID = 24;
            try
            {
                db.SaveChanges();
                return Json(new
                {
                    isOk = "1",
                    msg = "درخواست شما با موفقیت لغو شد"
                });
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new
                {
                    isOk = "0",
                    msg = "اشكال در لغو درخواست"
                });
            }
        }

        //دریافت لیست انواع کارتابل
        public JsonResult GetWorkflowTypeList()
        {
            System.Linq.IQueryable<VM_WorkFlowType> WorkFlowTypList = new WorkFlowTypeRepository().GetList();
            return Json(WorkFlowTypList, JsonRequestBehavior.AllowGet);
        }

        //جستجوی تشویق تحصیلی
        public JsonResult SearchEncourageAcademic(
                                  string EAParentNationalNo
                                , string EAParentFirstName
                                , string EAParentLastName
                                , string EAChildNationalNo
                                , string EAChildFirstName
                                , string EAChildLastName
                               )
        {
            EncourageAcademicSearchResult = new Welfare.Repos.EncourageAcademicRepository()
                                                .GetListForWorkflowSearch(EAParentNationalNo
                                                                , EAParentFirstName
                                                                , EAParentLastName
                                                                , EAChildNationalNo
                                                                , EAChildFirstName
                                                                , EAChildLastName).ToList();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //جستجوی حکم
        public JsonResult SearchRuling(
                                  string RulingPersonelNO
                                , string RulingPersonelNationalNo
                                , string RulingPersonelFirstName
                                , string RulingPersonelLastName
                                , string RulingRulId
                                , string RulingRulType
                                , string RulingRulState
                                , string RulingRulRunDate
                                , string RulingRulIssueDate
                                , string RulingEmpTypeName
                                , string RulingRulDabirDate
                                , string RulingRulDabirNO
                                )
        {
            RulingSearchResult = new Rulings.Repos.RulRepository().GetListForWorkflowSearch(
                                                                     RulingPersonelNO
                                                                    , RulingPersonelNationalNo
                                                                    , RulingPersonelFirstName
                                                                    , RulingPersonelLastName
                                                                    , RulingRulId
                                                                    , RulingRulType
                                                                    , RulingRulState
                                                                    , RulingRulRunDate
                                                                    , RulingRulIssueDate
                                                                    , RulingEmpTypeName
                                                                    , RulingRulDabirDate
                                                                    , RulingRulDabirNO).ToList();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //جستجوی ابلاغ
        public JsonResult SearchEblagh(
                                  string EblaghPersonelNO
                                , string EblaghPersonelNationalNo
                                , string EblaghPersonelFirstName
                                , string EblaghPersonelLastName
                                , string EblaghEblaghID
                                , string EblaghEBLKindId
                                , string EblaghEBLStateId
                                , string EblaghEBLRunDate
                                , string EblaghEBLIssueDate
                                , string EblaghEBLDabirDate
                                , string EblaghEBLDabirNo
                                )
        {
            EblaghSearchResult = new Rulings.Repos.EblaghRepository().GetListForWorkflowSearch(
                                                                         EblaghPersonelNO
                                                                       , EblaghPersonelNationalNo
                                                                       , EblaghPersonelFirstName
                                                                       , EblaghPersonelLastName
                                                                       , EblaghEblaghID
                                                                       , EblaghEBLKindId
                                                                       , EblaghEBLStateId
                                                                       , EblaghEBLRunDate
                                                                       , EblaghEBLIssueDate
                                                                       , EblaghEBLDabirDate
                                                                       , EblaghEBLDabirNo).ToList();
             return Json(true, JsonRequestBehavior.AllowGet);
        }
        */
    }
}
