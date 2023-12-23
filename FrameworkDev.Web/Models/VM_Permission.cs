using System.ComponentModel.DataAnnotations;

namespace FrameworkDev.Web.Models
{
    public class VM_Permission
    {
        [Key]
        public string PermissionKey { get; set; }

        public string PermissionName { get; set; }
    }

    public class VM_PermissionTree : VM_Permission
    {
        public string ParentKey { get; set; }

        public bool HasChildren { get; set; }

        public bool @checked { get; set; }
    }
}
