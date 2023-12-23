using System.Web.Mvc;

namespace FrameworkDev.Web.Controllers
{
    [Authorize]
    public class UnderConstructionController : Helpers.CustomController
    {
        public ActionResult UnderConstructionView()
        {
            return View();
        }
    }
}
