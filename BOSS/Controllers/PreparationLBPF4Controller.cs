using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class PreparationLBPF4Controller : Controller
    {
        // GET: PreparationLBPF4
        [Authorize]
        public ActionResult LBPF4()
        {
            return View();
        }
    }
}