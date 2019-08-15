using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ReviewQueriesPAOOController : Controller
    {
        // GET: ReviewQueriesPAOO
        [Authorize]
        public ActionResult QueriesPAOO()
        {
            return View();
        }
    }
}