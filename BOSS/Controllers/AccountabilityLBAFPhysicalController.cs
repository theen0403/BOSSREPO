using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{

    public class AccountabilityLBAFPhysicalController : Controller
    {
        // GET: AccountabilityLBAFPhysical
        [Authorize]
        public ActionResult LBAFPhysical()
        {
            return View();
        }
    }
}