using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class PreparationReportPlantillaController : Controller
    {
        // GET: PreparationReportPlantilla
        [Authorize]
        public ActionResult ReportPlantilla()
        {
            return View();
        }
    }
}