using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAnnotationsExtensions;
using FrameworkDev.Web.Resources;

namespace FrameworkDev.Web.Areas.Management.Models
{
    public class VM_User
    {
        [Key]
        [ReadOnly(true)]
        [Display(Name = "UserId", ResourceType = typeof(DisplayNames))]
        public int UserId { get; set; }

        [ReadOnly(true)]
        [Display(Name = "UserName", ResourceType = typeof(DisplayNames))]
        public string UserName { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(DisplayNames))]
        public string FirstName { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(DisplayNames))]
        public string LastName { get; set; }

        [Display(Name = "Email", ResourceType = typeof(DisplayNames))]
       // [Email]
        public string Email { get; set; }

        [Display(Name = "Password", ResourceType = typeof(DisplayNames))]
        [Required(ErrorMessage = "رمز عبور را وارد کنید.")]
        [StringLength(255, ErrorMessage = "باید بین 5 تا 255 کاراکتر باشد.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [PasswordPropertyText(true)]
        public string Password { get; set; }

        [Display(Name = "PasswordConfirm", ResourceType = typeof(DisplayNames))]
        [Required(ErrorMessage = "تکرار رمز عبور را وارد کنید.")]
        [StringLength(255, ErrorMessage = "باید بین 5 تا 255 کاراکتر باشد.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "تکرار رمز عبور باید با رمز عبور یکسان باشد.")]
        [NotMapped]
        [PasswordPropertyText(true)]
        public string PasswordConfirm { get; set; }

        [DisplayName("فعال بودن")]
        public bool IsActive { get; set; }

        [DisplayName("دسترسی ها")]
        public List<string> Permissions { get; set; }

        [Display(Name = "Roles", ResourceType = typeof(DisplayNames))]
        public List<int> Roles { get; set; }

        [DisplayName("پروفایل")]
        public Dictionary<string, string> Profiles { get; set; }

        [DisplayName("کد فعالسازی")]
        public Guid? ActivationCode { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*")]
        public string FullName { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "*")]
        [Display(Name = "تلفن همراه")]
        [StringLength(11, MinimumLength = 11)]
        public string Mobile { get; set; }
        public bool? RememberMe { get;  set; }

        [Display(Name = "RolePermissions", ResourceType = typeof(DisplayNames))]
        public List<int> RolePermissions { get; set; }
    }
}
