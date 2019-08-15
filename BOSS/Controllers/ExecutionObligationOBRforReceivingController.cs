using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ExecutionObligationOBRforReceivingController : Controller
    {
        // GET: ExecutionObligationOBRforReceiving
        [Authorize]
        public ActionResult ObligationOBRforReceiving()
        {
            return View();
        }
    }
}