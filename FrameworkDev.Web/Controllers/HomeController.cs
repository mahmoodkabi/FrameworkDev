using System.Net.Mime;
using System.Web.Mvc;
//using FrameworkDev.Web.Areas.Document.Models;
//using FrameworkDev.Web.Areas.Document.Repos;
using FrameworkDev.Web.Helpers;
using FrameworkDev.Web.Helpers.Authentication;
using FrameworkDev.Web.Models;

namespace FrameworkDev.Web.Controllers
{
    [Authorize]
    public class HomeController : Helpers.CustomController
    {
        private readonly FrameworkDevEntities context = new FrameworkDevEntities();

        public ActionResult Index()
        {
            return View(SubSystemsHelper.GetCurrentSubSystems());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult GetPersonelImage()
        {
            //Areas.Management.Models.VM_User CurrentUser;
            //Areas.Citizen.Models.VM_CitizenInfo UserCitizeInfo;
            //VM_HRDocs HRDoc;

            //try
            //{
            //    CurrentUser = new Areas.Management.Repos.UsersRepository().GetByID((User as CustomPrincipal).UserId);
            //    UserCitizeInfo = new Areas.Citizen.Repos.CitizenInfoRepository().GetByID((int)CurrentUser.CitizenID);
            //    HRDoc = new HRDocsRepository()
            //                .SelectByName(UserCitizeInfo.CTZNationalNo.Trim());
            //    if (HRDoc.file_stream == null)
            //    {
            //        HRDoc = new HRDocsRepository()
            //                .SelectByName("unknown");
            //    }
            //}
            //catch (System.Exception)
            //{
            //    HRDoc = new HRDocsRepository()
            //               .SelectByName("unknown");
            //}

            //byte[] fileBytes = HRDoc.file_stream;
            //string fileName = HRDoc.name;
            //return File(fileBytes, MediaTypeNames.Application.Octet, fileName);

            return null;
        }

        public JsonResult GetPersonelPossition()
        {
            //try
            //{
            //    Areas.Management.Models.VM_User _User = new Areas.Management.Repos.UsersRepository().GetByID((User as CustomPrincipal).UserId);
            //    Areas.Citizen.Models.VM_CitizenInfo _CitizeInfo = new Areas.Citizen.Repos.CitizenInfoRepository().GetByID((int)_User.CitizenID);
            //    Areas.Personnel.Models.VM_PersonnelInfo _PersonelInfo = new Areas.Personnel.Repos.PersonnelInfoRepository().GetByCitizenId(_CitizeInfo.CitizenID);
            //    Areas.Rulings.Models.VM_Eblagh _EblaghInfo = new Areas.Rulings.Repos.EblaghRepository().GetListByPersonelyId(_PersonelInfo.PersonelID).Find(e => e.EBLActive == true);
            //    Areas.Organization.Models.VM_Chart _ChartInfo = new Areas.Organization.Repos.ChartRepository().GetByID(_EblaghInfo.EBLChartId_fk);

            //    return Json(_ChartInfo.CRTChartTitle, JsonRequestBehavior.AllowGet);
            //}
            //catch
            //{
            //    return Json("نامشخص", JsonRequestBehavior.AllowGet);
            //}

            return null;
        }
    }
}
