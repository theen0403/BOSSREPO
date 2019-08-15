using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class PreparationPerOfficeController : Controller
    {
        // GET: PreparationPerOffice
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        //View Table For Program
        public ActionResult GetProgramlistDTable()
        {
            return PartialView("_TableProgramList");
        }
        //Add Program
        public ActionResult GetViewAddProgram(int ID)
        {
            return PartialView("_ViewAddProgram");
        }
        //Edit Program
        public ActionResult GetViewEditProgram(int ID)
        {
            return PartialView("_ViewEditProgram");
        }
        //View Table For Project
        public ActionResult GetProjectlistDTable()
        {
            return PartialView("_TableProjectList");
        }
        //Add Project
        public ActionResult GetViewAddProject(int ID)
        {
            return PartialView("_ViewAddProject");
        }
        //Edit Project
        public ActionResult GetViewEditProject(int ID)
        {
            return PartialView("_ViewEditProject");
        }
        //View Table For Activity
        public ActionResult GetActivitylistDTable()
        {
            return PartialView("_TableActivityList");
        }
        //Add Activity
        public ActionResult GetViewAddActivity(int ID)
        {
            return PartialView("_ViewAddActivity");
        }
        //Edit Activity
        public ActionResult GetViewEditActivity(int ID)
        {
            return PartialView("_ViewEditActivity");
        }
        //Add Attachment Activity
        public ActionResult GetViewAddAttachment(int ID)
        {
            return PartialView("_ViewAddAttachment");
        }
        //Attachment Activity
        public ActionResult GetViewAttachment(int ID)
        {
            return PartialView("_ViewAttachment");
        }
        public ActionResult GetViewPrintPreview(int ID)
        {
            return PartialView("_ViewPrintPreview");
        }
    }
}