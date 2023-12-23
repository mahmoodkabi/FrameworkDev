
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using FrameworkDev.Web.Areas.BaseInfo.Controllers;
using FrameworkDev.Web.Areas.BaseInfo.Models;
using FrameworkDev.Web.Areas.BaseInfo.Repos;
using FrameworkDev.Web.Helpers.Authentication;
using FrameworkDev.Web.Helpers.Menus;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace FrameworkDev.Web.Areas.BaseInfo.Controllers
{
    [CustomAuthorize(PermissionKey = "BAS:CDF", PermissionName = "اطلاعات پایه")]
    [MenuItem(Title = "اطلاعات پایه", Order = 8, ParentController = typeof(BaseInfoMenuController), CssIcon = "fa fa-optin-monster", SubSystems = "BaseInfo")]
    public class BaseInfoController : FrameworkDev.Web.Helpers.CustomController
    {
        private readonly BaseInfoRepository repo = new BaseInfoRepository();

        [CustomAuthorize(PermissionKey = "BAS:CDF:R", PermissionName = "نمایش")]
        public ActionResult Index()
        {
            Helpers.SubSystemsHelper.SetCurrentSubSystem(Session, "BaseInfo");
            return View();
        }
        public JsonResult CheckCodeDuplicateError(string Code, int? BaseInfoId)
        {
            var baseInfo = repo.GetList().FirstOrDefault(x => x.BaseCode == Code & (BaseInfoId == null || x.BaseID != BaseInfoId));
            return Json(baseInfo == null, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(PermissionKey = "BAS:CDF:R", PermissionName = "نمایش")]
        public ActionResult GetBaseInfo(int id)
        {
            VM_BaseInfo vm = repo.GetByID(id);
            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(PermissionKey = "BAS:CDF:R", PermissionName = "نمایش")]

        public ActionResult GetBaseInfoTree(int? id = null)
        {
            return Json(repo.GetBaseInfoTree1(id), JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(PermissionKey = "BAS:CDF:U", PermissionName = "ویرایش")]
        [HttpPost]
        public ActionResult UpdateBaseInfo(VM_BaseInfo vm)
        {
            vm.ModifyDate = DateTime.Now;
            vm.UserId = (User as CustomPrincipal).UserId;


            VM_BaseInfo newVM = repo.Update(vm);
            return Json(newVM, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(PermissionKey = "BAS:CDF:C", PermissionName = "ایجاد")]
        [HttpPost]
        public ActionResult InsertBaseInfo(VM_BaseInfo vm)
        {
            if (!ModelState.IsValid)
            {
                return Json("Error", JsonRequestBehavior.AllowGet); ;
            }

            vm.ModifyDate = DateTime.Now;
            vm.UserId = (User as CustomPrincipal).UserId;
            vm.BaseID = 0;
            VM_BaseInfo res = repo.Insert(vm);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(PermissionKey = "BAS:CDF:D", PermissionName = "حذف")]
        [HttpPost]
        public ActionResult DeleteBaseInfo(VM_BaseInfo vm)
        {
            VM_BaseInfo res = repo.Delete(vm.BaseID);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}
