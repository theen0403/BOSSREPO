using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ExecutionAdjustmentListofAdjustedOBRController : Controller
    {
        // GET: ExecutionAdjustmentListofAdjustedOBR
        [Authorize]
        public ActionResult AdjustmentListofAdjustedOBR()
        {
            return View();
        }
    }
}