using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FrameworkDev.Web.Models;

namespace FrameworkDev.Web.Areas.WebGIS.Models
{
    public class VM_UserLayer
    {
        public int UserLayerId { get; set; }
        public int UserId_fk { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public System.DateTime ModifyDate { get; set; }
        public ICollection<USERLAYERLINE> UserLayerLines { get;  set; }
        public ICollection<USERLAYERPOINT> UserLayerPoints { get;  set; }
        public ICollection<USERLAYERPOLYGON> UserLayerPolygons { get;  set; }
        public int UserLayerLinesCount { get;  set; }
        public int UserLayerPointsCount { get;  set; }
        public int UserLayerPolygonsCount { get;  set; }
    }
}