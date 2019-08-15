using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ReviewReportApprovedPAOOController : Controller
    {
        // GET: ReviewReportApprovedPAOO
        [Authorize]
        public ActionResult ReportApprovedPAOO()
        {
            return View();
        }
    }
}