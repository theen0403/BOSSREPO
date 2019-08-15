using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class RecordsBudgetLedgerOOEController : Controller
    {
        // GET: RecordsBudgetLedgerOOE
        [Authorize]
        public ActionResult BudgetLedgerOOE()
        {
            return View();
        }
    }
}