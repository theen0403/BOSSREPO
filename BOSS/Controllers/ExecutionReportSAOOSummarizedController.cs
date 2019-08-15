using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ExecutionReportSAOOSummarizedController : Controller
    {
        // GET: ExecutionReportSAOOSummarized
        [Authorize]
        public ActionResult ReportSAOOSummarized()
        {
            return View();
        }
    }
}