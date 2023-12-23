using System.Web.Mvc;

namespace FrameworkDev.Web.Areas.UrbanElementsManagement
{
    public class UrbanElementsManagementAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "UrbanElementsManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "UrbanElementsManagement_default",
                "UrbanElementsManagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}