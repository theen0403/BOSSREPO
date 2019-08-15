using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class PreparationLBPF2Controller : Controller
    {
        // GET: PreparationLBPF2
        [Authorize]
        public ActionResult LBPF2View()
        {
            return View();
        }
        public ActionResult GetViewAddAccount(int ID)
        {
            return PartialView("_ViewAddAccount");
        }
        public ActionResult GetViewUpdateSignatory(int ID)
        {
            return PartialView("_ViewUpdateSignatory");
        }
        public ActionResult GetViewPrintPreview(int ID)
        {
            return PartialView("_ViewPrintPreview");
        }
    }
}