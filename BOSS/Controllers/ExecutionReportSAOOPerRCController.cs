using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ExecutionReportSAOOPerRCController : Controller
    {
        // GET: ExecutionReportSAOOPerRC
        [Authorize]
        public ActionResult ReportSAOOPerRC()
        {
            return View();
        }
    }
}