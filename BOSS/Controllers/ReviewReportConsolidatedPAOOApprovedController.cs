﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ReviewReportConsolidatedPAOOApprovedController : Controller
    {
        // GET: ReviewReportConsolidatedPAOOApproved
        [Authorize]
        public ActionResult ReportConsolidatedPAOOApproved()
        {
            return View();
        }
    }
}