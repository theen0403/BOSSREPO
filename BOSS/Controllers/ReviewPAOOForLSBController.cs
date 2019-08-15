using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ReviewPAOOForLSBController : Controller
    {
        // GET: ReviewPAOOForLSB
        [Authorize]
        public ActionResult ReviewPAOOForLSB()
        {
            return View();
        }
    }
}