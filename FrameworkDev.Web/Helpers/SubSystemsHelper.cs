using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.SessionState;
using FrameworkDev.Web.Helpers.Authentication;

namespace FrameworkDev.Web.Helpers
{
    public static class SubSystemsHelper
    {
        public static List<SubSystem> GetCurrentSubSystems()
        {
            CustomPrincipal user = (HttpContext.Current.User as CustomPrincipal);

            List<SubSystem> result = new List<SubSystem>();

            if (user.HasSubSystemPermission("BAS"))
            {
                result.Add(new SubSystem() { Id = 1, Name = "BaseInfo", NameFa = "اطلاعات پایه ", DefaultURL = "/BaseInfo/BaseInfo" });
            }

            if (user.HasSubSystemPermission("WebGIS"))
            {
                result.Add(new SubSystem() { Id = 2, Name = "WebGIS", NameFa = "WebGIS", DefaultURL = "/WebGIS/Home" });
            }

            if (user.HasSubSystemPermission("PassagesManagement"))
            {
                result.Add(new SubSystem() { Id = 3, Name = "PassagesManagement", NameFa = "مديريت معابر", DefaultURL = "/PassagesManagement/Home" });
            }

            if (user.HasSubSystemPermission("UrbanElementsManagement"))
            {
                result.Add(new SubSystem() { Id = 4, Name = "UrbanElementsManagement", NameFa = "المان هاي شهري", DefaultURL = "/UrbanElementsManagement/Home" });
            }

            if (user.HasSubSystemPermission("MNG"))
            {
                result.Add(new SubSystem() { Id = 5, Name = "Management", NameFa = "مدیریت سیستم", DefaultURL = "/Management/Users" });
            }
            
            if (user.HasSubSystemPermission("USL"))
            {
                result.Add(new SubSystem() { Id = 6, Name = "UserLog", NameFa = "مدیریت وقایع", DefaultURL = "/UnderConstruction/UnderConstructionView" });
            }
            
            //if (user.HasSubSystemPermission("BAS"))
            //{
            //    result.Add(new SubSystem() { Id = 4, Name = "Workflows", NameFa = "گردش كار", DefaultURL = "WorkFlow/WorkDesk/MyReq" });
            //}
            
            //if (user.HasSubSystemPermission("STP"))
            //{
            //    result.Add(new SubSystem() { Id = 13, Name = "StaffPortal", NameFa = "کارتابل", DefaultURL = "/StaffPortal/MyWorkFlow/ToDoReq" });
            //}
                                 
            //if (user.HasSubSystemPermission("UC2"))
            //{
            //    result.Add(new SubSystem() { Id = 4, Name = "uc2", NameFa = "..", DefaultURL = "/UnderConstruction/UnderConstructionView" });
            //}

            return result;
        }

        public static void SetCurrentSubSystem(HttpSessionStateBase _session, string _ssName)
        {
            SetCurrentSubSystem(_session, GetCurrentSubSystems().Find(x => x.Name == _ssName));
        }

        public static void SetCurrentSubSystem(HttpSessionStateBase _session, int _ssId)
        {
            SetCurrentSubSystem(_session, GetCurrentSubSystems().Find(x => x.Id == _ssId));
        }

        public static void SetCurrentSubSystem(HttpSessionStateBase _session, SubSystem _ss)
        {
            _session["currentSS"] = _ss;
        }

        public static SubSystem GetCurrentSubSystem(HttpSessionStateBase _session)
        {
            return (SubSystem)_session["currentSS"] ?? null;
        }

        public static SubSystem GetCurrentSubSystem(HttpSessionState _session)
        {
            return (SubSystem)_session["currentSS"] ?? null;
        }
    }











    public class SubSystem
    {
        [Key]
        public int Id { get; set; }

        [Index(IsUnique = true)]
        public string Name { get; set; }

        [Index(IsUnique = true)]
        public string NameFa { get; set; }

        public string DefaultURL { get; set; }
    }
}


//--------------------------------------------------------------
