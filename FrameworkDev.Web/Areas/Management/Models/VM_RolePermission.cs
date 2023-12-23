using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrameworkDev.Web.Areas.Management.Models
{
    /// <summary>
    /// 
    /// </summary>
    public partial class VM_RolePermission
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int RPKId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PermissionKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public bool @checked { get; set; }
    }
}
