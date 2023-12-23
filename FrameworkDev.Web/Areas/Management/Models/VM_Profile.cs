namespace FrameworkDev.Web.Areas.Management.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public partial class VM_Profile
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Display(Name = "UserId", ResourceType = typeof(Resources.DisplayNames))]
        [ReadOnly(true)]
        public int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Display(Name = "Key", ResourceType = typeof(Resources.DisplayNames))]
        public string Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Value", ResourceType = typeof(Resources.DisplayNames))]
        public string Value { get; set; }
    }
}
