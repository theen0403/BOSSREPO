﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ExecutionObligationOBRforTransmitalController : Controller
    {
        // GET: ExecutionObligationOBRforTransmital
        [Authorize]
        public ActionResult ObligationOBRforTransmital()
        {
            return View();
        }
    }
}