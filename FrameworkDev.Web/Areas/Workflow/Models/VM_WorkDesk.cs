
using System;
using System.Collections.Generic;
using System.ComponentModel;
namespace FrameworkDev.Web.Areas.Workflow.Models
{


    /// <summary>
    /// 
    /// </summary>
    public partial class VM_WorkDesk
    {
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("شناسایی")]
        public long? ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("نوع گردش کار")]
        public int? WorkFlowTypeID { get; set; }

        [DisplayName("شناسه بدهی")]
        public int TreDebitId { get; set; }

        [DisplayName("شناسه پرونده")]
        public int TreFileId { get; set; }


        [DisplayName("مبلغ نقد")]
        public decimal? CashAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("شماره موجودیت")]
        public int? EntityID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("شماره درخواست")]
        public int? RequestID { get; set; }

        [DisplayName("توضیح")]
        public string Description { get; set; }



        /// <summary>
        /// 
        /// </summary>
        [DisplayName("مرحله گردش كار")]
        public int? WorkFlowStepID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("تاریخ درخواست میلادی ")]
        public DateTime? RequestDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("تاریخ درخواست")]
        public string RequestDateJalali { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("نام كاربری ثبت كننده ")]
        public string InitialUserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ثبت كننده")]
        public string FullNameInitialUserName { get; set; }

        [DisplayName("ثبت كننده")]
        public string fullnameini { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("نام كاربری فرستنده")]
        public string FromUserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("فرستنده")]
        public string FullNameFromUserName { get; set; }


        [DisplayName("فرستنده")]
        public string fullnamefrom { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DisplayName(" نتیجه درخواست")]
        public string RequestResultName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName(" شناسه نتیجه درخواست")]
        public int? RequestResultID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("تاریخ تایید میلادی")]
        public DateTime? FinalConfirmationDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("تاریخ تایید")]
        public string FinalConfirmationDateJalali { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("گیرنده")]
        public string FullNameReciverType { get; set; }        

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("تاریخ ثبت درخواست")]
        public string PSTRegistrationDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("تاریخ ثبت درخواست")]
        public string PSTRegistrationDateFa { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool PSTChangetimeOver { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("تاریخ ورود")]
        public string TEVEnterDate { get; set; }

        public ICollection<VM_WorkDeskRequestParaph> RequestParaph { get; set; }

        public VM_WorkFlowType WorkFlowType { get; set; }

        [DisplayName("وضعیت مجوز")]
        public string PermissionStatusName { get; set; }

        [DisplayName("تاریخ تغییر وضعیت مجوز")]
        public string StatusDate { get; set; }


        [DisplayName("شماره پرونده")]
        public string FileNo { get; set; }

        [DisplayName("نوع مجوز")]
        public string PermissionType { get; set; }

        [DisplayName("نوع مجوز")]
        public int entitytype { get; set; }

        [DisplayName("نام منطقه")]
        public string ZName { get; set; }

        [DisplayName("شناسه منطقه")]
        public int ZoneId { get; set; }

    }
}
