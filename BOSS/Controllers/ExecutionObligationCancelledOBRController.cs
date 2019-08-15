using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ExecutionObligationCancelledOBRController : Controller
    {
        // GET: ExecutionObligationCancelledOBR
        [Authorize]
        public ActionResult ObligationCancelledOBR()
        {
            return View();
        }
    }
}