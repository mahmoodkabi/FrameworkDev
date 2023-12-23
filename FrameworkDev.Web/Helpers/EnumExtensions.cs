using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace FrameworkDev.Web.Helpers
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum enu)
        {
            DisplayAttribute attr = GetDisplayAttribute(enu);
            return attr != null ? attr.Description : enu.ToString();
        }

        private static DisplayAttribute GetDisplayAttribute(object value)
        {
            Type type = value.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException(string.Format("Type {0} is not an enum", type));
            }

            FieldInfo field = type.GetField(value.ToString());
            return field?.GetCustomAttribute<DisplayAttribute>();
        }
    }
}
