using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FrameworkDev.Web.Areas.Document.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class VM_Document
    {
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("شناسه سند پیوست")]
        public int DocId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("شناسه جدول")]
        public int? TableNameId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("شناسه ردیف در جدول")]
        public int? TableRowId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("شناسه فایل در جدول")]
        public string DocHRDocId_fk { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("شرح پیوست")]
        public string DocDesc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("نوع")]
        public int? DocType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("فعالیت")]
        public byte? DocActive { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("وضعیت")]
        public byte? DocStatus { get; set; }
        public string DocDabirNo { get; set; }
        public Nullable<System.DateTime> DocDabirDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("تاریخ ایجاد")]
        public DateTime DocCreDate { get; set; }

        //Additional Data
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("نام فایل")]
        public string DocName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [RegularExpression(@"([1]\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01]))", ErrorMessage = "فرمت تاریخ صحیح نمی باشد")]
        [DisplayName("زمان پیوست")]
        public string DocCrePersianDate { get; set; }
    }
}


