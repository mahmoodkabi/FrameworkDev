using System.Web.Mvc;

namespace FrameworkDev.Web.Controllers
{
    [Authorize]
    public class ErrorController : Helpers.CustomController
    {
        public ActionResult InternalServerError()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}
