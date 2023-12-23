using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FrameworkDev.Web.Areas.Workflow.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class VM_WorkDeskRequestParaph
    {
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("شناسه")]
        public int RequestParaphID { get; set; }      
        /// <summary>
        /// 
        /// </summary>
        public int RequestID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int WorkFlowID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ایجادكننده")]
        [Editable(false)]
        public string ParapherFullName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("متن پاراف")]
        public string ParaphText { get; set; }        
        /// <summary>
        /// 
        /// </summary>
        public DateTime ParaphDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("تاریخ پاراف")]
        [Editable(false)]
        public string Str_ParaphDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? IsSeen { get; set; }

    }
}