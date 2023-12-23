using FrameworkDev.Web.Areas.Management.Models;
using FrameworkDev.Web.Areas.Management.Repos;
using FrameworkDev.Web.Helpers;
using FrameworkDev.Web.Helpers.Authentication;

using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
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
    [CustomAuthorize(PermissionKey = "MNG:USR", PermissionName = "کاربران")]
    public class UsersApiController : CustomApiController<UsersRepository>
    {
        public UsersApiController()
        {
            this.repo.IP = System.Web.HttpContext.Current.Request.UserHostAddress;
            this.repo.Username = User.Identity.Name;

        }

        [CustomAuthorize(PermissionKey = "MNG:USR:R", PermissionName = "نمایش")]
        public async Task<DataSourceResult> GetUsers([ModelBinder(typeof(WebApiDataSourceRequestModelBinder))]DataSourceRequest request)
        {
            return (await repo.GetListAsync().ConfigureAwait(false)).ToDataSourceResult(request);
        }

        [CustomAuthorize(PermissionKey = "MNG:USR:R", PermissionName = "نمایش")]
        public async Task<VM_User> GetUser(int id)
        {
            VM_User vm = await repo.GetByIDAsync(id).ConfigureAwait(false);

            if (vm == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return vm;
        }

        [CustomAuthorize(PermissionKey = "MNG:USR:U", PermissionName = "ویرایش")]
        public async Task<HttpResponseMessage> PutUser(int id, VM_User user)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != user.UserId)
            {
                user.UserId = id;
            }

            try
            {
                VM_User vmResult = await repo.UpdateAsync(user).ConfigureAwait(false);

                DataSourceResult result = new DataSourceResult
                {
                    Data = new[] { vmResult },
                    Total = 1
                };

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = vmResult.UserId }));

                return response;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
            catch (DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (DbEntityValidationResult validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (DbValidationError validationError in validationErrors.ValidationErrors)
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

        [CustomAuthorize(PermissionKey = "MNG:USR:C", PermissionName = "ایجاد")]
        public async Task<HttpResponseMessage> PostUser(VM_User vm)
        {
            if (ModelState.IsValid)
            {
                VM_User resultVM = await repo.InsertAsync(vm).ConfigureAwait(false);

                DataSourceResult result = new DataSourceResult
                {
                    Data = new[] { resultVM },
                    Total = 1
                };

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, result);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = resultVM.UserId }));

                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [CustomAuthorize(PermissionKey = "MNG:USR:D", PermissionName = "حذف")]
        public async Task<HttpResponseMessage> DeleteUser(int id)
        {
            try
            {
                VM_User vm = await repo.DeleteAsync(id).ConfigureAwait(false);
                DataSourceResult result = new DataSourceResult
                {
                    Data = new[] { vm },
                    Total = 1
                };
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }
    }
}
