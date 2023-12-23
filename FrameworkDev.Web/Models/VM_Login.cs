using System.ComponentModel.DataAnnotations;

namespace FrameworkDev.Web.Models
{
    public class VM_Login
    {
        [Display(Name = "نام کاربری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*")]
        public string UserName { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*")]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر داشته باش")]
        public bool? RememberMe { get; set; }

        [Display(Name = "نام")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*")]
        public string LastName { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "تکرار رمز عبور")]
        [Compare("Password", ErrorMessage = "*")]
        public string ConfirmPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "*")]
        [Display(Name = "تلفن همراه")]
        [StringLength(11, MinimumLength = 11)]
        public string Mobile { get; set; }
    }
}
