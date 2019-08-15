using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class PreparationReportSummaryOfPersonnelController : Controller
    {
        // GET: PreparationReportSummaryOfPersonnel
        [Authorize]
        public ActionResult ReportSummaryOfPersonnel()
        {
            return View();
        }
    }
}