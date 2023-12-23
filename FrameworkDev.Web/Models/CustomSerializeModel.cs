using System.Collections.Generic;

namespace FrameworkDev.Web.Models
{
    public class CustomSerializeModel
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<string> Roles { get; set; }

        public List<string> PermissionKeys { get; set; }
    }
}
