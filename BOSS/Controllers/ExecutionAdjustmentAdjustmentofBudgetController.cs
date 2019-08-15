using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ExecutionAdjustmentAdjustmentofBudgetController : Controller
    {
        // GET: ExecutionAdjustmentAdjustmentofBudget
        [Authorize]
        public ActionResult AdjustmentAdjustmentofBudget()
        {
            return View();
        }
    }
}