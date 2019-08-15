using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ExecutionReportOBRTransmitalReportController : Controller
    {
        // GET: ExecutionReportOBRTransmitalReport
        [Authorize]
        public ActionResult ReportOBRTransmitalReport()
        {
            return View();
        }
    }
}