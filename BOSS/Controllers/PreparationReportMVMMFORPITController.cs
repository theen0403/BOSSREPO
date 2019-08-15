using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class PreparationReportMVMMFORPITController : Controller
    {
        // GET: PreparationReportMVMMFORPIT
        [Authorize]
        public ActionResult ReportMVMMFORPIT()
        {
            return View();
        }
    }
}