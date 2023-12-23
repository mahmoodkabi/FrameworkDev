using System.Web.Mvc;
using FrameworkDev.Web.Helpers.Authentication;
using FrameworkDev.Web.Helpers.Menus;
using Kendo.Mvc.UI;
using System.Threading.Tasks;
using FrameworkDev.Web.Areas.BaseInfo.Repos;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using FrameworkDev.Web.Areas.BaseInfo.Models;
using FrameworkDev.Web.Areas.Management.Models;
using System.Linq;

namespace FrameworkDev.Web.Areas.BaseInfo.Controllers
{
    [CustomAuthorize(PermissionKey = "BAS:ZON", PermissionName = "مناطق")]
    //[MenuItem(Title = " مناطق ", Order = 2, ParentController = typeof(BaseInfoMenuController), CssIcon = "fa fa-random", SubSystems = "BaseInfo")]
    public class ZonesController : Helpers.CustomController
    {
        ZonesRepository repo = new ZonesRepository();

        [CustomAuthorize(PermissionKey = "BAS:ZON:R", PermissionName = "نمایش")]
        public ActionResult Index()
        {
            Helpers.SubSystemsHelper.SetCurrentSubSystem(Session, "BaseInfo");
            return View();
        }


        [CustomAuthorize(PermissionKey = "BAS:ZON:R", PermissionName = "نمایش")]
        [HttpGet]
        public JsonResult GetList([DataSourceRequest]DataSourceRequest request)
        {
            var res =   repo.GetListByParentID(100).ToDataSourceResult(request);
            return Json(res.Data, JsonRequestBehavior.AllowGet);
        }
        [CustomAuthorize(PermissionKey = "BAS:ZON:R", PermissionName = "نمایش")]
        [HttpGet]
        public JsonResult GetListByParentID([DataSourceRequest]DataSourceRequest request)
        {
            var res = repo.GetListByParentID(100).ToDataSourceResult(request);
            return Json(res.Data, JsonRequestBehavior.AllowGet);
        }

        //[CustomAuthorize(PermissionKey = "BAS:ZON:R", PermissionName = "نمایش")]
        // public ActionResult GetZonesDifferenceList([DataSourceRequest] DataSourceRequest request)
        //{
        //    int UserId = Convert.ToInt32(Request.Params["UserId"]);
        //    int UID = Convert.ToInt32(Request.Params["UID"]);
        //    if (UserId == 0 && UID != 0) UserId = UID;

        //    Management.Repos.UserZonesRepository rep_user_zon = new Management.Repos.UserZonesRepository();

        //    BaseInfo.Repos.ZonesRepository rep_zon = new BaseInfo.Repos.ZonesRepository();

           
        //   List<VM_ZoneInfo> zones = rep_zon.GetListZone().ToList() ;

        //   List<VM_UserZones> user_zones = rep_user_zon.GetByUserID(UserId).ToList();

        //    var result = zones.Where(p => !user_zones.Any(p2 => p2.USZZoneId_fk == p.ZoneId));

        //    return  Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}
        public JsonResult CheckCodeDuplicate(string Code)
        {
            var zone = repo.GetList().FirstOrDefault(x => x.Code == Code);
            return Json(zone == null, JsonRequestBehavior.AllowGet);
        }
    }
}
