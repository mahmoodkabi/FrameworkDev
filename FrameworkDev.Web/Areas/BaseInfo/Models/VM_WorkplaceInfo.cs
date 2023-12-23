using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FrameworkDev.Web.Areas.BaseInfo.Models
{
    public class VM_WorkplaceInfo
    {
        [ReadOnly(true)]
        [DisplayName(" شناسه محل اشتغال")]
        public int WorkplaceID { get; set; }

        [DisplayName("شناسه والد ")]
        public int? WKPParentId_fk { get; set; }

        [DisplayName(" کد محل اشتغال")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد کد محل اشتغال الزامی می باشد")]
        [StringLength(20, ErrorMessage = "تعداد كاراكتر مجاز 20 كاراكتر می باشد")]
        public string WKPCode { get; set; }

        [DisplayName(" نام محل اشتغال")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد نام محل اشتغال الزامی می باشد")]
        [StringLength(200, ErrorMessage = "تعداد كاراكتر مجاز 200 كاراكتر می باشد")]
        public string WKPName { get; set; }

        [DisplayName(" شناسه ملی")]
        [StringLength(10, ErrorMessage = "تعداد كاراكتر مجاز 10 كاراكتر می باشد")]
        public string WKPNationalID { get; set; }

        [DisplayName("کد اقتصادی")]
        [StringLength(10, ErrorMessage = "تعداد كاراكتر مجاز 10 كاراكتر می باشد")]
        public string WKPEccCode { get; set; }

        [DisplayName(" شماره کارگاه بیمه ")]
        [StringLength(50, ErrorMessage = "تعداد كاراكتر مجاز 50 كاراكتر می باشد")]
        public string WKPWorkShopNo { get; set; }

        [DisplayName(" تلفن محل اشتغال")]
        [StringLength(10, ErrorMessage = "تعداد كاراكتر مجاز 10 كاراكتر می باشد")]
        public string WKPTelNo { get; set; }

        [DisplayName(" آدرس محل")]
        [StringLength(200, ErrorMessage = "تعداد كاراكتر مجاز 200 كاراكتر می باشد")]
        public string WKPAdress { get; set; }

        [DisplayName(" نوع محل اشتغال")]
        public int? WKPType { get; set; }

        [DisplayName(" فعال بودن")]
        public bool WKPActive { get; set; }

        [DisplayName(" وضعیت محل اشتغال")]
        public byte? WKPStatus { get; set; }

        [DisplayName(" زمان ایجاد محل اشتغال")]
        public DateTime WKPCreDate { get; set; }

        [DisplayName(" کاربر ایجاد کننده محل اشتغال")]
        public int WKPUserID_fk { get; set; }

        public bool HasChildren { get; set; }
    }
}
