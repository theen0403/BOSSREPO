using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class PreparationReportAIPController : Controller
    {
        // GET: PreparationReportAIP
        [Authorize]
        public ActionResult ReportAIP()
        {
            return View();
        }
    }
}