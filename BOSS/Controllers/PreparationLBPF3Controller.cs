using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class PreparationLBPF3Controller : Controller
    {
        // GET: PreparationLBPF3
        [Authorize]
        public ActionResult LBPF3View()
        {
            return View();
        }
    }
}