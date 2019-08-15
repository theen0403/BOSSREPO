using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ExecutionAdjustmentofBudgetInterOfficeController : Controller
    {
        // GET: ExecutionAdjustmentofBudgetInterOffice
        [Authorize]
        public ActionResult AdjustmentofBudgetInterOffice()
        {
            return View();
        }
    }
}