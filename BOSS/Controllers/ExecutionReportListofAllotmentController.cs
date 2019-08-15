using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ExecutionReportListofAllotmentController : Controller
    {
        // GET: ExecutionReportListofAllotment
        [Authorize]
        public ActionResult ReportListofAllotment()
        {
            return View();
        }
    }
}