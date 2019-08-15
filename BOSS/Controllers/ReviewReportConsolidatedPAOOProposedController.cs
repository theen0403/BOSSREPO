using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ReviewReportConsolidatedPAOOProposedController : Controller
    {
        // GET: ReviewReportConsolidatedPAOO
        [Authorize]
        public ActionResult ReportConsolidatedPAOOProposed()
        {
            return View();
        }
    }
}