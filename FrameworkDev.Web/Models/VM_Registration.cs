using System;
using System.ComponentModel.DataAnnotations;

namespace FrameworkDev.Web.Models
{
    public class VM_Registration
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "*")]
        [Display(Name = "نام کاربری")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*")]
        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*")]
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        public Guid ActivationCode { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "تکرار رمز عبور")]
        [Compare("Password", ErrorMessage = "*")]
        public string ConfirmPassword { get; set; }
    }
}
