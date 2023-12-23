using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace FrameworkDev.Web.Helpers.CustomDataAnnotation
{
    public class IsDateAttribute : ValidationAttribute
    {
        //A property to hold the name of the one you're going to use.
        public string Property { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PersianCalendar pc = new PersianCalendar();
            var res = Utility.ToMiladiDate((string)value);

            if (res != null)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("فرمت تاریخ صحیح نمی باشد");
        }

    }
}