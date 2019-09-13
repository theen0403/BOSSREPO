using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class FileMaintenancePayeeController : Controller
    {
        // GET: FileMaintenancePayee
        public ActionResult FilePayee()
        {
            return View();
        }
        //======================================
        //Department
        public ActionResult GetAddDeptModal()
        {
            return PartialView("Modals/_AddDepartmentModal");
        }
    }
}