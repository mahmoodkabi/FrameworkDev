using System.Web.Mvc;
using FrameworkDev.Web.Helpers.Authentication;
using FrameworkDev.Web.Helpers.Menus;

namespace FrameworkDev.Web.Areas.BaseInfo.Controllers
{
    [CustomAuthorize(PermissionKey = "BAS", PermissionName = "اطلاعات پایه")]
    public class BaseInfoMenuController : Helpers.CustomController
    {
        [MenuItem(Title = "مدیریت اطلاعات پایه", Order = 800, IsClickable = false, CssIcon = "fa fa-american-sign-language-interpreting", SubSystems = "BaseInfo")]
        public ActionResult Index()
        {
            Helpers.SubSystemsHelper.SetCurrentSubSystem(Session, "BaseInfo");
            return View();
        }
    }
}
