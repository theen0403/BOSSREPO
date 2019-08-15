using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class PreparationChecklistsFormLBA1AController : Controller
    {
        // GET: ReviewChecklistsFormLBA1A
        [Authorize]
        public ActionResult ChecklistsFormLBA1A()
        {
            return View();
        }
    }
}