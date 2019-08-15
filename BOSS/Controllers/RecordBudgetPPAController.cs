using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class RecordBudgetPPAController : Controller
    {
        // GET: RecordBudgetPPA
        [Authorize]
        public ActionResult BudgetPPA()
        {
            return View();
        }
    }
}