using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Persistence.Implementation;
using SuperSmart.Core.Persistence.Interface;
using SuperSmart.Host.Helper;
using System;
using System.Web.Mvc;
using System.Web.Security;

namespace SuperSmart.Host.Controllers
{
    public class VerificationController : Controller
    {
        IVerificationPersistence verificationPersistence = new VerificationPersistence();

        [HttpGet]
        public ActionResult Register()
        {
            return View("Register");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel registerViewModel)
        {
            try
            {
                verificationPersistence.Register(registerViewModel);
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.Merge(ex as PropertyExceptionCollection);
            }
            return View("Register");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View("Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            try
            {
                FormsAuthentication.SetAuthCookie(verificationPersistence.Login(loginViewModel), true);
                return Redirect(Request?.UrlReferrer?.ToString() ?? "/");
            }
            catch (Exception ex)
            {
                ModelState.Merge(ex as PropertyExceptionCollection);
            }
            return View("Login");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect(Request?.UrlReferrer?.ToString() ?? "/");
        }
    }
}