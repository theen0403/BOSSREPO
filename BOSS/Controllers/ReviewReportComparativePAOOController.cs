using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ReviewReportComparativePAOOController : Controller
    {
        // GET: ReviewReportComparativePAOO
        [Authorize]
        public ActionResult ReviewReportComparativePAOOIndex()
        {
            return View();
        }
    }
}