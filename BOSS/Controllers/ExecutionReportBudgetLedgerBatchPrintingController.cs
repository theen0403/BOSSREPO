using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ExecutionReportBudgetLedgerBatchPrintingController : Controller
    {
        // GET: ExecutionReportBudgetLedgerBatchPrinting
        [Authorize]
        public ActionResult ReportBudgetLedgerBatchPrinting()
        {
            return View();
        }
    }
}