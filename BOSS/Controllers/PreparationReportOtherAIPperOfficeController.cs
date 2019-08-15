using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class PreparationReportOtherAIPperOfficeController : Controller
    {
        // GET: PreparationReportOtherAIPperOffice
        [Authorize]
        public ActionResult ReportOtherAIPperOffice()
        {
            return View();
        }
    }
}