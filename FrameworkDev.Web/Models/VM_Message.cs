using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FrameworkDev.Web.Models
{
    public class VM_Message
    {
        public string Message { get; set; }

        public string Description { get; set; }

        public string Result { get; set; }
    }
}
