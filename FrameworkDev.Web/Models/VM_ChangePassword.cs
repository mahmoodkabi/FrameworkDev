using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrameworkDev.Web.Models
{
    public class VM_ChangePassword
    {
        [Display(Name = "CurrentPassword", ResourceType = typeof(Resources.DisplayNames))]
        [Required(ErrorMessage = "رمز عبور جاری مورد نیاز است!")]
        [NotMapped]
        public string CurrentPassword { get; set; }

        [Display(Name = "Password", ResourceType = typeof(Resources.DisplayNames))]
        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "رمز عبور باید بین 5 تا 255 کاراکتر باشد!", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [NotMapped]
        public string Password { get; set; }

        [Display(Name = "PasswordConfirm", ResourceType = typeof(Resources.DisplayNames))]
        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(255, ErrorMessage = "رمز عبور باید بین 5 تا 255 کاراکتر باشد!", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Must be same as Password")]
        [NotMapped]
        public string PasswordConfirm { get; set; }
    }
}
