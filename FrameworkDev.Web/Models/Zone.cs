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
    
    public partial class Zone
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Zone()
        {
            this.TBL_UserZones = new HashSet<TBL_UserZones>();
            this.Zones1 = new HashSet<Zone>();
        }
    
        public int ZoneId { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string WorkShopNo { get; set; }
        public string AccNo { get; set; }
        public string Address { get; set; }
        public string TelNo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBL_UserZones> TBL_UserZones { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Zone> Zones1 { get; set; }
        public virtual Zone Zone1 { get; set; }
    }
}
