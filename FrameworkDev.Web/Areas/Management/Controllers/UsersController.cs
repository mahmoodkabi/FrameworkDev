using FrameworkDev.Web.Areas.Management.Models;
using FrameworkDev.Web.Areas.Management.Repos;
using FrameworkDev.Web.Helpers.Authentication;
using FrameworkDev.Web.Helpers.Menus;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FrameworkDev.Web.Areas.Management.Controllers
{
    /// <summary>
    /// UsersController
    /// </summary>
    [CustomAuthorize(PermissionKey = "MNG:USR", PermissionName = "کاربران")]
    [MenuItem(Title = "کاربران", Order = 150, ParentController = typeof(ManagementMenuController), CssIcon = "fa fa-user fa-lg fa-fw", SubSystems = "Management")]
    public class UsersController : Helpers.CustomController
    {
        private readonly UsersRepository repo = new UsersRepository();
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize(PermissionKey = "MNG:USR:R", PermissionName = "نمایش")]
        public ActionResult Index()
        {
            Helpers.SubSystemsHelper.SetCurrentSubSystem(Session, "Management");
            ViewData["ZoneList"] = new BaseInfo.Repos.ZonesRepository().GetList();

           // ViewData["UnitList"] = new Organization.Repos.OrganUnitRepository().GetList();

          //  ViewData["EmplTypeList"] = new BaseInfo.Repos.EmployeeTypeRepository().GetList();

            return View();
        }

        [CustomAuthorize(PermissionKey = "MNG:USR:R", PermissionName = "نمایش")]
        public ActionResult LoadUserList([DataSourceRequest] DataSourceRequest request)
        {
            if (Request.Params["roleid"] == null || Convert.ToInt32(Request.Params["roleid"]) == 0)
                return Json("", JsonRequestBehavior.AllowGet);

            IQueryable<VM_User> resval = repo.GetUsersListByRoleId(Convert.ToInt32(Request.Params["roleid"]));
            DataSourceResult result = resval.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(PermissionKey = "MNG:USR:C", PermissionName = "ایجاد")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddUserListSave([DataSourceRequest] DataSourceRequest request, VM_User VMUser)
        {
            if (VMUser.Roles == null) VMUser.Roles = new List<int>();
            VMUser.Roles.Add(Convert.ToInt32(Request.Params["roleid"]));
            VM_User resval = repo.Insert(VMUser);
            return Json(new[] { resval }.ToDataSourceResult(request, ModelState));
        }

        [CustomAuthorize(PermissionKey = "MNG:USR:C", PermissionName = "ایجاد")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddUser(VM_User VMUser, int userId, string userName)
        {
            if (VMUser.Roles == null) 
                VMUser.Roles = new List<int>();

            VMUser.UserId = userId;
            VMUser.UserName = userName;

            VMUser.Roles.Add(Convert.ToInt32(Request.Params["roleid"]));
            VM_User res = repo.Insert(VMUser);
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        [CustomAuthorize(PermissionKey = "MNG:USR:U", PermissionName = "ویرایش")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditUserListSave([DataSourceRequest] DataSourceRequest request, VM_User VMUser)
        {
            
            VM_User resval = repo.Update(VMUser);
            return Json(new[] { resval }.ToDataSourceResult(request, ModelState));
        }

        [CustomAuthorize(PermissionKey = "MNG:USR:U", PermissionName = "ویرایش")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditUser(VM_User VMUser, int userId, string userName)
        {
            VMUser.UserId = userId;

            VM_User res = repo.Update(VMUser);
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        [CustomAuthorize(PermissionKey = "MNG:USR:D", PermissionName = "حذف")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteUserListSave([DataSourceRequest] DataSourceRequest request, VM_User VMUser)
        {
            VM_User resval = repo.Delete(VMUser.UserId);
            return Json(new[] { resval }.ToDataSourceResult(request, ModelState));
        }

        [CustomAuthorize(PermissionKey = "MNG:USR:D", PermissionName = "حذف")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteUser(VM_User VMUser, int userId, string userName)
        {
            VMUser.UserId = userId;
            VM_User res = repo.Delete(VMUser.UserId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

    }
}
