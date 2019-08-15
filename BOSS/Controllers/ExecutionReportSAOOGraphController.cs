using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ExecutionReportSAOOGraphController : Controller
    {
        // GET: ExecutionReportSAOOGraph
        [Authorize]
        public ActionResult ReportSAOOGraph()
        {
            return View();
        }
    }
}