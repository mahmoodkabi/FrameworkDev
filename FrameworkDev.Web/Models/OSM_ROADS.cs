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
    
    public partial class OSM_ROADS
    {
        public int OBJECTID { get; set; }
        public string osm_id { get; set; }
        public Nullable<short> code { get; set; }
        public string fclass { get; set; }
        public string name { get; set; }
        public string @ref { get; set; }
        public string oneway { get; set; }
        public Nullable<short> maxspeed { get; set; }
        public Nullable<decimal> layer { get; set; }
        public string bridge { get; set; }
        public string tunnel { get; set; }
        public System.Data.Entity.Spatial.DbGeometry Shape { get; set; }
        public string CountyOsmId { get; set; }
        public string CityOsmId { get; set; }
    }
}
