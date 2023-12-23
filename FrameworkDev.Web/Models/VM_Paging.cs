using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FrameworkDev.Web.Models
{
    public class VM_Paging
    {
        public int TotalRows { get; set; }

        public int skip { get; set; }

        public int take { get; set; }
    }
}
