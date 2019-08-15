using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class PreparationReportPSBatchPrintingController : Controller
    {
        // GET: PreparationReportPSBatchPrinting
        [Authorize]
        public ActionResult ReportPSBatchPrinting()
        {
            return View();
        }
    }
}