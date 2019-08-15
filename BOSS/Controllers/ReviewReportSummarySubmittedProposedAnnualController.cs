using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ReviewReportSummarySubmittedProposedAnnualController : Controller
    {
        // GET: ReviewReportSummaryProposedAnnual
        [Authorize]
        public ActionResult ReportSummarySubmittedProposedAnnual()
        {
            return View();
        }
    }
}