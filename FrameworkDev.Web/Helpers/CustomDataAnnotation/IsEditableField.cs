using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FrameworkDev.Web.Helpers.CustomDataAnnotation
{
    public class IsEditableFieldAttribute : ValidationAttribute
    {
        //A property to hold the name of the one you're going to use.
        public string Property { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //Get the value of the property using reflection.
            var otherProperty = validationContext.ObjectType.GetProperty(Property);
            var otherPropertyValue = (string)otherProperty.GetValue(validationContext.ObjectInstance, null);
            otherPropertyValue = otherPropertyValue == null ? "" : otherPropertyValue.Trim();

            if (otherPropertyValue != "")
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid property message.");
            //return base.IsValid(value); 
        }

    }
}