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
    
    public partial class RequestSignature
    {
        public int RequestSignatureID { get; set; }
        public int RequestID { get; set; }
        public int WorkFlowID { get; set; }
        public string UserName { get; set; }
        public System.DateTime DateSignature { get; set; }
        public string Description { get; set; }
        public string FormName { get; set; }
    
        public virtual Request Request { get; set; }
    }
}
