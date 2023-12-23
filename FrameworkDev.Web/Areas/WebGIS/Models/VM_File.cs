using FrameworkDev.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace FrameworkDev.Web.Areas.WebGIS.Models
{
    public class VM_File
    {
        public HttpPostedFileBase File { set; get; }
        public DateTime ModifyDate { get;  set; }
        public int UserId { get;  set; }
        public string FileName { get;  set; }

        public List<VM_GeometryType> FileGeometries { get; set; }
    }


    public class VM_GeometryType
    {

        public string StringFileGeometry { get; set; }
        public DbGeometry FileGeometry { get; set; }
        public string Type { get; set; }
        public List<VM_Feature> Features { get; set; }

        public VM_Message Message { get; set; }
        public int id { get;  set; }
    }

    public class VM_Feature
    {
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        public int Id { get;  set; }
    }
}


