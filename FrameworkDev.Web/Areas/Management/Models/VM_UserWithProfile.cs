using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FrameworkDev.Web.Areas.Management.Models
{
    public class VM_UserWithProfile : VM_User
    {
        [StringLength(11, ErrorMessage = "تلفن منزل اشتباه وارد شده است", MinimumLength = 11)]
        [DisplayName("تلفن منزل")]
        public string CTZTelNo { get; set; }

        [DisplayName("تلفن همراه")]
        [Required(ErrorMessage = "شماره همراه را وارد نمایید")]
        [StringLength(11, ErrorMessage = "شماره همراه اشتباه وارد شده است", MinimumLength = 11)]
        [RegularExpression("([0-9]+)", ErrorMessage = "شماره همراه اشتباه وارد شده است")]
        [DisplayFormat(NullDisplayText = "09XX-XXX-XXXX")]
        public string CTZMobileNo { get; set; }

        [DisplayName("آدرس")]
        [Required(ErrorMessage = "لطفا آدرس را وارد نمایید")]
        [StringLength(200, ErrorMessage = "تعداد كاراكتر مجاز 200 كاراكتر می باشد")]
        public string CTZAddressHome { get; set; }


        [DisplayName("کدملی")]
        [Required(ErrorMessage = "لطفاکد ملی وارد کنید")]
        [StringLength(10, ErrorMessage = "كد ملی اشتباه وارد شده است", MinimumLength = 10)]
        public string CTZNationalNo { get; set; }
    }
}
