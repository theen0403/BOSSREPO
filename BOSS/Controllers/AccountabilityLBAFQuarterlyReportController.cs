using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class AccountabilityLBAFQuarterlyReportController : Controller
    {
        // GET: AccountabilityLBAFQuarterlyReport
        [Authorize]
        public ActionResult LBAFQuarterlyReport()
        {
            return View();
        }
    }
}