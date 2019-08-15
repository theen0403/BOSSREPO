using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ExecutionObligationApprovedOBRController : Controller
    {
        // GET: ExecutionObligationApprovedOBR
        [Authorize]
        public ActionResult ObligationApprovedOBR()
        {
            return View();
        }
    }
}