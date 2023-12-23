
using System;
using System.Collections.Generic;
using System.ComponentModel;
namespace FrameworkDev.Web.Areas.Workflow.Models
{

    public partial class VM_WorkDeskDoc
    {
        [DisplayName("شناسایی")]
        public long? ID { get; set; }

        /// <summary>
        [DisplayName("نوع گردش کار")]
        public int? WorkFlowTypeID { get; set; }

        [DisplayName("شناسه بدهی")]
        public int TreDebitId { get; set; }

        [DisplayName("شناسه پرونده")]
        public int TreFileId { get; set; }

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

        
         

        [DisplayName("نام منطقه")]
        public string ZName { get; set; }


        [DisplayName("شناسه منطقه")]
        public int ZoneId { get; set; }


        [DisplayName("توضیحات سند")]
        public string  DochNote { get; set; }


        [DisplayName("شناسه نوع سند ")]
        public int ActionCodeId { get; set; }

        [DisplayName("نوع سند")]
        public string  ActionCodeName { get; set; }

        [DisplayName("تاریخ ایجاد سند")]
        public DateTime? CreDate { get; set; }


        [DisplayName("تاریخ ایجاد سند")]
        public string CreDatestr { get; set; }

        [DisplayName("تاریخ سند")]
        public string DocHDate { get;  set; }
    }
}
