using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ExecutionAllotmentReleasingofAllotmentController : Controller
    {
        // GET: ExecutionAllotmentReleasingofAllotment
        [Authorize]
        public ActionResult AllotmentReleasingofAllotment()
        {
            return View();
        }
    }
}