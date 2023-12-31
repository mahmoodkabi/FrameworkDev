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
    
    public partial class Place
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Place()
        {
            this.Persons = new HashSet<Person>();
            this.Persons1 = new HashSet<Person>();
            this.Places1 = new HashSet<Place>();
        }
    
        public int PlaceId { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Nullable<int> TelCode { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreDate { get; set; }
        public Nullable<int> UserId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Person> Persons { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Person> Persons1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Place> Places1 { get; set; }
        public virtual Place Place1 { get; set; }
    }
}
