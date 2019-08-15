using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class PreparationReportStatementofFundOperationController : Controller
    {
        // GET: PreparationReportStatementofFundOperation
        [Authorize]
        public ActionResult ReportStatementofFundOperation()
        {
            return View();
        }
    }
}