using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ExecutionAdjustmentPersonnelServicesSavingController : Controller
    {
        // GET: ExecutionAdjustmentPersonnelServicesSaving
        [Authorize]
        public ActionResult AdjustmentPersonnelServicesSaving()
        {
            return View();
        }
    }
}