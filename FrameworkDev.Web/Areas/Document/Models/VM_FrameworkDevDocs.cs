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
    public class VM_FrameworkDevDocs
    {
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("شناسه یکتای ردیف")]
        public Guid stream_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("محتوی فایل")]
        public byte[] file_stream { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("نام فایل")]
        public string name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("پوشه/فایل")]
        public bool is_directory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("محتوی فایل")]
        public string unc_path { get; set; }
        
    }
}