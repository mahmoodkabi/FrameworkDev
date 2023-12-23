using System.Web.Mvc;

namespace TIS.Web.Areas.CheckOperation
{
    public class CheckOperationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CheckOperation";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CheckOperation_default",
                "CheckOperation/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}