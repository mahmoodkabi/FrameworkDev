using System.ComponentModel;
namespace FrameworkDev.Web.Areas.Workflow.Models
{
    public partial class VM_WorkFlowType
    {
        [DisplayName("شناسه")]
        public int WorkFlowTypeID { get; set; }

        [DisplayName("نام لاتین نوع گردش کار")]
        public string Name { get; set; }

        [DisplayName("نوع درخواست")]
        public string FaName { get; set; }

    }
}
