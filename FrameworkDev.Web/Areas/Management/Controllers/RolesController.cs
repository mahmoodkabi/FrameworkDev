using FrameworkDev.Web.Helpers.Authentication;
using FrameworkDev.Web.Helpers.Menus;
using System.Web.Mvc;

namespace FrameworkDev.Web.Areas.Management.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [CustomAuthorize(PermissionKey = "MNG:ROL", PermissionName = "انواع نقش ها")]
    [Authorize]
    [MenuItem(Title = "نقش ها", Order = 102, ParentController = typeof(ManagementMenuController), CssIcon = "fa fa-user fa-lg fa-fw", SubSystems = "Management")]
    public class RolesController : Helpers.CustomController
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            Helpers.SubSystemsHelper.SetCurrentSubSystem(Session, "Management");
            return View();
        }
    }
}
