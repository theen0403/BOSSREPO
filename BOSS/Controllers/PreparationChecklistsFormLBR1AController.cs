﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class PreparationChecklistsFormLBR1AController : Controller
    {
        // GET: PreparationChecklistsFormLBR1A
        [Authorize]
        public ActionResult ChecklistsFormLBR1A()
        {
            return View();
        }
    }
}