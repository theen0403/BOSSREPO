using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class RecordSAOOController : Controller
    {
        // GET: RecordSAOO
        [Authorize]
        public ActionResult RecordSAOO()
        {
            return View();
        }
    }
}