using FrameworkDev.Web.Areas.Management.Models;
using FrameworkDev.Web.Areas.Management.Repos;
using FrameworkDev.Web.Helpers;
using FrameworkDev.Web.Helpers.Authentication;

using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace FrameworkDev.Web.Areas.Management.Controllers
{
    [CustomAuthorize(PermissionKey = "MNG:USR:USZ", PermissionName = "دسترسی منطقه")]
    public class UserZonesApiController : CustomApiController<UserZonesRepository>
    {
        public UserZonesApiController()
        {
            this.repo.IP = System.Web.HttpContext.Current.Request.UserHostAddress;
            this.repo.Username = User.Identity.Name;

        }


        [CustomAuthorize(PermissionKey = "MNG:USR:USZ:R", PermissionName = "نمایش")]
        public async Task<DataSourceResult> GetUserZones([ModelBinder(typeof(WebApiDataSourceRequestModelBinder))]DataSourceRequest request)
        {
            IEnumerable<KeyValuePair<string, string>> q = Request.GetQueryNameValuePairs();

            string userid = q.ToList().FirstOrDefault(x => x.Key == "UserId").Value;

            if (!string.IsNullOrEmpty(userid))
            {
                int _userId = int.Parse(userid);

                if (_userId > 0)
                {
                    return (repo.GetByUserID(_userId)).ToDataSourceResult(request);
                }
            }

            return (await repo.GetListAsync().ConfigureAwait(false)).ToDataSourceResult(request);
        }

        [CustomAuthorize(PermissionKey = "MNG:USR:USZ:R", PermissionName = "نمایش")]
        public async Task<VM_UserZones> GetUserZones(int id)
        {
            VM_UserZones vm = await repo.GetByIDAsync(id).ConfigureAwait(false);

            if (vm == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return vm;
        }

        [CustomAuthorize(PermissionKey = "MNG:USR:USZ:U", PermissionName = "ویرایش")]
        public async Task<HttpResponseMessage> PutUserZones(int id, VM_UserZones vm)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != vm.UserZoneID)
            {
                vm.UserZoneID = id;
            }

            vm.USZStatus = 0;
            vm.USZType = 0;
            vm.USZUserId_fk = (User as CustomPrincipal).UserId;
            vm.USZCreDate = DateTime.Now;

            try
            {
                VM_UserZones newVM = await repo.UpdateAsync(vm).ConfigureAwait(false);
                return Request.CreateResponse(HttpStatusCode.OK, newVM);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        [CustomAuthorize(PermissionKey = "MNG:USR:USZ:C", PermissionName = "ایجاد")]
        public async Task<HttpResponseMessage> PostUserZones(VM_UserZones vm)
        {
            if (ModelState.IsValid)
            {
                vm.USZStatus = 0;
                vm.USZType = 0;
                vm.USZCreateUserId_fk = (User as CustomPrincipal).UserId;
                vm.USZCreDate = DateTime.Now;

                VM_UserZones resultVM = await repo.InsertAsync(vm).ConfigureAwait(false);

                DataSourceResult result = new DataSourceResult
                {
                    Data = new[] { resultVM },
                    Total = 1
                };

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, result);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = resultVM.UserZoneID }));

                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [CustomAuthorize(PermissionKey = "MNG:USR:USZ:D", PermissionName = "حذف")]
        public async Task<HttpResponseMessage> DeleteUserZones(int id)
        {
            try
            {
                VM_UserZones vm = await repo.DeleteAsync(id).ConfigureAwait(false);
                return Request.CreateResponse(HttpStatusCode.OK, vm);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        [CustomAuthorize(PermissionKey = "MNG:USR:USZ:C", PermissionName = "ایجاد")]
        public HttpResponseMessage AddUserZones(int[] selected_zones, int UserId)
        {
            try
            {
                if (UserId > 0 && selected_zones.Length > 0)
                {
                    foreach (int item in selected_zones)
                    {
                        VM_UserZones VMUserZones = new VM_UserZones
                        {
                            USZCreDate = DateTime.Now,
                            USZZoneId_fk = item,
                            USZUserId_fk = UserId,
                            USZActive = true
                        };

                        VMUserZones = repo.Insert(VMUserZones);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}
