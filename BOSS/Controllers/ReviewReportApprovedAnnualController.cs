﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class ReviewReportApprovedAnnualController : Controller
    {
        // GET: ReviewReportApprovedAnnual
        [Authorize]
        public ActionResult ReportApprovedAnnual()
        {
            return View();
        }
    }
}