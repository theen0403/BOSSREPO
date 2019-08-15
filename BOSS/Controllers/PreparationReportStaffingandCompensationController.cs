using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class PreparationReportStaffingandCompensationController : Controller
    {
        // GET: PreparationReportStaffingandCompensation
        [Authorize]
        public ActionResult ReportStaffingandCompensation()
        {
            return View();
        }
    }
}