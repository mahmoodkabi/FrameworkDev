using System;
using System.ComponentModel;
namespace FrameworkDev.Web.Areas.Workflow.Models
{
    public partial class VM_RequestParaph
    {
        [DisplayName("شناسه")]
        public int RequestParaphID { get; set; }

        [DisplayName("شناسه درخواست")]
        public int RequestID_fk { get; set; }

        [DisplayName("شناسه گردش")]
        public int WorkFlowID { get; set; }

        [DisplayName("کاربر")]
        public string UserName { get; set; }

        [DisplayName("متن پاراف")]
        public string ParaphText { get; set; }

        [DisplayName("تاریخ پاراف")]
        public System.DateTime ParaphDate { get; set; }

        [DisplayName("دیده شده")]
        public Nullable<bool> IsSeen { get; set; }

        [DisplayName("شناسه نوع گردش")]
        public Nullable<int> WorkFlowTypeID_fk { get; set; }

    }
}
