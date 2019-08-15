using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class PreparationChecklistsFormLBA1BController : Controller
    {
        // GET: PreparationChecklistsFormLBA1B
        [Authorize]
        public ActionResult ChecklistsFormLBA1B()
        {
            return View();
        }
    }
}