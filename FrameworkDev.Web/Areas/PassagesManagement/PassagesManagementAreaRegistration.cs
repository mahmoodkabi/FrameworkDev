using System.Web.Mvc;

namespace FrameworkDev.Web.Areas.PassagesManagement
{
    public class PassagesManagementAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PassagesManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PassagesManagement_default",
                "PassagesManagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}