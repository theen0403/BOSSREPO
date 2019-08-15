using BOSS.Models;
using BOSS.Models.BarModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class BarController : Controller
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public ActionResult GetSideBar()
        {

            return PartialView("_Sidebar");
        }
        public ActionResult GetNavBar(BarModel model)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            Guid id = Guid.Parse(claims.ElementAt(0).Value);

            Account tbl_Accounts = (from e in BOSSDB.Accounts where e.AccountID == id select e).FirstOrDefault();

            model.FullName = tbl_Accounts.PersonalInformation.Fullname;
            return PartialView("_Navbar", model);
        }
    }
}