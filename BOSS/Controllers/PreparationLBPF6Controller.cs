using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class PreparationLBPF6Controller : Controller
    {
        // GET: PreparationLBPF6
        [Authorize]
        public ActionResult LBPF6()
        {
            return View();
        }
    }
}