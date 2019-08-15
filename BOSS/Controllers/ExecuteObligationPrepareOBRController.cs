using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ExecuteObligationPrepareOBRController : Controller
    {
        // GET: ExecuteObligationPrepareOBR
        [Authorize]
        public ActionResult ObligationPrepareOBR()
        {
            return View();
        }
    }
}