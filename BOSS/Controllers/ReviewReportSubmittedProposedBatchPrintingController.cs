using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ReviewReportSubmittedProposedBatchPrintingController : Controller
    {
        // GET: ReviewReportSubmittedProposedBatchPrinting
        [Authorize]
        public ActionResult ReportSubmittedProposedBatchPrinting()
        {
            return View();
        }
    }
}