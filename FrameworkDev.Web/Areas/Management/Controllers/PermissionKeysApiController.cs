using FrameworkDev.Web.Areas.Management.Repos;
using FrameworkDev.Web.Helpers;
using FrameworkDev.Web.Helpers.Authentication;
using FrameworkDev.Web.Models;

using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace FrameworkDev.Web.Areas.Management.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class PermissionKeysApiController : CustomApiController<dynamic>
    {
       

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [CustomAuthorize(PermissionKey = "MNG:PRK", PermissionName = "دسترسی ها")]
        public DataSourceResult GetPermissionKeys([ModelBinder(typeof(WebApiDataSourceRequestModelBinder))]DataSourceRequest request)
        {
            IQueryable<VM_PermissionTree> result = CustomPermissionKeyHelper.GetAllPermissionKeysTree().AsQueryable();

            return result.ToDataSourceResult(request);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize(PermissionKey = "MNG:PRK:R", PermissionName = "نمایش")]

        //[Route("api/PermissionKeysApi/GetPermissionKeysList")]
        public IHttpActionResult GetPermissionKeysList()
        {
            return Json(CustomPermissionKeyHelper.GetAllPermissionKeys());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="PermissionKey"></param>
        /// <returns></returns>
        [CustomAuthorize(PermissionKey = "MNG:PRK:R", PermissionName = "نمایش"), HttpGet]
        public IHttpActionResult GetPermissionKeysTree(int roleId = 0, string PermissionKey = "")
        {
            if (!(roleId > 0))
            {
                return null;
            }

            List<VM_PermissionTree> result = CustomPermissionKeyHelper.GetAllPermissionKeysTree();

            result = result.Where(x => x.ParentKey == PermissionKey).Distinct().OrderBy(x => x.PermissionName).ToList();

            List<string> rolePKList = new RolesRepository().GetPermissionsList(roleId).ToList();

            if (rolePKList != null)
            {
                foreach (VM_PermissionTree item in result)
                {
                    item.@checked = rolePKList.Any(x => x == item.PermissionKey);
                }
            }

            return Json(result);
        }
    }
}
