using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class PreparationAnnualInvestmentPlanController : Controller
    {
        // GET: PreparationAnnualInvestmentPlan
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        //Get Table list of Program Modal
        public ActionResult GetProgramlistDT()
        {
            return PartialView("_TableProgramList");
        }
        //View Program Details Modal
        public ActionResult GetProgramDetailsModal(int ID)
        {
            return PartialView("_ViewProgram");
        }
        //Get Table list of Project Modal
        public ActionResult GetProjectlistDT()
        {
            return PartialView("_TableProjectList");
        }
        //View Project Details Modal
        public ActionResult GetViewProjectDetail(int ID)
        {
            return PartialView("_ViewProject");
        }
        //Get Table list of Activity Modal
        public ActionResult GetActivitylistDT()
        {
            return PartialView("_TableActivityList");
        }
        //View Activity Details Modal
        public ActionResult GetViewActivityDetails(int ID)
        {
            return PartialView("_ViewActivity");
        }
       

    }
}
