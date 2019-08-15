using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ReviewReportBudgetMatrixController : Controller
    {
        // GET: ReviewReportBudgetMatrix
        [Authorize]
        public ActionResult ReportBudgetMatrix()
        {
            return View();
        }
    }
}