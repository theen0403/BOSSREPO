using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ReviewPAOOForLFCController : Controller
    {
        // GET: ReviewPAOOForLFC
        [Authorize]
        public ActionResult PAOOForLFC()
        {
            return View();
        }
    }
}