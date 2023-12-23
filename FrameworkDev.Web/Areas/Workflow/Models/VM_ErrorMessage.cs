using System;

namespace FrameworkDev.Web.Areas.Workflow.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class VM_ErrorMessage
    {
        /// <summary>
        /// 
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Exception Exp { get; set; }
    }
}
