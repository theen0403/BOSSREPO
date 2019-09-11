using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class FileMaintenanceSectorController : Controller
    {
        // GET: FileMaintenanceSector
        public ActionResult FileSector()
        {
            return View();
        }
        public ActionResult SectorTab()
        {
            return PartialView("SectorTab/IndexSectorTab");
        }
        public ActionResult SubSectorTab()
        {
            return PartialView("SubSectorTab/IndexSubSectorTab");
        }
    }
}