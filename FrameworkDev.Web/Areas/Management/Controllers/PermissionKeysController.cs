using FrameworkDev.Web.Helpers.Authentication;
using FrameworkDev.Web.Helpers.Menus;
using System.Web.Mvc;

namespace FrameworkDev.Web.Areas.Management.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [CustomAuthorize(PermissionKey = "MNG:PRK", PermissionName = "دسترسی ها")]
    [Authorize]
    [MenuItem(Title = "دسترسی ها", Order = 102, ParentController = typeof(ManagementMenuController), CssIcon = "fa fa-user fa-lg fa-fw", SubSystems = "Management")]
    public class PermissionKeysController : Helpers.CustomController
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize(PermissionKey = "MNG:PRK:R", PermissionName = "نمایش")]
        public ActionResult Index()
        {
            Helpers.SubSystemsHelper.SetCurrentSubSystem(Session, "Management");
            return View();
        }
    }
}
