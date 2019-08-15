using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ReviewListOfProposedController : Controller
    {
        // GET: ReviewListOfProposed
        [Authorize]
        public ActionResult ListOfProposed()
        {
            return View();
        }
    }
}