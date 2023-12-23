using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using FrameworkDev.Web.Areas.BaseInfo.Repos;
using FrameworkDev.Web.Helpers.Authentication;
using FrameworkDev.Web.Helpers.Menus;
using System.Threading.Tasks;
using System;
using Kendo.Mvc.Extensions;
using FrameworkDev.Web.Areas.BaseInfo.Models;

namespace FrameworkDev.Web.Areas.BaseInfo.Controllers
{
    [CustomAuthorize(PermissionKey = "BAS:PRS", PermissionName = "مودیان")]
    //[MenuItem(Title = "مودیان", Order = 1, ParentController = typeof(BaseInfoMenuController), CssIcon = "fa fa-grav", SubSystems = "BaseInfo")]
    public class PersonsController : Helpers.CustomController
    {
        private readonly PersonRepository repo = new PersonRepository();

        [CustomAuthorize(PermissionKey = "BAS:PRS:R", PermissionName = "نمایش")]
        public ActionResult Index()
        {
            Helpers.SubSystemsHelper.SetCurrentSubSystem(Session, "BaseInfo");

            PlaceRepository repo11 = new PlaceRepository();
            ViewData["PlaceList"] = repo11.GetList();
            return View();
        }

        [CustomAuthorize(PermissionKey = "BAS:PRS:C", PermissionName = "ایجاد")]
        [HttpPost]
        public JsonResult AddPerson(VM_Person vm)
        {
            vm.PersonMelliCode = vm.PersonMelliCode ?? "";
            if (vm.PersonType == false && vm.PersonRadio == "MeliCode")// حقیقی و کد ملی اصلی
            {
                if (vm.PersonMelliCode.Trim().Length != 10)
                {
                    vm.Message.Result = "Error";
                    vm.Message.Message = "تعداد كاراكتر مجاز 10 كاراكتر می باشد";
                    return Json(vm, JsonRequestBehavior.AllowGet);
                }

                var mellicode = repo.GetList().FirstOrDefault(x => x.PersonMelliCode == vm.PersonMelliCode);
                if (mellicode != null)
                {
                    mellicode.Message.Result = "Error";
                    mellicode.Message.Message = "کد ملی تکراری می باشد";
                    return Json(mellicode, JsonRequestBehavior.AllowGet);
                }
            }
            else if (vm.PersonType == false && vm.PersonRadio == null)   // حقیقی و کد ملی موقت
            {
                var numbers = repo.GetList().Select(x => x.PersonMelliCode).ToList();

                int temp;
                int max = numbers.Where(n => (int.TryParse(n, out temp) ? temp : 0) < 200000).Select(n => int.TryParse(n, out temp) ? temp : 0).Max();
                vm.PersonMelliCode = (max + 1).ToString(); 

            }


                vm.IsAccountOwner = 1087;
            vm.IsAccountOwnerCompany = 1087;
            vm.UserId = (User as CustomPrincipal).UserId;
            vm.CreDate = DateTime.Now;
            var res = repo.Insert(vm);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(PermissionKey = "BAS:PRS:U", PermissionName = "ویرایش")]
        public JsonResult EditPerson(VM_Person VMPerson)
        {
            VMPerson.PersonMelliCode = VMPerson.PersonMelliCode ?? "";
            if (VMPerson.PersonType == false && VMPerson.PersonRadio == "MeliCode") // حقیقی و کد ملی اصلی
            {
                if (VMPerson.PersonMelliCode.Trim().Length != 10)
                {
                    VMPerson.Message.Result = "Error";
                    VMPerson.Message.Message = "تعداد كاراكتر مجاز 10 كاراكتر می باشد";
                    return Json(VMPerson, JsonRequestBehavior.AllowGet);
                }

                var mellicode = repo.GetList().FirstOrDefault(x => x.PersonId != VMPerson.PersonId && x.PersonMelliCode == VMPerson.PersonMelliCode);
                if (mellicode != null)
                {
                    mellicode.Message.Result = "Error";
                    mellicode.Message.Message = "کد ملی تکراری می باشد";
                    return Json(mellicode, JsonRequestBehavior.AllowGet);
                }
            }
            else if (VMPerson.PersonType == false && VMPerson.PersonRadio == null)   // حقیقی و کد موقت
            {
                VMPerson.PersonMelliCode = VMPerson.TempMelliCode.Trim().PadLeft(10,'0');
            }

            VMPerson.UserId = (User as CustomPrincipal).UserId;
            VMPerson.CreDate = DateTime.Now;

            var res = repo.Update(VMPerson);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(PermissionKey = "BAS:PRS:R", PermissionName = "نمایش")]
        public JsonResult GetList([DataSourceRequest]DataSourceRequest request)
        {
            var result = repo.GetList().ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [CustomAuthorize(PermissionKey = "BAS:PRS:D", PermissionName = "حذف")]
        [HttpPost]
        public JsonResult DeletePerson(VM_Person vm)
        {
            vm.UserId = (User as CustomPrincipal).UserId;
            vm.CreDate = DateTime.Now;
            var res = repo.Delete(vm.PersonId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SelectPerson(string MelliCode)
        {
            var res = repo.GetPersonByMeliCode(MelliCode);
            if (res == null)
            {
                return Json(new { isok = false }, JsonRequestBehavior.AllowGet);
            }


            if (res.PersonType == false)
                return Json(new { isok = true, data = res.Name + " " + res.Family, ID = res.PersonId }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { isok = true, data = res.CompanyName, ID = res.PersonId }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckPersonMelliCodeDuplicate(string PersonMelliCode)
        {
            var mellicode = repo.GetList().FirstOrDefault(x => x.PersonMelliCode == PersonMelliCode);
            return Json(mellicode == null, JsonRequestBehavior.AllowGet);
        }


        [CustomAuthorize(PermissionKey = "BAS:PRS:R", PermissionName = "نمایش")]
        [HttpGet]
        public JsonResult GetListByParentID([DataSourceRequest]DataSourceRequest request)
        {
            PlaceRepository repo = new PlaceRepository();
            var res = repo.GetListByParentID(241).ToDataSourceResult(request);
            return Json(res.Data, JsonRequestBehavior.AllowGet);
        }



    }
}

