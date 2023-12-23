using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FrameworkDev.Web.Areas.BaseInfo.Models
{
    public class VM_PlaceInfo
    {
        [ReadOnly(true)]
        [DisplayName(" شناسه محل")]
        public int PlaceId { get; set; }

        [DisplayName(" شناسه والد")]
        public int? ParentId { get; set; }

        [DisplayName(" نام محل")]
        [Required(ErrorMessage = "نام محل اجباری است")]
        [StringLength(200, ErrorMessage = "تعداد كاراكتر مجاز 200 كاراكتر می باشد")]
        public string Name { get; set; }

        [DisplayName(" کد محل")]
      //  [Remote("CheckPlaceCodeDuplicate", "Place", ErrorMessage = " {0} تکراری است")]
        [StringLength(50, ErrorMessage = "تعداد كاراكتر مجاز 50 كاراكتر می باشد")]
        public string Code { get; set; }

        [DisplayName(" طول جغرافیایی")]
        [StringLength(50, ErrorMessage = "تعداد كاراكتر مجاز 50 كاراكتر می باشد")]
        public string PLCX { get; set; }

        [DisplayName(" عرض جغرافیایی")]
        [StringLength(50, ErrorMessage = "تعداد كاراكتر مجاز 50 كاراكتر می باشد")]
        public string PLCY { get; set; }

        [DisplayName(" پیش شماره تلفن")]
        public int? TelCode { get; set; }

        [DisplayName(" ضریب منطقه")]
        public decimal? PLCCoefficient { get; set; }

        [DisplayName(" فوق‌العاده مکان")]
        public int? PLCExtra { get; set; }

        [DisplayName(" محرومیت مکانی")]
        public int? PLCPrivation { get; set; }

        [DisplayName(" شرایط آب و هوا")]
        public int? PLCWeather { get; set; }

        [DisplayName(" دوری راه")]
        public int? PLCAway { get; set; }

        [DisplayName(" نقاط مرزی")]
        public int? PLCBoundary { get; set; }

        [StringLength(1000, ErrorMessage = "تعداد كاراكتر مجاز 1000 كاراكتر می باشد")]
        [DisplayName(" توضیحات")]
        public string Note { get; set; }

        [DisplayName(" نوع منطقه")]
        public int? PLCType { get; set; }

        [DisplayName("فعال/غیرفعال")]
        [Required(ErrorMessage = "وضعیت محل  اجباری است")]
        public bool PLCActive { get; set; }

        [DisplayName(" وضعیت")]
        public byte? PLCStatus { get; set; }

        [DisplayName("تاریخ ایجاد")]

        public DateTime CreDate { get; set; }

        [DisplayName("کاربرایجادکننده")]

        public int UserId { get; set; }

        public bool HasChildren { get; set; }
    }
}
