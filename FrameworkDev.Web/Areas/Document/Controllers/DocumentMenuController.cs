using FrameworkDev.Web.Helpers.Authentication;
using FrameworkDev.Web.Helpers.Menus;

using System.Web.Mvc;

namespace FrameworkDev.Web.Areas.Document.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [CustomAuthorize(PermissionKey = "DOC", PermissionName = "اطلاعات پیوست ها")]
    public class DocumentMenuController : Helpers.CustomController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize(PermissionKey = "DOC", PermissionName = "اطلاعات پیوست ها")]
        [MenuItem(Title = "پیوست ها", Order = 1230, IsClickable = false, CssIcon = "fa fa-sitemap")]
        public ActionResult Index()
        {
            //Helpers.SubSystemsHelper.SetCurrentSubSystem(Session, "Document");
            return View();
        }
    }
}
