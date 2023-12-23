using System.Web.Mvc;

namespace FrameworkDev.Web.Areas.Document
{
    public class DocumentAreaRegistration : AreaRegistration
    {
        public override string AreaName => "Document";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Document_default",
                "Document/{controller}/{action}/{id}",
                new
                {
                    action = "Index",
                    id = UrlParameter.Optional
                }
            );
        }
    }
}