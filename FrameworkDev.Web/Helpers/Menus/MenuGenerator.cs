using FrameworkDev.Web.Helpers.Authentication;
using FrameworkDev.Web.Models; using FrameworkDev.Web.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace FrameworkDev.Web.Helpers.Menus
{
    public class MenuGenerator
    {
        protected static CustomPrincipal CurrentUser => HttpContext.Current.User as CustomPrincipal;



        public static List<Menu> CreateMenu(SubSystem _subSystem)
        {
            return CreateMenuBase(_subSystem);

        }

        public static List<Menu> CreateMenuBase(SubSystem _subSystem)
        {
            List<Menu> menus = new List<Menu>();

            Assembly currentAssembly = Assembly.GetAssembly(typeof(MenuGenerator));
            List<Type> allControllers = currentAssembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Controller))).ToList();
            List<Type> menuControllers = allControllers.Where(t => t.GetCustomAttribute<MenuItemAttribute>() != null || t.GetMethods().Any(m => m.GetCustomAttribute<MenuItemAttribute>() != null)).ToList();

            if (_subSystem != null)
            {
                IEnumerable<Type> subSystemMenus = allControllers.Where(t =>
                        (t.GetCustomAttribute<MenuItemAttribute>() != null && t.GetCustomAttribute<MenuItemAttribute>().SubSystems != null && t.GetCustomAttribute<MenuItemAttribute>().SubSystems.Split(',').Length > 0 && t.GetCustomAttribute<MenuItemAttribute>().SubSystems.Split(',').Contains(_subSystem.Name)) ||
                         t.GetMethods().Any(m =>
                             (m.GetCustomAttribute<MenuItemAttribute>() != null && m.GetCustomAttribute<MenuItemAttribute>().SubSystems != null && m.GetCustomAttribute<MenuItemAttribute>().SubSystems.Split(',').Length > 0 && m.GetCustomAttribute<MenuItemAttribute>().SubSystems.Split(',').Contains(_subSystem.Name))
                        ));
                if (subSystemMenus.Any())
                {
                    menuControllers = subSystemMenus.ToList();
                }
                else
                {
                    menuControllers.Clear();
                }

                List<Menu> submenuControllers = new List<Menu>();
                menuControllers.ForEach(controller =>
                {
                    MenuItemAttribute navigation = controller.GetCustomAttribute<MenuItemAttribute>();
                    if (navigation == null)
                    {
                        controller.GetMethods().ToList().ForEach(method =>
                        {
                            navigation = method.GetCustomAttribute<MenuItemAttribute>();
                            if (navigation == null)
                            {
                                return;
                            }

                            if (!UserHasAccess(method.GetCustomAttribute<CustomAuthorizeAttribute>()))
                            {
                                return;
                            }

                            if (!UserHasAccess(method.GetCustomAttribute<CustomAuthorizeAttribute>()))
                            {
                                return;
                            }

                            Menu actionMenu = CreateAreaMenuItemFromAction(controller, method, navigation);
                            menus.Add(actionMenu);
                        });
                        return;
                    }

                    if (!UserHasAccess(controller.GetCustomAttribute<CustomAuthorizeAttribute>()))
                    {
                        return;
                    }

                    Menu menu = CreateAreaMenuItemFromController(controller, navigation);
                    if (navigation.ParentController != null)
                    {
                        if (navigation.ParentController.IsSubclassOf(typeof(Controller)))
                        {
                            menu.ParentControllerFullName = navigation.ParentController.FullName;
                            submenuControllers.Add(menu);
                        }
                    }
                    menus.Add(menu);
                });

                menus = menus.Except(submenuControllers).ToList();

                submenuControllers.ForEach(sm =>
                {
                    Menu parentMenu = menus.FirstOrDefault(m => m.ControllerFullName == sm.ParentControllerFullName);
                    parentMenu?.SubMenus.Add(new SubMenu()
                    {
                        Name = sm.Name,
                        Url = sm.Url,
                        CssIcon = sm.CssIcon,
                        Order = sm.Order,
                        Target = sm.Target
                    });
                });
            }

            return menus.OrderBy(m => m.Order).ToList();
        }

        private static Menu CreateAreaMenuItemFromController(Type controller, MenuItemAttribute menuItemAttribute)
        {
            string area = GetAreaNameForController(controller);
            string controllerName = controller.Name.Replace("Controller", "");
            Menu menu = new Menu()
            {
                Name = menuItemAttribute.Title ?? controllerName,
                ControllerFullName = controller.FullName,
                Order = menuItemAttribute.Order,
                CssIcon = menuItemAttribute.CssIcon,
                Target = menuItemAttribute.Target
            };

            if (menuItemAttribute.IsClickable)
            {
                menu.Url = CreateActionPath(area, controllerName, menuItemAttribute.Action ?? "Index");
            }
            List<SubMenu> submenus = new List<SubMenu>();       
            controller.GetMethods().ToList().ForEach(method =>
            {
                menuItemAttribute = method.GetCustomAttribute<MenuItemAttribute>();
                if (menuItemAttribute == null)
                {
                    return;
                }

                if (!UserHasAccess(method.GetCustomAttribute<CustomAuthorizeAttribute>()))
                {
                    return;
                }

                SubMenu submenu = new SubMenu()
                {
                    Name = menuItemAttribute.Title ?? method.Name,
                    Order = menuItemAttribute.Order,
                    CssIcon = menuItemAttribute.CssIcon,
                    Target = menuItemAttribute.CssIcon
                };
                if (menuItemAttribute.IsClickable)
                {
                    submenu.Url = CreateActionPath(area, controllerName, method.Name);
                }
                submenus.Add(submenu);
            });
            menu.SubMenus = submenus.OrderBy(m => m.Order).ToList();
            return menu;
        }

        private static bool UserHasAccess(CustomAuthorizeAttribute authorizedRoleAttribute)
        {
            if (authorizedRoleAttribute == null)
            {
                return true;
            }

            if (CurrentUser != null)
            {
                return CustomAuthorizationHelper.FullAuthorize(CurrentUser, authorizedRoleAttribute.Users, authorizedRoleAttribute.Roles, authorizedRoleAttribute.PermissionKey);
            }

            return false;
        }

        private static string CreateActionPath(string area, string controller, string action)
        {
            if (string.IsNullOrWhiteSpace(area))
            {
                return $"~/{controller}/{action}";
            }

            return $"~/{area}/{controller}/{action}";
        }

        private static Menu CreateAreaMenuItemFromAction(Type controller, MethodInfo method, MenuItemAttribute menuItemAttribute)
        {
            string area = GetAreaNameForController(controller);

            Menu menu = new Menu()
            {
                Name = menuItemAttribute.Title ?? method.Name,
                ControllerFullName = controller.FullName,
                Order = menuItemAttribute.Order,
                CssIcon = menuItemAttribute.CssIcon,
                Target = menuItemAttribute.Target
            };

            if (menuItemAttribute.CustomURL?.Length > 0)
            {
                menu.Url = menuItemAttribute.CustomURL;
            }
            else if (menuItemAttribute.IsClickable)
            {
                menu.Url = CreateActionPath(area, controller.Name.Replace("Controller", ""), method.Name);
            }

            return menu;
        }

        private static string GetAreaNameForController(Type controller)
        {
            string area = "";
            if (string.IsNullOrWhiteSpace(controller.Namespace))
            {
                return area;
            }

            if (controller.Namespace.Contains("Areas"))
            {
                List<string> parts = controller.Namespace.Split('.').ToList();
                area = parts[parts.FindLastIndex(n => n.Equals("Areas")) + 1];
            }
            return area;
        }
    }
}
