using System.Web;
using System.Web.Mvc;

namespace FrameworkDev.Web.Helpers.Authentication
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public string PermissionKey { get; set; }

        public string PermissionName { get; set; }

        protected virtual CustomPrincipal CurrentUser => HttpContext.Current.User as CustomPrincipal;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (CurrentUser == null)
            {
                return false;
            }

            return CustomAuthorizationHelper.FullAuthorize(CurrentUser, Users, Roles, PermissionKey);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            RedirectToRouteResult routeData = null;

            if (CurrentUser == null)
            {
                routeData = new RedirectToRouteResult
                    (new System.Web.Routing.RouteValueDictionary
                    (new
                    {
                        area = "",
                        controller = "Account",
                        action = "Login",
                    }
                    ));
            }
            else
            {
                routeData = new RedirectToRouteResult
                (new System.Web.Routing.RouteValueDictionary
                 (new
                 {
                     area = "",
                     controller = "Error",
                     action = "AccessDenied"
                 }
                 ));
            }

            filterContext.Result = routeData;
        }
    }
}
