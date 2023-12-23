using AutoMapper;
using FrameworkDev.Web.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FrameworkDev.Web.Areas.Management.Models
{
    /// <summary>
    ///
    /// </summary>
    public partial class VM_Role
    {
        /// <summary>
        ///
        /// </summary>
        [Key]
        [Display(Name = "RoleId", ResourceType = typeof(Resources.DisplayNames))]
        [ReadOnly(true)]
        public int RoleId { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "RoleName", ResourceType = typeof(Resources.DisplayNames))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RoleNameRequired", ErrorMessageResourceType = typeof(Resources.Errors))]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessageResourceName = "RoleNameInvalidChars", ErrorMessageResourceType = typeof(Resources.Errors))]
        public string RoleName { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "RoleNameFa", ResourceType = typeof(Resources.DisplayNames))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RoleNameFaRequired", ErrorMessageResourceType = typeof(Resources.Errors))]
        [RegularExpression("^[ابپتثجچحخدذإرزژسشصضطظعغفقکكگلمنوهیی 1234567890]*$", ErrorMessageResourceName = "RoleNameFaInvalidChars", ErrorMessageResourceType = typeof(Resources.Errors))]
        public string RoleNameFa { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class VM2M_Role_Converter : ITypeConverter<VM_Role, Role>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Role Convert(VM_Role source, Role destination, ResolutionContext context)
        {
            Role result = new Role
            {
                RoleId = source.RoleId,
                RoleName = source.RoleName,
                RoleNameFa = source.RoleNameFa
            };

            return result;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class M2VM_Role_Converter : ITypeConverter<Role, VM_Role>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public VM_Role Convert(Role source, VM_Role destination, ResolutionContext context)
        {
            VM_Role result = new VM_Role
            {
                RoleId = source.RoleId,
                RoleName = source.RoleName,
                RoleNameFa = source.RoleNameFa
            };

            return result;
        }
    }
}
