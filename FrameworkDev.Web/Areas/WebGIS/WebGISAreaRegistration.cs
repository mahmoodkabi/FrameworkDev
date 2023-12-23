using System.Web.Mvc;

namespace FrameworkDev.Web.Areas.WebGIS
{
    public class WebGISAreaRegistration : AreaRegistration 
    {
        //public override string AreaName 
        //{
        //    get 
        //    {
        //        return "WebGIS";
        //    }
        //}

        public override string AreaName => "WebGIS";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "WebGIS_default",
                "WebGIS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}