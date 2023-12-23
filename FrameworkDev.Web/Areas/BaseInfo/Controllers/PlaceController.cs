using System.Threading.Tasks;
using System.Web.Mvc;
using FrameworkDev.Web.Areas.BaseInfo.Repos;
using FrameworkDev.Web.Helpers.Authentication;
using FrameworkDev.Web.Helpers.Menus;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using FrameworkDev.Web.Models;

namespace FrameworkDev.Web.Areas.BaseInfo.Controllers
{
    [CustomAuthorize(PermissionKey = "BAS:PLC", PermissionName = "اطلاعات محل ها")]
    //[MenuItem(Title = "محل ها", Order = 7, ParentController = typeof(BaseInfoMenuController), CssIcon = "fa fa-cog fa-lg fa-fw", SubSystems = "BaseInfo")]
    public class PlaceController : Helpers.CustomController
    {
        private readonly PlaceRepository repo = new PlaceRepository();
        private readonly FrameworkDevEntities context = new FrameworkDevEntities();

        [CustomAuthorize(PermissionKey = "BAS:PLC:R", PermissionName = "نمایش")]
        public ActionResult Index()
        {
            Helpers.SubSystemsHelper.SetCurrentSubSystem(Session, "BaseInfo");
            return View();
        }

        [CustomAuthorize(PermissionKey = "BAS:PLC:R", PermissionName = "نمایش")]
        [HttpGet]
        public JsonResult GetList([DataSourceRequest]DataSourceRequest request)
        {
            var res = repo.GetList().ToDataSourceResult(request);
            return Json(res.Data, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(PermissionKey = "BAS:PLC:R", PermissionName = "نمایش")]
        [HttpGet]
        public JsonResult GetListByParentID([DataSourceRequest]DataSourceRequest request)
        {
            var res = repo.GetListByParentID(241).ToDataSourceResult(request);
            return Json(res.Data, JsonRequestBehavior.AllowGet);
        }

    }
}
