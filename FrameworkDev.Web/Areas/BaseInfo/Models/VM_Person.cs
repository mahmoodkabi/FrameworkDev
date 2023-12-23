using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FrameworkDev.Web.Models;


namespace FrameworkDev.Web.Areas.BaseInfo.Models
{
    public class VM_Person
    {
        public VM_Person()
        {
            Message = new VM_Message();
        }
        

        
        [DisplayName("شناسه شخص ")]
        [Display(Name = "PersonId", ResourceType = typeof(Resources.DisplayNames))]
        public int PersonId { get; set; }


        [Display(Name = "PersonMelliCode", ResourceType = typeof(Resources.DisplayNames))]
        [DisplayName("کد ملی ")]
        public string PersonMelliCode { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Resources.DisplayNames))]
        [DisplayName("نام")]
        [Required(ErrorMessage = "وارد كردن نام الزامی است")]
        public string Name { get; set; }


        [Display(Name = "Family", ResourceType = typeof(Resources.DisplayNames))]
        [DisplayName("نام خانوادگی")]
        [Required(ErrorMessage = "وارد كردن نام خانوادگی الزامی است")]
        public string Family { get; set; }


        [Display(Name = "FatherName", ResourceType = typeof(Resources.DisplayNames))]
        [DisplayName("نام پدر")]
        public string FatherName { get; set; }


        [Display(Name = "Sex", ResourceType = typeof(Resources.DisplayNames))]
        [UIHint("IGStringBooleanSex")]
        [DisplayName("جنسیت")]
        public Nullable<bool> Sex { get; set; }


        [Display(Name = "IDNo", ResourceType = typeof(Resources.DisplayNames))]
        [DisplayName("شماره شناسنامه")]
        public string IDNo { get; set; }


        [Display(Name = "BirthDate", ResourceType = typeof(Resources.DisplayNames))]
        [DisplayName("تاریخ تولد")]
        [DataType(DataType.Date)]
      //  [UIHint("GridPersianDateTime")]
        [RegularExpression(@"([1]\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01]))", ErrorMessage = "فرمت تاریخ صحیح نمی باشد")]
        public string BirthDate { get; set; }


        [DisplayName("محل تولد")]
        [Display(Name = "BirthPlcId", ResourceType = typeof(Resources.DisplayNames))]
        public int? BirthPlcId { get; set; }

        [Display(Name = "ExportPlcId", ResourceType = typeof(Resources.DisplayNames))]
        [DisplayName("محل صدور شناسنامه")]
        public int? ExportPlcId { get; set; }


        [Display(Name = "TelNo", ResourceType = typeof(Resources.DisplayNames))]
        [DisplayName("شماره تلفن")]
        [StringLength(11, ErrorMessage = "تعداد كاراكتر مجاز 11 كاراكتر می باشد", MinimumLength = 11)]
        public string TelNo { get; set; }

        [Display(Name = "MobileNo", ResourceType = typeof(Resources.DisplayNames))]
        [DisplayName("شماره همراه")]
        [Required(ErrorMessage = "وارد كردن شماره همراه الزامی است")]
        [StringLength(11, ErrorMessage = "تعداد كاراكتر مجاز 11 كاراكتر می باشد", MinimumLength = 11)]
        public string MobileNo { get; set; }

        [Display(Name = "Address", ResourceType = typeof(Resources.DisplayNames))]
        [DisplayName("آدرس محل سكونت")]
        public string Address { get; set; }


        [Display(Name = "PostCode", ResourceType = typeof(Resources.DisplayNames))]
        [DisplayName("کد پستی")]
        [StringLength(10, ErrorMessage = "تعداد كاراكتر مجاز 10 كاراكتر می باشد", MinimumLength = 10)]
        public string PostCode { get; set; }

        [Display(Name = "IsAccountOwner", ResourceType = typeof(Resources.DisplayNames))]
        [DisplayName("صاحب حساب")]
        public int IsAccountOwner { get; set; }



        [Display(Name = "CompanyName", ResourceType = typeof(Resources.DisplayNames))]
        [DisplayName("نام شركت ")]
        [Required(ErrorMessage = "وارد كردن نام شرکت الزامی است")]
        public string CompanyName { get; set; }


        [Display(Name = "CompanyMelliCode", ResourceType = typeof(Resources.DisplayNames))]
        [DisplayName("شناسه ملی شرکت")]
        [Required(ErrorMessage = "وارد كردن شناسه ملی شرکت الزامی است")]
        [StringLength(11, ErrorMessage = "تعداد كاراكتر مجاز 11 كاراكتر می باشد", MinimumLength = 11)]
        public string CompanyMelliCode { get; set; }


        [Display(Name = "NameManager", ResourceType = typeof(Resources.DisplayNames))]
        [DisplayName("نام مدیرعامل")]
        public string NameManager { get; set; }


        [Display(Name = "FamilyManager", ResourceType = typeof(Resources.DisplayNames))]
        [DisplayName("نام خانوادگی مدیرعامل")]
        public string FamilyManager { get; set; }


        [Display(Name = "IDNoCompany", ResourceType = typeof(Resources.DisplayNames))]
        [DisplayName("شماره ثبت")]
        [Required(ErrorMessage = "وارد كردن شماره ثبت شرکت الزامی است")]
        public string IDNoCompany { get; set; }


        [Display(Name = "BirthDateCompany", ResourceType = typeof(Resources.DisplayNames))]
        [DisplayName("تاریخ ثبت")]
        [Required(ErrorMessage = "وارد كردن تاریخ ثبت شرکت الزامی است")]
        [RegularExpression(@"([1]\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01]))", ErrorMessage = "فرمت تاریخ صحیح نمی باشد")]

        public string BirthDateCompany { get; set; }

        [Display(Name = "ExportPlcIdCompany", ResourceType = typeof(Resources.DisplayNames))]
        [DisplayName("محل ثبت")]
        [Required(ErrorMessage = "وارد كردن محل ثبت شرکت الزامی است")]
        public int? ExportPlcIdCompany { get; set; }


        [Display(Name = "TelNoCompany", ResourceType = typeof(Resources.DisplayNames))]
        [DisplayName("شماره تلفن شرکت")]
        [Required(ErrorMessage = "وارد كردن شماره تلفن شرکت الزامی است")]
        public string TelNoCompany { get; set; }


        [Display(Name = "MobileNoCompany", ResourceType = typeof(Resources.DisplayNames))]
        [DisplayName("شماره همراه")]
        public string MobileNoCompany { get; set; }


        [Display(Name = "AddressCompany", ResourceType = typeof(Resources.DisplayNames))]
        [DisplayName("آدرس محل شركت")]
        public string AddressCompany { get; set; }


        [Display(Name = "PostCodeCompany", ResourceType = typeof(Resources.DisplayNames))]
        [DisplayName("کد پستی شرکت")]
        public string PostCodeCompany { get; set; }


        [Display(Name = "IsAccountOwnerCompany", ResourceType = typeof(Resources.DisplayNames))]
        [DisplayName("صاحب حساب")]
        public int IsAccountOwnerCompany { get; set; }


        [Display(Name = "PersonType", ResourceType = typeof(Resources.DisplayNames))]
        [DisplayName("نوع شخص   ")]
        public bool PersonType { get; set; }


        [Display(Name = "CreDate", ResourceType = typeof(Resources.DisplayNames))]
        [DisplayName("تاریخ ایجاد   ")]
        public System.DateTime CreDate { get; set; }

   
        [Display(Name = "UserId", ResourceType = typeof(Resources.DisplayNames))]
        [DisplayName("شناسه كاربر   ")]
        public int UserId { get; set; }
        

        public VM_Message Message { get; set; }

        public string FullPersonName { get;  set; }
        public int AccountOwnerId { get;  set; }

        [DisplayName("کد موقت ")]
        public string TempMelliCode { get; set; }

        public string PersonRadio { get; set; }
        
    }
   

}