using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FrameworkDev.Web.Areas.Management.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class VM_UserZones
    {
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ردیف")]
        public int UserZoneID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("شناسه كاربر")]

        // [Required(ErrorMessage = "لطفایک كاربر را انتخاب کنید")]
        public int USZUserId_fk { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("شناسه منطقه")]

        // [Required(ErrorMessage = "لطفایک منطقه را انتخاب کنید")]
        public int USZZoneId_fk { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("توضیحات")]
        [StringLength(1000, ErrorMessage = "تعداد كاراكتر مجاز 1000 كاراكتر می باشد")]
        public string USZNote { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("نوع")]
        public int? USZType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("وضعیت")]
        public byte? USZStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("وضعیت فعال")]

        // [Required(ErrorMessage = " فعال بودن اجباری است")]

        public bool USZActive { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("تاریخ ثبت")]

        // [Required(ErrorMessage = " تاریخ ثبت اجباری است")]
        public DateTime USZCreDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("کد کاربر ثبت کننده")]
        public int USZCreateUserId_fk { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("نام کاربر ثبت کننده")]
        public int USZUserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("نام منطقه")]
        public String ZONName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("شناسه منطقه منطقه")]
        public int ZoneID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("نوع")]
        public int? ZONType { get; set; }
        public string Code { get;  set; }
    }
}
