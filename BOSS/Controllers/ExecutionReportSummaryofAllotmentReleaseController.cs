﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ExecutionReportSummaryofAllotmentReleaseController : Controller
    {
        // GET: ExecutionReportSummaryofAllotmentRelease
        [Authorize]
        public ActionResult ReportSummaryofAllotmentRelease()
        {
            return View();
        }
    }
}