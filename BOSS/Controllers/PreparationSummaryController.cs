using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class PreparationSummaryController : Controller
    {
        // GET: PreparationSummary
        [Authorize]
        public ActionResult PreparationSummaryIndex()
        {
            return View();
        }

        public ActionResult GetViewRCenter()
        {
            return PartialView("_TableResponsibilityCenter");
        }
    }
}