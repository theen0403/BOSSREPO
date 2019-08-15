using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ReviewReportStatementofSupplementalController : Controller
    {
        // GET: ReviewReportStatementofSupplemental
        [Authorize]
        public ActionResult ReportStatementofSupplemental()
        {
            return View();
        }
    }
}