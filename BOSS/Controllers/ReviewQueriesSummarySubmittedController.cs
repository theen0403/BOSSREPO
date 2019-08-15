using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ReviewQueriesSummarySubmittedController : Controller
    {
        // GET: ReviewQueriesSummarySubmitted
        [Authorize]
        public ActionResult QueriesSummarySubmitted()
        {
            return View();
        }
    }
}