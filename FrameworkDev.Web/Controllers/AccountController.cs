using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using FrameworkDev.Web.Areas.Management.Models;
using FrameworkDev.Web.Areas.Management.Repos;
using FrameworkDev.Web.Helpers.Authentication;
using FrameworkDev.Web.Models;
using Newtonsoft.Json;

namespace FrameworkDev.Web.Controllers
{
    [Authorize]
    public class AccountController : Helpers.CustomController
    {
        [Authorize]

        public ActionResult Index()
        {
            Response.Redirect("/", true);
            return View();
        }

        [Authorize]
        public ActionResult MyProfile()
        {
            using (FrameworkDevEntities _db = new FrameworkDevEntities())
            {
                User _user = _db.Users.FirstOrDefault(x => x.Username == User.Identity.Name);

                VM_UserWithProfile vm = Mapper.Map<VM_UserWithProfile>(_user);

                //if (_user.CitizenID.HasValue)
                //{
                //    TBL_CitizenInfo _citizen = _db.TBL_CitizenInfo.Find(_user.CitizenID);
                //    vm.CTZNationalNo = _citizen.CTZNationalNo;
                //    vm.CTZTelNo = _citizen.CTZTelNo;
                //    vm.CTZMobileNo = _citizen.CTZMobileNo;
                //    vm.CTZAddressHome = _citizen.CTZAddressHome;
                //}

                return View("MyProfile", vm);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult MyProfile(VM_UserWithProfile vm)
        {
            using (FrameworkDevEntities _db = new FrameworkDevEntities())
            {
                User entity = _db.Users.FirstOrDefault(x => x.Username == User.Identity.Name);

                entity.FirstName = vm.FirstName;
                entity.LastName = vm.LastName;
                entity.Email = vm.Email;

                //if (entity.CitizenID.HasValue)
                //{
                //    TBL_CitizenInfo _citizen = _db.TBL_CitizenInfo.Find(entity.CitizenID);
                //    _citizen.CTZNationalNo = _citizen.CTZNationalNo;
                //    _citizen.CTZTelNo = vm.CTZTelNo;
                //    _citizen.CTZMobileNo = vm.CTZMobileNo;
                //    _citizen.CTZAddressHome = vm.CTZAddressHome;
                //}

                _db.SaveChanges();

                //vm.CitizenID = entity.CitizenID;
            }

            return View("MyProfile", vm);
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View(new VM_ChangePassword());
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(VM_ChangePassword vm)
        {
            if (ModelState.IsValid)
            {
                CustomPrincipal user = User as CustomPrincipal;

                if (new CustomMembership().ValidateUser(user.Identity.Name, vm.CurrentPassword))
                {
                    using (UsersRepository repo = new UsersRepository())
                    {
                        VM_User vmUser = repo.GetByID(user.UserId);

                        if (user != null)
                        {
                            vmUser.Password = vm.Password;
                            vmUser.PasswordConfirm = vm.PasswordConfirm;

                            VM_User updateResult = repo.Update(vmUser);
                        }
                    }
                }
            }

            return View(vm);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> Login(string ReturnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                return LogOut();
            }
            ViewBag.ReturnUrl = ReturnUrl;

            VM_Login loginView = new VM_Login() { RememberMe = false };

            return View(loginView);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login(VM_Login loginView, string ReturnUrl = "")
        {
            if (!string.IsNullOrEmpty(loginView.UserName) && !string.IsNullOrEmpty(loginView.Password))
            {
                if (Membership.ValidateUser(loginView.UserName, loginView.Password))
                {
                    CustomMembershipUser user = (CustomMembershipUser)Membership.GetUser(loginView.UserName, false);
                    if (user != null)
                    {
                        CustomSerializeModel userModel = new CustomSerializeModel()
                        {
                            UserId = user.UserId,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Roles = user.Roles.Select(r => r.RoleName).ToList()

                        };

                        string userData = JsonConvert.SerializeObject(userModel);

                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1,
                            loginView.UserName,
                            DateTime.Now,
                            DateTime.Now.AddMinutes((int)FormsAuthentication.Timeout.TotalMinutes),
                            loginView.RememberMe ?? false,
                            userData
                            );

                        string enTicket = FormsAuthentication.Encrypt(authTicket);

                        HttpCookie faCookie = new HttpCookie(Utility.GetAuthCookieName(), enTicket) { Expires = DateTime.Now.AddMinutes((int)FormsAuthentication.Timeout.TotalMinutes) };

                        Response.Cookies.Add(faCookie);
                    }

                    if (!Url.IsLocalUrl(ReturnUrl) || ReturnUrl.ToLower().Replace("/", "") == "accountlogout")
                    {
                        ReturnUrl = "/";
                    }

                    loginView.Password = "********";


                    return Redirect(ReturnUrl);
                }
            }

            ModelState.AddModelError("", "نام کاربری و یا رمز عبور شما نامعتبر است. لطفا دوباره سعی کنید");

            return View(loginView);
        }

        [Authorize]
        public ActionResult LogOut()
        {
            Response.Cookies.Remove(Utility.GetAuthCookieName());
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", null);
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult AddUser(VM_Login vmLogin, string ReturnUrl = "")
        { 
            UsersRepository repoUser = new UsersRepository();
            RolesRepository repoRole = new RolesRepository();

            VM_User VMUser = new VM_User()
            {
                Email = vmLogin.Email,
                LastName = vmLogin.LastName,
                FirstName = vmLogin.FirstName,
                Password = vmLogin.Password,
                PasswordConfirm = vmLogin.ConfirmPassword,
                UserName = vmLogin.UserName,
                FullName = vmLogin.FullName,
                Mobile = vmLogin.Mobile,
                IsActive = true
            };

            if (VMUser.Roles == null)
                VMUser.Roles = new List<int>();

            VMUser.FirstName = "";
            VMUser.LastName = VMUser.FullName;

            //اضافه کردن رل کاربر به ثبت نامی های جدید
            var userRole = repoRole.GetList().Where(p => p.RoleName == "Users").First();
            VMUser.Roles.Add(userRole.RoleId);

            //اضافه کردن حق دسرسی به سیستم وب جی آی اس
            var rolePermissions = new List<RolePermission>();
            rolePermissions.Add(new RolePermission() { RoleId = userRole .RoleId, PermissionKey = "WebGIS" });
            rolePermissions.Add(new RolePermission() { RoleId = userRole .RoleId, PermissionKey = "WebGIS:ManageWebGIS" });

            VM_User res = repoUser.Insert(VMUser, rolePermissions);

            //-----------------------------------احراز هویت و ورود به سیستم------------------
            CustomMembershipUser user = (CustomMembershipUser)Membership.GetUser(VMUser.UserName, false);
            if (user != null)
            {
                CustomSerializeModel userModel = new CustomSerializeModel()
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = user.Roles.Select(r => r.RoleName).ToList()

                };

                string userData = JsonConvert.SerializeObject(userModel);

                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1,
                    VMUser.UserName,
                    DateTime.Now,
                    DateTime.Now.AddMinutes((int)FormsAuthentication.Timeout.TotalMinutes),
                    VMUser.RememberMe ?? false,
                    userData
                    );

                string enTicket = FormsAuthentication.Encrypt(authTicket);

                HttpCookie faCookie = new HttpCookie(Utility.GetAuthCookieName(), enTicket) { Expires = DateTime.Now.AddMinutes((int)FormsAuthentication.Timeout.TotalMinutes) };

                Response.Cookies.Add(faCookie);

                VMUser.Password = "********";
            }

            if (!Url.IsLocalUrl(ReturnUrl) || ReturnUrl.ToLower().Replace("/", "") == "accountlogout")
            {
                ReturnUrl = "/";
            }
            //------------------------------------------------------------------------------


            return Redirect(ReturnUrl);
        }

    }
}
