using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ExecuteAllotmentStatusOfAllotmentController : Controller
    {
        // GET: ExecuteAllotmentStatusOfAllotment
        [Authorize]
        public ActionResult AllotmentStatusOfAllotment()
        {
            return View();
        }
    }
}