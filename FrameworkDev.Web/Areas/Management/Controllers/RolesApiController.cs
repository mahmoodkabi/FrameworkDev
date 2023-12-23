using System;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using FrameworkDev.Web.Areas.Management.Models;
using FrameworkDev.Web.Areas.Management.Repos;
using FrameworkDev.Web.Helpers;
using FrameworkDev.Web.Helpers.Authentication;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace FrameworkDev.Web.Areas.Management.Controllers
{
    [CustomAuthorize(PermissionKey = "MNG:ROL", PermissionName = "انواع نقش ها")]
    public class RolesApiController : CustomApiController<RolesRepository>
    {
        [CustomAuthorize(PermissionKey = "MNG:ROL:R", PermissionName = "نمایش")]
        public async Task<DataSourceResult> GetRoles([ModelBinder(typeof(WebApiDataSourceRequestModelBinder))]DataSourceRequest request)
        {
            return (await repo.GetListAsync().ConfigureAwait(false)).ToDataSourceResult(request);
        }

        [CustomAuthorize(PermissionKey = "MNG:ROL:R", PermissionName = "نمایش")]
        public async Task<VM_Role> GetRole(int id)
        {
            VM_Role vm = await repo.GetByIDAsync(id).ConfigureAwait(false);

            if (vm == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return vm;
        }

        [CustomAuthorize(PermissionKey = "MNG:ROL:R", PermissionName = "نمایش")]
        [HttpGet]
        public async Task<IHttpActionResult> Lookup([ModelBinder(typeof(WebApiDataSourceRequestModelBinder))]DataSourceRequest request, int _maxCount)
        {
            if (request.PageSize > _maxCount)
            {
                request.PageSize = _maxCount;
            }

            return Json((await repo.GetLookupListAsync().ConfigureAwait(false)).ToDataSourceResult(request).Data);
        }

        [CustomAuthorize(PermissionKey = "MNG:ROL:U", PermissionName = "ویرایش")]
        public async Task<HttpResponseMessage> PutRole(int id, VM_Role role)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != role.RoleId)
            {
                role.RoleId = id;
            }

            try
            {
                VM_Role vmResult = await repo.UpdateAsync(role).ConfigureAwait(false);

                DataSourceResult result = new DataSourceResult
                {
                    Data = new[] { vmResult },
                    Total = 1
                };

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = vmResult.RoleId }));

                return response;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (System.Data.Entity.Validation.DbEntityValidationResult validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (System.Data.Entity.Validation.DbValidationError validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);

                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }

        [CustomAuthorize(PermissionKey = "MNG:ROL:C", PermissionName = "ایجاد")]
        public async Task<HttpResponseMessage> PostRole(VM_Role vm)
        {
            if (ModelState.IsValid)
            {
                VM_Role resultVM = await repo.InsertAsync(vm).ConfigureAwait(false);

                DataSourceResult result = new DataSourceResult
                {
                    Data = new[] { resultVM },
                    Total = 1
                };

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, result);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = resultVM.RoleId }));

                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [CustomAuthorize(PermissionKey = "MNG:ROL:D", PermissionName = "حذف")]
        public async Task<HttpResponseMessage> DeleteRole(int id)
        {
            try
            {
                VM_Role vm = await repo.DeleteAsync(id).ConfigureAwait(false);
                return Request.CreateResponse(HttpStatusCode.OK, vm);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        [CustomAuthorize(PermissionKey = "MNG:ROL:X", PermissionName = "اكسل")]
        public async Task<HttpResponseMessage> Excle(int id)
        {
            return null;
        }
    }
}
