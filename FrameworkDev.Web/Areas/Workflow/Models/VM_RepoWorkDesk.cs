using System;
using System.ComponentModel;

namespace FrameworkDev.Web.Areas.Workflow.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class VM_RepoWorkDesk
    {
        /// <summary>
        /// 
        /// </summary>
        public bool HaveError { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EngMessage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ExceptionMessage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public VM_RepoWorkDesk()
        {
            HaveError = false;
        }
    }
}
