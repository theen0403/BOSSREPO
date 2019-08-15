using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ReviewReportApprovedPAOOBatchPrintingController : Controller
    {
        // GET: ReviewReportApprovedPAOOBatchPrinting
        [Authorize]
        public ActionResult ReviewReportApprovedPAOOBatchPrinting()
        {
            return View();
        }
    }
}