namespace FrameworkDev.Web.Areas.Workflow.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class VM_RepoResponse<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public T Entity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HaveError { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public VM_ErrorMessage Error { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public VM_RepoResponse()
        {
            HaveError = false;
        }
    }
}
