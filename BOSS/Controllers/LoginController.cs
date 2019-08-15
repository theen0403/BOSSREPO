using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using BOSS.Models;
using BOSS.Models.LoginModel;
using System.Security.Claims;
using System.Web.Routing;


namespace BOSS.Controllers
{
    public class LoginController : Controller
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public ActionResult LoginIndex()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            if (claims.Count() > 0)
            {
                var ActionName = "Index";
                var ControllerName = "Home";
                return RedirectToAction(ActionName, ControllerName);
            }
            else
            {
                return View();
            }
        }
        IAuthenticationManager Authentication
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LoginIndex(LoginModel log, string ReturnURL = "")
        {
            if (!ModelState.IsValid)
            {
                return View(log);
            }

            var user = BOSSDB.Accounts.Where(a => a.Username == log.username && a.IsLock == false).FirstOrDefault();
            if (user != null)
            {
                var validate = PasswordHash.PasswordHash.ValidatePassword(log.password, user.Password);
                if (validate == true)
                {
                    var identity = new System.Security.Claims.ClaimsIdentity(new[] { new Claim(ClaimTypes.Authentication, user.AccountID.ToString()) }, DefaultAuthenticationTypes.ApplicationCookie);

                    Authentication.SignIn(new AuthenticationProperties(), identity);

                    var ActionName = "Index";
                    var ControllerName = "Home";
                    return RedirectToAction(ActionName, ControllerName);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        public ActionResult ConfirmLogout()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            Authentication.SignOut();
            return RedirectToAction("LoginIndex", "Login");
        }

    }
}