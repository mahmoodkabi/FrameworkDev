using System.Web.Mvc;
using FrameworkDev.Web.Helpers.Authentication;
using FrameworkDev.Web.Helpers.Menus;

namespace FrameworkDev.Web.Areas.BaseInfo.Controllers
{
    [CustomAuthorize(PermissionKey = "WebGIS", PermissionName = "WebGIS")]
    public class WebGISMenuController : Helpers.CustomController
    {
        [MenuItem(Title = "WebGIS", Order = 900, IsClickable = false, CssIcon = "fa fa-american-sign-language-interpreting", SubSystems = "WebGIS")]
        public ActionResult Index()
        {
            Helpers.SubSystemsHelper.SetCurrentSubSystem(Session, "WebGIS");
            return View();
        }
    }
}
