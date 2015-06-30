using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using IShop.AspNetIdentity;
using IShop.DataAccess.Context;
using IShop.Models;
using IShop.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Common.Web.Mvc.AspNetIdentity;

namespace IShop.Controllers.Areas.AZ.Controllers
{
    [Authorize]
    public class SecurityController : Controller
    {
        private readonly IAspNetIdentity<IShopContext, ApplicationUser, ApplicationUserManager, RoleManager<IdentityRole>> _aspNetIdentity;

        public SecurityController(IAspNetIdentity<IShopContext, ApplicationUser, ApplicationUserManager, RoleManager<IdentityRole>> aspNetIdentity)
        {
            _aspNetIdentity = aspNetIdentity;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult SignIn()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Order");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignIn(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _aspNetIdentity.SignInAsync(model.Login, model.Password, model.RememberMe);

                    if (Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Index", "Order");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Global", e.Message);
                }
            }

            return View(model);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _aspNetIdentity.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Global", e.Message);
                }
            }

            return View(model);
        }

        public async Task<ActionResult> ResetPassword(string id)
        {
            var user = await _aspNetIdentity.FindUserByIdAsync(id);

            return View(new ResetPasswordViewModel { Id = id, UserName = user.UserName });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _aspNetIdentity.ResetPasswordAsync(model.Id, model.NewPassword);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Global", e.Message);
                }
            }

            return View(model);
        }

        public async Task<ActionResult> Manage()
        {
            return View(new AdminUserManageViewModel { Email = await _aspNetIdentity.GetEmailAsync(User.Identity.GetUserId()) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(AdminUserManageViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _aspNetIdentity.SetEmailAsync(User.Identity.GetUserId(), model.Email);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Global", e.Message);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignOut()
        {
            _aspNetIdentity.SignOut();

            return RedirectToAction("SignIn");
        }
    }
}