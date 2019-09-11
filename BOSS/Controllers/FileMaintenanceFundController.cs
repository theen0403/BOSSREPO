using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class FileMaintenanceFundController : Controller
    {
        // GET: FileMaintenanceFund
        public ActionResult FileFund()
        {
            return View();
        }
        public ActionResult FundTab()
        {
            return PartialView("FundTab/IndexFundTab");
        }
        public ActionResult SubFundTab()
        {
            return PartialView("SubFundTab/IndexSubFundTab");
        }
    }
}