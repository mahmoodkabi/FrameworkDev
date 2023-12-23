using FrameworkDev.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrameworkDev.Web.Areas.WebGIS.Models
{
    public class VM_Bookmark
    {
        //public VM_Bookmark()
        //{
        //    Paging = new VM_Paging();
        //}

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

        //public VM_Paging Paging;
    }
}