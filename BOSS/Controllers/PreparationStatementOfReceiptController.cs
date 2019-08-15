using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class PreparationStatementOfReceiptController : Controller
    {
        // GET: PreparationStatementOfReceipt
        [Authorize]
        public ActionResult ReportStatementOfReceipt()
        {
            return View();
        }
    }
}