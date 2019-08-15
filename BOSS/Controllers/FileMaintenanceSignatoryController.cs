using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class FileMaintenanceSignatoryController : Controller
    {
        // GET: FileMaintenanceSignatory
        [Authorize]
        public ActionResult FileSignatory()
        {
            return View();
        }
        //View Table For Signatory
        public ActionResult GetSignatoryDTable()
        {
            return PartialView("_TableFileMaintenanceSignatory");
        }
    }
}