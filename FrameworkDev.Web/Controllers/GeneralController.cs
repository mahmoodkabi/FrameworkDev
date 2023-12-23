using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FrameworkDev.Web.Areas.BaseInfo.Repos;

namespace FrameworkDev.Web.Controllers
{
    public class GeneralController : Helpers.CustomController
    {
        public JsonResult GetAllZone()
        {
            using (Areas.Management.Repos.UserZonesRepository repository = new Areas.Management.Repos.UserZonesRepository())
            {
                List<Areas.Management.Models.VM_UserZones> result = repository.GetAccessList(User.Identity.Name, "45").ToList();
                result = result.OrderBy(ab => ab.Code).ToList();
               return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAllZoneType1()
        {
            using (Areas.Management.Repos.UserZonesRepository repository = new Areas.Management.Repos.UserZonesRepository())
            {
                List<Areas.Management.Models.VM_UserZones> result = repository.GetAccessList(User.Identity.Name, "45").ToList();
                result = result.Where(ab => ab.ZONType == 1).OrderBy(ab => ab.ZoneID).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
      
        public JsonResult GetPersonFullName([DataSourceRequest]DataSourceRequest request, string textFilter)
        {
            IQueryable<Areas.BaseInfo.Models.VM_Person> res = null;
            Areas.BaseInfo.Repos.PersonRepository repository = new Areas.BaseInfo.Repos.PersonRepository();
            if (textFilter == "" || textFilter == null)
                res = repository.GetList();
            else
                res = repository.GetList().Where(p => p.FullPersonName.Contains(textFilter));

            DataSourceResult result = res.ToDataSourceResult(request);
            return Json(result.Data, JsonRequestBehavior.AllowGet);
        }


    }
}
