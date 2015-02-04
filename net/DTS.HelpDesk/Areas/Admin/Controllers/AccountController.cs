using DTS.DataTables.MVC;
using DTS.HelpDesk.Areas.Admin.Infrastructure.Helpers;
using DTS.HelpDesk.Areas.Admin.Infrastructure.Mailer;
using DTS.HelpDesk.Areas.Admin.Infrastructure.Repositories;
using DTS.HelpDesk.Areas.Admin.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DTS.HelpDesk.Areas.Admin.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
           //var roleStore = new RoleStore<IdentityRole>(context);
           // var roleManager = new RoleManager<IdentityRole>(roleStore);

        private ApplicationUserManager _userManager;
        private RoleManager<IdentityRole> _roleManger;
        private HelpDeskContext _context;
        private IQueryable<RoleViewModel> _allRoles;
        private string[] _reservedUsernames;

        private IMailer _mailer;


        public AccountController()
        {

            this._roleManger = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new HelpDeskContext()));
            this._allRoles = this._roleManger.Roles.ToList().Select(x => new RoleViewModel(x)).AsQueryable();
            this._mailer = new Mailer();
            this._reservedUsernames = ConfigurationHelper.GetAppSetting("dts.helpdesk.ReservedUsernames").ToString().Split(',');
        }

        public AccountController(ApplicationUserManager userManager, IMailer mailer)
        {

            UserManager = userManager;
            this._context = new HelpDeskContext();
            this._roleManger = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new HelpDeskContext()));
            this._mailer = mailer;
            this._allRoles = this._roleManger.Roles.ToList().Select(x => new RoleViewModel(x)).AsQueryable();
            this._reservedUsernames = ConfigurationHelper.GetAppSetting("dts.helpdesk.ReservedUsernames").ToString().Split(',');
        }

        public ApplicationUserManager UserManager {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }



        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            ViewBag.Columns = GetAccountColumns();
            return View("Index");
        }

        private string GetAccountColumns()
        {
            return JsonConvert.SerializeObject(new List<ColumnHeader>() { 
                new ColumnHeader() { data = "Id", name = "", visible = true, sortable = false, width = "50px", className = "text-center"  },
                new ColumnHeader() { data = "DirectoryName", name = "Name", visible = true, sortable = true },
                new ColumnHeader() { data = "Email", name = "Email", visible = true, sortable = true },
                new ColumnHeader() { data = "Roles", name = "Roles", visible = true, sortable = true },
                new ColumnHeader() { data = "IsLockedOut", name = "Locked Out", visible = true, sortable = true }
            });
        }


        [HttpPost]
        public JsonResult GetData([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            // get all users
            var allRoles = this._roleManger.Roles.ToList().Select(x => new RoleViewModel(x)).AsQueryable();

            var data = UserManager.Users.ToList().Select(x => new ProfileViewModel(x, allRoles));
            var filteredData = data;
            var searchValue = "";
            // filter data
            if (!string.IsNullOrWhiteSpace(requestModel.Search.Value))
            {
                searchValue = requestModel.Search.Value.ToLower();
                filteredData = filteredData.Where(x => 
                    x.DirectoryName.ToLower().Contains(searchValue) || 
                    x.Roles.ToDelimitedString(y => y.Name).Contains(searchValue) || 
                    x.Email.ToLower().Contains(searchValue));
            }

            // get sorting
            var sortColumns = string.Join(", ",
                requestModel.Columns.GetSortedColumns()
                .Select(
                    x => string.Format("{0} {1}", x.Data, x.SortDirection.ToString().ToLower().Contains("asc") ? "ASC" : "DESC")
                ).ToArray());

            // get paged data
            var pagedData = filteredData
                .OrderBy(sortColumns)
                .Skip(requestModel.Start)
                .Take(requestModel.Length);

            var result = new DataTablesResponse(requestModel.Draw, pagedData, data.Count(), data.Count());
            return Json(result);
        }


        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            AddToViewBag();
            var model = new ProfileViewModel();
            return View("Create", model);
        }

        private void AddToViewBag()
        {
            ViewBag.PossibleRoles = this._allRoles.ToList();
            ViewBag.YesOrNo = new List<SelectListItem>() { 
                new SelectListItem() { Text = "No", Value = false.ToString() },
                new SelectListItem() { Text = "Yes", Value = true.ToString() }
            };


            var timeZoneIds = TimeZoneInfo.FindSystemTimeZoneById("U.S. Mountain Standard Time");//.GetSystemTimeZones();
            ViewBag.PossibleTimezones = timeZoneIds;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProfileViewModel model)
        {
            // remove id, will assign it before saving
            ModelState.Remove("Id");

            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }

            // create new users
            model.Id = Guid.NewGuid().ToString();
            var user = new ApplicationUser();

            // hash password
            var hasher = new PasswordHasher();
            var password = model.Password;
            model.Password = hasher.HashPassword(model.Password);
            model.RequirePasswordReset = true;
            model.PasswordResetToken = UserManager.GeneratePasswordResetToken(user.Id);

            user.Update(model);
            UserManager.Create(user, password);

            // add users roles
            foreach (var roleId in model.RoleIds)
            {
                var roleName = this._roleManger.FindById(roleId).Name;
                UserManager.AddToRole(user.Id, roleName);
            }

            // kick off email to user that account was created
            _mailer.AccountCreated(user.Email, user.Email, password).Send();

            return RedirectToAction("Index", "Account", new { area = "Admin" });
        }


        [Authorize(Roles = "admin")]
        public ActionResult Edit(string id)
        {
            var user = UserManager.FindById(id);
            if (user != null)
            {
                AddToViewBag();
                var model = new ProfileViewModel(user, this._allRoles);
                return View("Edit", model);
            }
            return RedirectToAction("Index", "Error", new { area = "Admin" });        
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProfileViewModel model, string toDo = "")
        {
            // handle delete
            if (toDo.ToLower() == "delete")
            {
                var user = UserManager.FindById(model.Id);
                if (!this._reservedUsernames.Contains(user.Email))
                {

                    //UserManager.ClearAllRoles(user.Id);
                    UserManager.Delete(user);
                    return RedirectToAction("Index");
                }
                AddToViewBag();
                ModelState.AddModelError("Email", "Account is reserved and cannot be deleted.");
                return View("Edit", model);
            }


            // remove passwords if it's empty
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
            }

            if (ModelState.IsValid)
            {
                // update user
                var user = UserManager.FindById(model.Id);
                user.Update(model);
                UserManager.Update(user);

                // update password if changing
                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    var hasher = new PasswordHasher();
                    UserManager.RemovePassword(user.Id);
                    UserManager.AddPassword(user.Id, model.Password);

                    user.PasswordResetToken = UserManager.GeneratePasswordResetToken(user.Id);
                    user.RequirePasswordReset = true;
                    UserManager.Update(user);
                }


                // udpate, remove all then add back selected ones
                UserManager.ClearAllRoles(user.Id);
                foreach (var roleId in model.RoleIds)
                {
                    var roleName = this._roleManger.FindById(roleId).Name;
                    UserManager.AddToRole(user.Id, roleName);
                }


                // almost done, send email if admin changed the password, would be rude not to alert user of this change.
                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    _mailer.AccountUpdated(user.Email, user.Email, model.Password).Send();
                }

                return RedirectToAction("Index", new { controller = "Account", area = "Admin" });
            }

            return View("Edit", model);
        }



        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.Email, model.Password);

                if (user != null)
                {
                    if (user.LockoutEnabled)
                    {
                        ModelState.AddModelError("", "Sorry, your account is locked. Contact admin@dtsagile.com for assistance.");
                        return View("Login", model);
                    }

                    if (user.RequirePasswordReset)
                    {
                        return RedirectToAction("ResetPassword", new { controller = "Account", area = "Admin", code = user.PasswordResetToken });
                    }


                    await SignInAsync(user, model.RememberMe);
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToAction("Index", new { controller = "Admin", area = "Admin" });
                    }
                    
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View("Login", model);
        }


        [AllowAnonymous]
        public ActionResult Forgot()
        {
            return View("Forgot");
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Forgot(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    ModelState.AddModelError("", "The user either does not exist or is not confirmed.");
                    return View("Forgot");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // generate a reset token, update user
                string resetToken = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                user.PasswordResetToken = resetToken;
                await UserManager.UpdateAsync(user);

                // construct the email
                await this._mailer.ForgotPassword(user.Email, resetToken).SendAsync();             
                
                //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View("Forgot", model);
        }

        
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
	

        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            // validate code
            var isValid = UserManager.Users.Any(x => x.PasswordResetToken == code);

            if (!isValid) 
            {
                return RedirectToAction("Index", new { controller = "Error", area = "" });
            }

            var model = new ResetPasswordViewModel()
            {
                Code = code,
            };

            return View("ResetPassword", model);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("", "No user found.");
                    return View();
                }

                if (model.Code != user.PasswordResetToken)
                {
                    return RedirectToAction("Error", new { controller = "Error", area = "Admin" });

                }


                IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
               
                if (result.Succeeded)
                {

                    // get user again, remove password reset token and save
                    user = await UserManager.FindByNameAsync(model.Email);
                    user.PasswordResetToken = "";
                    user.RequirePasswordReset = false;
                    await UserManager.UpdateAsync(user);

                    // log in user
                    await SignInAsync(user, false);


                    return RedirectToAction("Index", new { controller = "Admin", area = "Admin"} );
                }
                else
                {
                    AddErrors(result);
                    return View();
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                await SignInAsync(user, isPersistent: false);
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }


        public async Task<ActionResult> Manage()
        {
            AddToViewBag();
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            return View("Manage", new ProfileViewModel(user, this._allRoles));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(ProfileViewModel model)
        {
            // is user changing password?
            if (string.IsNullOrWhiteSpace(model.Password)) 
            {
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
            }
        
            // validate
            if (ModelState.IsValid)
            {
                // update user info
                var user = UserManager.FindById(model.Id);
                user.Update(model);
                UserManager.Update(user);

                // now reset the password if changing
                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    // remove password
                    UserManager.RemovePassword(user.Id);

                    // add new password
                    UserManager.AddPassword(user.Id, model.Password);
                }


                // update roles 
                if (User.IsInRole("admin"))
                {
                    // clear all roles
                    UserManager.ClearAllRoles(user.Id);

                    // add back selected ones
                    foreach (var roleId in model.RoleIds)
                    {
                        var roleName = this._roleManger.FindById(roleId).Name;
                        UserManager.AddToRole(user.Id, roleName);
                    }
                }

                return RedirectToAction("Profile", new { conroller = "Account", area = "Admin" });

            }
            return View("Manage", model);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", new { controller = "Account", area = "Admin" });
        }


        public async Task<ActionResult> Profile()
        {
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            return View("Profile", new ProfileViewModel(user, this._allRoles));

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private void SendEmail(string email, string callbackUrl, string subject, string message)
        {
            // For information on sending mail, please visit http://go.microsoft.com/fwlink/?LinkID=320771
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}