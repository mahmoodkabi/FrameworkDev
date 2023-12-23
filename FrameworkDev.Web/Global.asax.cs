using AutoMapper;
using FrameworkDev.Web.Helpers.Authentication;
using FrameworkDev.Web.Helpers.Culture;
using FrameworkDev.Web.Models;
using Newtonsoft.Json;
using Stimulsoft.Base;
using Stimulsoft.LicenseHelper;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace FrameworkDev.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            //StimulsoftLicenseHelper.Activate();
            //StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHnaINyusxCfdMCquzmJf+AVbSbDq8ay5IiA7bcv06TocugvSFmps41oXwe3e7kjQUt/mVWh8XY4LaL2lgbBS0csdxFDrWE11RkqzaTbwt8WWSkjjctukkQ+pBHvDeOTAyJQM1EpoeOWlyH8au1DABFjRTTp0renGG7+210rDhExBjfp8gP/QTnojSwibU7kMU1Yxx6f87OfEjuSKEHK+9BdBhxLfVrE2KI9uIFdVfexpjV/qseIs8huasRvl2illV223RKY/gQR+xnTm44wQ3eLVLfNhk/mQDI1Tx00w84OHVoCpAiL22iq4LdkCQ4SzFqrIc6IvOnoUusVMNwqXva+5JNgxf3W4kYCZFw+KopnqChdDLik3ITbYx+A4q/8LkbCVHMtB3k8uOARi27HaBZ0cay9SxZlsPNioxuEVQ69n7JmQvcJ9asToK7RVxhRL3W/eCsyshctHPZK3cGEY/u04KQ3YwzDofhlCDqvlAT14apmjH811rJlB2yzj63LBjY+2mLbJH3m3lWBHovdO3BpEGhdNdJDYcdDZWxaZviFWorj/f4b/d6Yv5n1uuMo7fNNqraeqM2EMLDVY9ZjHNz5ta4C8pykgPGM5afzQj5bhfC5Xd6Ah4sSiSXh6cyE/NOQ0Dir1XNTyB0UBgus6OBsu+qBGdKmCOJrWJ71eMmx5ZTmTwDMdvhQ7ZHJNlAT4BLZ47I1mzvJoJ/FHf/LegnrR6QoV/EU9su/ROScmWfwGb0V3ywnFEG8LIYwRHAEZ4fsEHSNYCUbvn3aiB8o6gvk2hK08Ug7vbVTCDdANtVG4bNPcNb0nzQtNiwa4W45wNbAIZYW9K5mma/iOEdC6gQO0QTDVsJkrww5M6WJJ+Pj2i8F+9SBqHVWNPUO0K84yVo6YWEwMSBypY1MipKsChBZRdbLrwrp+9lm4zpQNQFYMswzzcfKLoKMFDKBPE55vDHvmWZHAKU9wb4Drynb1Mqu/ixsfbpiWx8zQ0RJOFOd7R333shxiQQbf8UQmT5vxumHRgYmeF3/l8QVR6RXIqMEZKWWTmfRPUu4LWmK/H5bUy/q1bwIa2iOH/PQoucZJ3Wm/CCOdrOTTva/ZIXJzodxDwQhjgJJzHEfZ6nob2TpTezAFcMzzcqxKb+gcNS4ugd0fWUQlfjlBNmwwE0zUa4XnzAXSRaiSPPs248pfPSyBpFWPgXdS/d++x5BNtXXNEZvH0R2jZWRHN/oCt47oq/Pog4tCM+TAkduPXWUivtuspVGm0VyICm2g7sXEbsii3TfGlKgwUz0NCKiTD51ou3EyEdJSlIdwWps+PzDZuSBrMVrdWi+R8u+3+AJl4wjjEfsU1G/T3I09Q7IqJyDy0Y6NI4O79rp+pW8ofNcbfZxHUBAt1MWi/gx4BEL4uw83qRSSSlht3bmw4yFfwSFVCVHNknRf+Bgv1wo26ezfvwPFmSj2TvSyJhAWoVAyxjZvFb+K4uq7yrzeiOkuaSfbHQP8EJMXDvQI5juZ8z5mDkLTXqnr4zwKA0rRwCofRlaGki5TvvDSbu9JakAqeP4y4G5sQSgMbv8CgUa4NB9poHfF6bD+Xtf+NXhnwSgtlJ2JOkc2+A4RCbI71pPbo1yPveCXVPi0rA69FpLUdM2yvPGMg==";

            SqlServerTypes.Utilities.LoadNativeAssemblies(Server.MapPath("~/bin"));

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Mapper.Initialize(cfg => cfg.ValidateInlineMaps = false);

            using (FrameworkDevEntities data = new FrameworkDevEntities())
            {
                foreach (User user in data.Users.Where(x => x.Salt == "testsalt"))
                {
                    user.Salt = CustomAuthenticationHelpers.GeneratePassword(100);
                    user.Password = CustomAuthenticationHelpers.EncodePassword(user.Password, user.Salt);
                }
                data.SaveChanges();
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e) => CultureHelper.SetPersianCulture();

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[Utility.GetAuthCookieName()];

            if (authCookie == null)
            {
                return;
            }

            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            CustomSerializeModel serializeModel = JsonConvert.DeserializeObject<CustomSerializeModel>(authTicket.UserData);

            CustomPrincipal principal = new CustomPrincipal(authTicket.Name)
            {
                UserId = serializeModel.UserId,
                FirstName = serializeModel.FirstName,
                LastName = serializeModel.LastName,
                Roles = serializeModel.Roles.ToArray<string>(),
                PermissionKeys = CustomAuthorizationHelper.GetAllPermissionKeys(serializeModel.UserId).ToArray()

            };

            HttpContext.Current.User = principal;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
#if DEBUG
#else
            var exception = Server.GetLastError();
            var httpContext = ((HttpApplication)sender).Context;
            httpContext.Response.Clear();
            httpContext.ClearError();

            if (new HttpRequestWrapper(httpContext.Request).IsAjaxRequest())
            {
                return;
            }

            ExecuteErrorController(httpContext, exception as HttpException);

#endif
        }

        private void ExecuteErrorController(HttpContext httpContext, HttpException exception)
        {
            //RouteData routeData = new RouteData();
            //routeData.Values["controller"] = "Error";

            //if (exception != null && exception.GetHttpCode() == (int)HttpStatusCode.NotFound)
            //    routeData.Values["action"] = "NotFound";
            //else
            //    routeData.Values["action"] = "InternalServerError";

            //using (Controller controller = new ErrorController())
            //    ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
        }
    }
}
