using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class PreparationReportPersonnelScheduleController : Controller
    {
        // GET: PreparationReportPersonnelSchedule
        [Authorize]
        public ActionResult ReportPersonnelSchedule()
        {
            return View();
        }
    }
}