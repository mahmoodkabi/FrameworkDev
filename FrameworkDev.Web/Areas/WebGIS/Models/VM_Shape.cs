using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrameworkDev.Web.Areas.WebGIS.Models
{
    public class VM_ShapeFeature
    {
        public string Type { get; set; }
        public int UserLayerShapeId { get; set; }
        public string FeatureName { get; set; }
        public string FeatureValue { get; set; }
        public bool IsNewFeature { get; set; }
        
    }
}