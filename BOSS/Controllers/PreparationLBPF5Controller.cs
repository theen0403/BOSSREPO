using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class PreparationLBPF5Controller : Controller
    {
        // GET: PreparationLBPF5
        [Authorize]
        public ActionResult LBPF5()
        {
            return View();
        }
        //ViewContext New Credit Modal
        public ActionResult GetViewNewCredit(int ID)
        {
            return PartialView("_ViewNewCredit");
        }
    }
}