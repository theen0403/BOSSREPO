using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ExecutionAdjustmentContinuingAppropriationController : Controller
    {
        // GET: ExecutionAdjustmentContinuingAppropriation
        [Authorize]
        public ActionResult AdjustmentContinuingAppropriation()
        {
            return View();
        }
    }
}