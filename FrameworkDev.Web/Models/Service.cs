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
    
    public partial class Service
    {
        public int ArcServiceID { get; set; }
        public int ArcServiceParentID { get; set; }
        public string ServiceName { get; set; }
        public string ServiceAddress { get; set; }
        public string GroupService { get; set; }
        public string ServiceDescription { get; set; }
        public string Description { get; set; }
        public Nullable<bool> Active { get; set; }
    }
}
