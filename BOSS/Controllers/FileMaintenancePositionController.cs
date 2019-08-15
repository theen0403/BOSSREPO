using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class FileMaintenancePositionController : Controller
    {
        // GET: FileMaintenancePosition
        [Authorize]
        public ActionResult FilePosition()
        {
            return View();
        }
        //View Table For Program
        public ActionResult GetPositionSPDTable()
        {
            return PartialView("_TableFileMaintenancePositionSP");
        }
        public ActionResult GetAddPosition()
        {
            return PartialView("_AddPosition");
        }
    }
}