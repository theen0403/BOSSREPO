using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ReviewReportStatementofFundController : Controller
    {
        // GET: ReviewReportStatementofFund
        [Authorize]
        public ActionResult ReportStatementofFund()
        {
            return View();
        }
    }
}