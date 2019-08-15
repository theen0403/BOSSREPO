using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ReviewPAOOListOfProposedFinalizationController : Controller
    {
        // GET: ReviewPAOOListOfProposedFinalization
        [Authorize]
        public ActionResult PAOOListOfProposedFinalization()
        {
            return View();
        }
    }
}