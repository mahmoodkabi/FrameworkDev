using FrameworkDev.Web.Areas.Management.Models;
using FrameworkDev.Web.Areas.Management.Repos;
using FrameworkDev.Web.Helpers.Authentication;

using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FrameworkDev.Web.Areas.Management.Controllers
{
    //======================================================منو
    /// <summary>
    ///
    /// </summary>
    [CustomAuthorize(PermissionKey = "MNG:USR:USZ", PermissionName = "دسترسی منطقه")]
    public class UserZonesController : Helpers.CustomController
    {
        private UserZonesRepository repo = new UserZonesRepository();
      
        /////======================================================
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize(PermissionKey = "MNG:USR:USZ:R", PermissionName = "نمایش")]
        public ActionResult Index()
        {
            Helpers.SubSystemsHelper.SetCurrentSubSystem(Session, "Management");
            return View();
        }

        //=====================================================
        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[CustomAuthorize(PermissionKey = "MNG:USR:USZ:R", PermissionName = "نمایش")]
        //public ActionResult LoadUserZones([DataSourceRequest] DataSourceRequest request)
        //{
        //    List<VM_UserZones> resval = repo.GetByUserID(Convert.ToInt32(Request.Params["UserId"])).ToList();
        //    DataSourceResult result = resval.ToDataSourceResult(request);

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <param name="VMUserZones"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [CustomAuthorize(PermissionKey = "MNG:USR:USZ:C", PermissionName = "ایجاد")]
        public ActionResult AddUserZonesSave([DataSourceRequest] DataSourceRequest request, VM_UserZones VMUserZones)
        {
            VMUserZones.USZCreDate = DateTime.Now;
            VMUserZones.USZUserId_fk = Convert.ToInt32(Request.Params["UID"]);
            VM_UserZones resval = repo.Insert(VMUserZones);
            return Json(new[] { resval }.ToDataSourceResult(request, ModelState));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <param name="VMUserZones"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [CustomAuthorize(PermissionKey = "MNG:USR:USZ:U", PermissionName = "ویرایش")]
        public ActionResult EditUserZonesSave([DataSourceRequest] DataSourceRequest request, VM_UserZones VMUserZones)
        {
            VMUserZones.USZCreDate = DateTime.Now;

            VM_UserZones resval = repo.Update(VMUserZones);
            return Json(new[] { resval }.ToDataSourceResult(request, ModelState));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <param name="VMUserZones"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [CustomAuthorize(PermissionKey = "MNG:USR:USZ:D", PermissionName = "حذف")]
        public ActionResult DeleteUserZonesSave([DataSourceRequest] DataSourceRequest request, VM_UserZones VMUserZones)
        {
            VM_UserZones resval = repo.Delete(VMUserZones.UserZoneID);
            return Json(new[] { resval }.ToDataSourceResult(request, ModelState));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="selected_zones"></param>
        /// <returns></returns>
        [CustomAuthorize(PermissionKey = "MNG:USR:USZ:C", PermissionName = "ایجاد")]
        public ActionResult SaveZones(int UserId, List<int> selected_zones)
        {
            if (UserId>0 && selected_zones != null && selected_zones.Count > 0)
            {
                foreach (int item in selected_zones)
                {
                    VM_UserZones VMUserZones = new VM_UserZones
                    {
                        USZCreDate = DateTime.Now,
                        USZZoneId_fk = item,
                        USZUserId_fk = UserId,
                        USZActive = true
                    };

                    VMUserZones = repo.Insert(VMUserZones);
                }
            }
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
}
