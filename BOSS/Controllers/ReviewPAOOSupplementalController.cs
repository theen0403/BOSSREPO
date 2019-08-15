using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ReviewPAOOSupplementalController : Controller
    {
        // GET: ReviewPAOOSupplemental
        [Authorize]
        public ActionResult PAOOSupplemental()
        {
            return View();
        }
    }
}