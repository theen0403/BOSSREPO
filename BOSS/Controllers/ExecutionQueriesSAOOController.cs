using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ExecutionQueriesSAOOController : Controller
    {
        // GET: ExecutionQueriesSAOO
        [Authorize]
        public ActionResult QueriesSAOO()
        {
            return View();
        }
    }
}