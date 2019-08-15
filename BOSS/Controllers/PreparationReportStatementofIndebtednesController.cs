using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class PreparationReportStatementofIndebtednesController : Controller
    {
        // GET: PreparationReportStatementofIndebtednes
        [Authorize]
        public ActionResult ReportStatementofIndebtednes()
        {
            return View();
        }
    }
}