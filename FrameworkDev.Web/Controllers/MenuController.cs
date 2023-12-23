using System.Web.Mvc;
using FrameworkDev.Web.Helpers;
using FrameworkDev.Web.Helpers.Menus;

namespace FrameworkDev.Web.Controllers
{
    [Authorize()]
    public class MenuController : Helpers.CustomController
    {
        public PartialViewResult Index()
        {
            return PartialView("~/Views/Shared/Partials/_menu.cshtml", MenuGenerator.CreateMenu(SubSystemsHelper.GetCurrentSubSystem(Session)));

            //System.Web.HttpCookie currentSybSystemCookie = Request.Cookies.Get("ssName");

            //if (currentSybSystemCookie != null)
            //{
            //    return PartialView("~/Views/Shared/Partials/_menu.cshtml", MenuGenerator.CreateMenu(currentSybSystemCookie.Value));
            //}

            //return PartialView("~/Views/Shared/Partials/_menu.cshtml", MenuGenerator.CreateMenu());
        }
    }
}
