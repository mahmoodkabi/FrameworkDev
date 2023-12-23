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
    public class VM_TableName
    {

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("شناسه جدول")]
        public int TablesNameID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("جدول والد")]
        public int? TBNParentID_fk { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("جدول زیرمجموعه")]
        public int? TBSubSystemID_fK { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("نام جدول")]
        public string TBNName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("فیلد کلید جدول")]
        public string TBNPrimaryKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("توضیحات")]
        public string TBNNote { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("نوع")]
        public int? TBNType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("فعال")]
        public byte TBNActive { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("وضعیت")]
        public byte? TBNStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("تاریخ ایجاد")]
        public DateTime TBNCreDate { get; set; }

    }
}