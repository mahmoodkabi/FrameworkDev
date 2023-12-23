using System;

namespace FrameworkDev.Web.Helpers.Menus
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MenuItemAttribute : Attribute
    {
        public MenuItemAttribute()
        {
            IsClickable = true;
            Action = "Index";
        }

        public bool IsClickable { get; set; }

        public string Title { get; set; }

        public string Action { get; set; }

        public string CssIcon { get; set; }

        public string SubSystems { get; set; }

        public int Order { get; set; }

        public string Target { get; set; }

        public string CustomURL { get; set; }

        public Type ParentController { get; set; }
    }
}
