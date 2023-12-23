using System.Web.Mvc;

namespace FrameworkDev.Web.Areas.BaseInfo
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseInfoAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// 
        /// </summary>
        public override string AreaName => "BaseInfo";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "BaseInfo_default",
                "BaseInfo/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
