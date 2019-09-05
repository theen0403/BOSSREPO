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
        public ActionResult GetViewAddStatutory(int ID)
        {
            return PartialView("_ViewAddStatutoryContractualObligation");
        }
        public ActionResult GetViewAddBudgetary(int ID)
        {
            return PartialView("_ViewAddBudgetaryRequirement");
        }
    }
}