﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ExecutionAdjustmentAdjustmentofOBRController : Controller
    {
        // GET: ExecutionAdjustmentAdjustmentofOBR
        [Authorize]
        public ActionResult AdjustmentAdjustmentofOBR()
        {
            return View();
        }
    }
}