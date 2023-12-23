using System.Web.Mvc;

using FrameworkDev.Web.Helpers.Menus;
using FrameworkDev.Web.Helpers.Authentication;

namespace FrameworkDev.Web.Areas.Management.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [CustomAuthorize(PermissionKey = "MNG", PermissionName = "مدیریت کاربران")]
    public class ManagementMenuController : Helpers.CustomController
    {
        // GET: Management/ManagementMenu
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [MenuItem(Title = "مدیریت سیستم", Order = 9000, IsClickable = false, CssIcon = "fa fa-cogs fa-lg fa-fw", SubSystems = "Management")]
        public ActionResult Index()
        {
            Helpers.SubSystemsHelper.SetCurrentSubSystem(Session, "Management");
            return View();
        }
    }
}
