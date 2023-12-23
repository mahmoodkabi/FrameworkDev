using System.Web.Mvc;
using FrameworkDev.Web.Helpers.Authentication;

namespace FrameworkDev.Web.Areas.Workflow.Controllers
{
    public class WFController : Helpers.CustomController
    {
        [CustomAuthorize(PermissionKey = "WFM", PermissionName = "كارتابل")]
        public ActionResult Index()
        {
            return View("~\\Area\\Workflow\\Views\\WorkDesk\\Index.cshtml");
        }
    }
}