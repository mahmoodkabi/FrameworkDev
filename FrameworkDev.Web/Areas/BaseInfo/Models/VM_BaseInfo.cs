using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FrameworkDev.Web.Areas.BaseInfo.Models
{
    public class VM_BaseInfo 
    {


        [Editable(false)]
        [Display(Name = "BaseID", ResourceType = typeof(Resources.DisplayNames))]
        public int BaseID { get; set; }

        [Display(Name = "سرشاخه")]
        public Nullable<int> ParentID { get; set; }


        [Display(Name = "BaseName", ResourceType = typeof(Resources.DisplayNames))]
        [StringLength(400, ErrorMessage = "تعداد كاراكتر مجاز 400 كاراكتر می باشد")]
        [Required(ErrorMessage = "وارد كردن نام  الزامی است")]
        public string BaseName { get; set; }


        [Display(Name = "BaseCode", ResourceType = typeof(Resources.DisplayNames))]
        [Remote("CheckCodeDuplicateError", "CodeFile", AdditionalFields = "BaseID", ErrorMessage = " {0} تکراری است")]
        [StringLength(400, ErrorMessage = "تعداد كاراكتر مجاز 400 كاراكتر می باشد")]
        //[Required(ErrorMessage = "وارد كردن کد الزامی است")]
        public string BaseCode { get; set; }


        [Display(Name = "EngName", ResourceType = typeof(Resources.DisplayNames))]
        public string EngName { get; set; }

        [Display(Name = "Description", ResourceType = typeof(Resources.DisplayNames))]
        public string Description { get; set; }




        [Display(Name = "Active", ResourceType = typeof(Resources.DisplayNames))]
        public Nullable<bool> Active { get; set; }

        [Display(Name = "ModifyDate", ResourceType = typeof(Resources.DisplayNames))]
        public System.DateTime ModifyDate { get; set; }


        [Display(Name = "UserId", ResourceType = typeof(Resources.DisplayNames))]
        public int UserId { get; set; }

        public bool HasChildren { get; set; }
        public bool? IsValue { get; set; }
        
    }
}
