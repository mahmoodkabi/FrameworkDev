//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FrameworkDev.Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BookMark
    {
        public int BookMarkID { get; set; }
        public int UserID_fk { get; set; }
        public string Xmin { get; set; }
        public string Ymin { get; set; }
        public string Xmax { get; set; }
        public string Ymax { get; set; }
        public string Wkid { get; set; }
        public string LatestWkid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string Extent { get; set; }
        public Nullable<int> Sacle { get; set; }
        public string CenterPoint { get; set; }
        public string Rings { get; set; }
        public string Note { get; set; }
        public Nullable<int> UserId { get; set; }
    }
}