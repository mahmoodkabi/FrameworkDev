using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrameworkDev.Web.Areas.WebGIS.Models
{
    public class VM_SearchResult
    {
        public Int64? RowId { get; set; }
        public string Url { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
        public string AttributeValue { get; set; }
        public int? LayerId { get; set; }
        public string typeShow { get; set; }
        public string Description { get;  set; }
        public string AttributeName { get;  set; }
        public int? UserLayerShapeId { get;  set; }
    }
}