using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ReviewReportProposedPAOOController : Controller
    {
        // GET: ReviewReportProposedPAOO
        [Authorize]
        public ActionResult ReportProposedPAOO()
        {
            return View();
        }
    }
}