using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FrameworkDev.Web.Areas.BaseInfo.Models
{
    public class VM_ZoneInfo
    {
        [ReadOnly(true)]
        [Display(Name = "ZoneId", ResourceType = typeof(Resources.DisplayNames))]
        public int ZoneId { get; set; }

        [Display(Name = "ParentId", ResourceType = typeof(Resources.DisplayNames))]
        public Nullable<int> ParentId { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Resources.DisplayNames))]
        [StringLength(200, ErrorMessage = "تعداد كاراكتر مجاز 200 كاراكتر می باشد")]
        [Required(ErrorMessage = "وارد كردن نام منطقه الزامی است")]
        public string Name { get; set; }


        [Display(Name = "Code", ResourceType = typeof(Resources.DisplayNames))]
        [StringLength(2, ErrorMessage = "کد نمیتواند بیشتر از دو رقم باشد", MinimumLength = 2)]
        [Remote("CheckCodeDuplicate", "Zones", ErrorMessage =" {0} تکراری است")]
        public string Code { get; set; }


        [Display(Name = "WorkShopNo", ResourceType = typeof(Resources.DisplayNames))]
        [StringLength(50, ErrorMessage = "تعداد كاراكتر مجاز 50 كاراكتر می باشد")]
        
        public string WorkShopNo { get; set; }


        [StringLength(1000, ErrorMessage = "تعداد كاراكتر مجاز 1000 كاراكتر می باشد")]
        [Required(ErrorMessage = "وارد كردن آدرس الزامی است")]
        [Display(Name = "Address", ResourceType = typeof(Resources.DisplayNames))]
        public string Address { get; set; }

        [Display(Name = "TelNo", ResourceType = typeof(Resources.DisplayNames))]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "وارد كردن شماره تلفن الزامی است")]
        [StringLength(11, ErrorMessage = "تعداد كاراكتر مجاز 11 كاراكتر می باشد", MinimumLength = 11)]
        public string TelNo { get; set; }



      

        public bool HasChildren { get; set; }


        public bool IsSelected { get; set; }
    }
}
