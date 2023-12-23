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
    
    public partial class WorkFlowStep
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WorkFlowStep()
        {
            this.Requests = new HashSet<Request>();
        }
    
        public int WorkFlowStepID { get; set; }
        public Nullable<int> WorkFlowTypeID_fk { get; set; }
        public string FormName { get; set; }
        public string StepName { get; set; }
        public string EngName { get; set; }
        public Nullable<int> NextStepID { get; set; }
        public Nullable<int> NextStepID1 { get; set; }
        public Nullable<int> PreviousStepID { get; set; }
        public string ReciverType { get; set; }
        public string ReciverTypeValue { get; set; }
        public string ReciverOtherType { get; set; }
        public string ReciverOtherTypeValue { get; set; }
        public Nullable<bool> IsSendToSpecialUser { get; set; }
        public string SpecialUserName { get; set; }
        public Nullable<bool> FirstStep { get; set; }
        public Nullable<bool> EndStep { get; set; }
        public Nullable<bool> btnSave { get; set; }
        public Nullable<bool> btnDelete { get; set; }
        public Nullable<bool> btnSendToNextStep { get; set; }
        public Nullable<bool> btnSendToPreviosStep { get; set; }
        public Nullable<bool> btnSendToInitializer { get; set; }
        public Nullable<bool> IsCondition { get; set; }
        public string LeftCondition { get; set; }
        public string OperatorCondition { get; set; }
        public string RightCondition { get; set; }
        public Nullable<int> RequestResultIDNext { get; set; }
        public Nullable<int> RequestResultIDPreviose { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Request> Requests { get; set; }
        public virtual WorkFlowType WorkFlowType { get; set; }
    }
}