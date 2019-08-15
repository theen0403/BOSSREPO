using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class PreparationChecklistFormLBR1BController : Controller
    {
        // GET: PreparationChecklistFormLBR1B
        [Authorize]
        public ActionResult ChecklistFormLBR1B()
        {
            return View();
        }
    }
}