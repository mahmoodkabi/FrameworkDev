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
    
    public partial class RequestParaph
    {
        public int RequestParaphID { get; set; }
        public int RequestID_fk { get; set; }
        public int WorkFlowID { get; set; }
        public string UserName { get; set; }
        public string ParaphText { get; set; }
        public System.DateTime ParaphDate { get; set; }
        public Nullable<bool> IsSeen { get; set; }
    
        public virtual Request Request { get; set; }
    }
}
