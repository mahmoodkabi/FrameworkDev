
using FrameworkDev.Web.Areas.Workflow.Models;
using FrameworkDev.Web.Models;
using FrameworkDev.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkDev.Web.Areas.Workflow.Repos
{
    public class WorkDeskRepository : CustomRepository<sp_WorkDesk_Result, int>
    {
        //public IList<sp_WorkDesk_Result> WorkDesk(string userName, string type, string fromDate, string toDate)
        //{
        //    // context.sp_WorkDeskWithPermission()
        //    List<sp_WorkDesk_Result> res = context.sp_WorkDesk(userName, type, (fromDate == "") ? null : fromDate, (toDate == "") ? null : toDate).ToList();
        //    return res;
        //}
        //public IList<sp_WorkDeskWithPermission_Result> WorkDeskWithPermission(string userName, string type, string fromDate, string toDate)
        //{
        //    List<sp_WorkDeskWithPermission_Result> res = context.sp_WorkDeskWithPermission(userName, type, (fromDate == "") ? null : fromDate, (toDate == "") ? null : toDate).ToList();
        //    return res;
        //}
        //public IList<sp_WorkDeskDoc_Result> WorkDeskDoc(string userName, string type, string fromDate, string toDate)
        //{
        //    List<sp_WorkDeskDoc_Result> res = context.sp_WorkDeskDoc(userName, type, (fromDate == "") ? null : fromDate, (toDate == "") ? null : toDate).ToList();
        //    return res;
        //}
        //public VM_RepoResponse<Request> CreateRequest(string userName, string formName, int entityID, bool needCheck)
        //{
        //    WorkFlowStep CurrentstepWorkflow = context.WorkFlowSteps.First(p => p.FormName == formName && p.FirstStep == true);
        //    VM_RepoResponse<Request> result = new VM_RepoResponse<Request>();
        //    VM_ErrorMessage erorr = new VM_ErrorMessage();

        //    using (var scope = new TransactionScope())
        //    {
        //        try
        //        {
        //            Request request = new Request()
        //            {
        //                EntityID = entityID,
        //                FromUserName = userName,
        //                InitialUserName = userName,
        //                ReciverType = null,
        //                ReciverTypeValue = null,
        //                RequestDate = DateTime.Now,
        //                WorkFlowStepID_fk = CurrentstepWorkflow.WorkFlowStepID
        //            };
        //            context.Requests.Add(request);
        //            context.SaveChanges();

        //            int CurrStep = CurrentstepWorkflow.WorkFlowStepID;
        //            List<sp_SendToNextLevel_Result> res = context.sp_SendToNextLevel(request.RequestID, CurrStep, "Send", userName, "", null, formName).ToList();
        //            if (res.FirstOrDefault() != null && res.FirstOrDefault().ExceptionMessage.Equals(""))
        //            {
        //                result.HaveError = false;
        //                erorr.Message = res.FirstOrDefault().Message;
        //                result.Error = null;
        //            }
        //            else
        //            {
        //                result.HaveError = true;
        //                erorr.Message = res.FirstOrDefault().ExceptionMessage;
        //                result.Error = erorr;
        //            }

        //            if (result.HaveError)
        //                scope.Dispose();
        //            else
        //                scope.Complete();

        //            return result;
        //        }
        //        catch (Exception ex)
        //        {
        //            scope.Dispose();
        //            throw;
        //        }
        //    }
        //}
        //public List<VM_RepoWorkDesk> SendToWorkFlow(string Type, string workFlowType, string userName, int requestID, int currentStepID, string sendOrMessage, string externalCondition, string paraph, string FormName)
        //{
        //    List<VM_RepoWorkDesk> result = new List<VM_RepoWorkDesk>();
        //    switch (workFlowType)
        //    {
        //        case "NextLevel":
        //            var dlResultNextLevel = context.sp_SendToNextLevel(requestID, currentStepID, sendOrMessage, userName, externalCondition, paraph, FormName);

        //            foreach (sp_SendToNextLevel_Result item in dlResultNextLevel)
        //            {
        //                VM_RepoWorkDesk repoWorkDesk = new VM_RepoWorkDesk()
        //                {
        //                    EngMessage = item.EngMessage,
        //                    ExceptionMessage = item.ExceptionMessage,
        //                    Message = item.Message
        //                };
        //                result.Add(repoWorkDesk);
        //            }
        //            return result;

        //        case "PreviousLevel":
        //            System.Data.Entity.Core.Objects.ObjectResult<sp_SendToPreviousLevel_Result> dlResultPreviousLevel;
        //            System.Data.Entity.Core.Objects.ObjectResult<sp_SendToPreviousLevelChequeOpr_Result> dlResultPreviousLevelChequeOpr;
        //            if (Type == "3")
        //            {
        //                dlResultPreviousLevel = context.sp_SendToPreviousLevel(requestID, currentStepID, sendOrMessage, userName, paraph, FormName);
        //                foreach (sp_SendToPreviousLevel_Result item in dlResultPreviousLevel)
        //                {
        //                    VM_RepoWorkDesk repoWorkDesk = new VM_RepoWorkDesk()
        //                    {
        //                        EngMessage = item.EngMessage,
        //                        ExceptionMessage = item.ExceptionMessage,
        //                        Message = item.Message
        //                    };
        //                    result.Add(repoWorkDesk);
        //                }
        //            }
        //            else if (Type == "4")
        //            {
        //                dlResultPreviousLevel = context.sp_SendToPreviousLevel(requestID, currentStepID, sendOrMessage, userName, paraph, FormName);

        //                foreach (sp_SendToPreviousLevel_Result item in dlResultPreviousLevel)
        //                {
        //                    VM_RepoWorkDesk repoWorkDesk = new VM_RepoWorkDesk()
        //                    {
        //                        EngMessage = item.EngMessage,
        //                        ExceptionMessage = item.ExceptionMessage,
        //                        Message = item.Message
        //                    };
        //                    result.Add(repoWorkDesk);
        //                }
        //            }
        //            else if (Type == "5")
        //            {
        //                dlResultPreviousLevel = context.sp_SendToPreviousLevel(requestID, currentStepID, sendOrMessage, userName, paraph, FormName);

        //                foreach (sp_SendToPreviousLevel_Result item in dlResultPreviousLevel)
        //                {
        //                    VM_RepoWorkDesk repoWorkDesk = new VM_RepoWorkDesk()
        //                    {
        //                        EngMessage = item.EngMessage,
        //                        ExceptionMessage = item.ExceptionMessage,
        //                        Message = item.Message
        //                    };
        //                    result.Add(repoWorkDesk);
        //                }

        //            }

        //            return result;

        //        case "SpecialUser":
        //            System.Data.Entity.Core.Objects.ObjectResult<sp_SendToSpecialUser_Result> dlResultSpecialUser = context.sp_SendToSpecialUser(requestID, currentStepID, sendOrMessage, userName);

        //            foreach (sp_SendToSpecialUser_Result item in dlResultSpecialUser)
        //            {
        //                VM_RepoWorkDesk repoWorkDesk = new VM_RepoWorkDesk()
        //                {
        //                    EngMessage = item.EngMessage,
        //                    ExceptionMessage = item.ExceptionMessage,
        //                    Message = item.Message
        //                };
        //                result.Add(repoWorkDesk);
        //            }

        //            return result;

        //        case "Itializer":
        //            System.Data.Entity.Core.Objects.ObjectResult<sp_SendToinItializer_Result> dlResultItializer = context.sp_SendToinItializer(requestID, currentStepID, sendOrMessage, userName, paraph);

        //            foreach (sp_SendToinItializer_Result item in dlResultItializer)
        //            {
        //                VM_RepoWorkDesk repoWorkDesk = new VM_RepoWorkDesk()
        //                {
        //                    EngMessage = item.EngMessage,
        //                    ExceptionMessage = item.ExceptionMessage,
        //                    Message = item.Message
        //                };
        //                result.Add(repoWorkDesk);
        //            }

        //            return result;

        //        default:
        //            return null;
        //    }
        //}
    }
}
