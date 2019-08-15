using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class PreparationLBPF9Controller : Controller
    {
        // GET: PreparationLBPF9
        [Authorize]
        public ActionResult LBPF9()
        {
            return View();
        }
    }
}