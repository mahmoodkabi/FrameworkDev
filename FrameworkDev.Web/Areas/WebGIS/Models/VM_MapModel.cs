using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrameworkDev.Web.Areas.WebGIS.Models
{
    public class VM_MapModel
    {
        public string CenterPoint { get; set; }
        public int? Sacle { get; set; }
        public string Point_X { get; set; }
        public string Point_Y { get; set; }
        public int Buffer { get; set; }
        public string Search { get; set; }
        public string Take { get; set; }
        public string LayerName { get; set; }
        public string groupService { get; set; }

        public string Xmax { get; set; }

        public string Ymax { get; set; }

        public string Xmin { get; set; }

        public string Ymin { get; set; }

        public string Wkid { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public string Extent { get; set; }
        public string Rings { get;  set; }
    }
}