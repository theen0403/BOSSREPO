using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ReviewListOfApprovedController : Controller
    {
        // GET: ReviewListOfApproved
        [Authorize]
        public ActionResult ListOfApproved()
        {
            return View();
        }
    }
}