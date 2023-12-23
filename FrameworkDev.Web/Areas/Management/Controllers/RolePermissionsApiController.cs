using FrameworkDev.Web.Areas.Management.Models;
using FrameworkDev.Web.Areas.Management.Repos;
using FrameworkDev.Web.Helpers;
using FrameworkDev.Web.Helpers.Authentication;

using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

using System;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace FrameworkDev.Web.Areas.Management.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [CustomAuthorize(PermissionKey = "MNG:ROL", PermissionName = "انواع نقش ها")]
    public class RolePermissionsApiController : CustomApiController<RolePermissionsRepository>
    {
       

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [CustomAuthorize(PermissionKey = "MNG:RLP:R", PermissionName = "نمایش")]
        public async Task<DataSourceResult> GetRolePermissions([ModelBinder(typeof(WebApiDataSourceRequestModelBinder))]DataSourceRequest request)
        {
            return (await repo.GetListAsync().ConfigureAwait(false)).ToDataSourceResult(request);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CustomAuthorize(PermissionKey = "MNG:RLP:R", PermissionName = "نمایش")]
        public async Task<VM_RolePermission> GetRolePermission(int id)
        {
            VM_RolePermission vm = await repo.GetByIDAsync(id).ConfigureAwait(false);

            if (vm == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return vm;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="RolePermission"></param>
        /// <returns></returns>
        [CustomAuthorize(PermissionKey = "MNG:RLP:U", PermissionName = "ویرایش")]
        public async Task<HttpResponseMessage> PutRolePermission(int id, VM_RolePermission RolePermission)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != RolePermission.RPKId)
            {
                RolePermission.RPKId = id;
            }

            try
            {
                VM_RolePermission vmResult = await repo.UpdateAsync(RolePermission).ConfigureAwait(false);

                DataSourceResult result = new DataSourceResult
                {
                    Data = new[] { vmResult },
                    Total = 1
                };

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = vmResult.RPKId }));

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

                        // raise a new exception nesting the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [CustomAuthorize(PermissionKey = "MNG:RLP:C", PermissionName = "ایجاد")]
        public async Task<HttpResponseMessage> PostRolePermission(VM_RolePermission vm)
        {
            if (ModelState.IsValid)
            {
                VM_RolePermission resultVM = await repo.InsertAsync(vm).ConfigureAwait(false);

                DataSourceResult result = new DataSourceResult
                {
                    Data = new[] { resultVM },
                    Total = 1
                };

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, result);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = resultVM.RPKId }));

                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CustomAuthorize(PermissionKey = "MNG:RLP:D", PermissionName = "حذف")]
        public async Task<HttpResponseMessage> DeleteRolePermission(int id)
        {
            try
            {
                VM_RolePermission vm = await repo.DeleteAsync(id).ConfigureAwait(false);
                return Request.CreateResponse(HttpStatusCode.OK, vm);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing) 
        {
            repo.Dispose();
            base.Dispose(disposing);
        }
    }
}
