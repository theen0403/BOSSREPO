using BOSS.GlobalFunctions;
using BOSS.Models;
using BOSS.Models.BPmodels.AIPmodels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class PreparationPerOfficeController : Controller
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        DatatypeValidation GlobalFunction = new DatatypeValidation();
        // GET: PreparationPerOffice
        [Authorize]
        public ActionResult Index()
        {
            PPAperOfficeModel model = new PPAperOfficeModel();
            model.DeptList = new SelectList(BOSSDB.Tbl_FMRes_Department, "DeptID", "DeptTitle");
            model.DeptList = (from li in model.DeptList orderby li.Text select li).ToList();
            return View(model);
        }

        public ActionResult GetIndexProgram()
        {
            return PartialView("1PROGRAM/IndexProgram");
        }
        public ActionResult GetProgramForm(int ActionID, int ProgramID)
        {
            PPAperOfficeModel model = new PPAperOfficeModel();

            if (ActionID == 2)
            {
                var progTbl = (from a in BOSSDB.BPAIP_PPAperOffice where a.ProgramID == ProgramID select a).FirstOrDefault();
                var ppaAmounttbl = (from a in BOSSDB.BP_PPA_Amount where a.PPACCCostID == ProgramID select a).FirstOrDefault();

                model.PPAperOfficeList.SectorID = GlobalFunction.ReturnEmptyInt(progTbl.Tbl_FMSector_SubSector.Tbl_FMSector_Sector.SectorID);
                model.PPAperOfficeList.SubSectorID = GlobalFunction.ReturnEmptyInt(progTbl.SubSectorID);
                model.PPAperOfficeList.AIPCode = GlobalFunction.ReturnEmptyString(progTbl.AIPCode);
                model.PPAperOfficeList.ProgDescription = GlobalFunction.ReturnEmptyString(progTbl.ProgDescription);
                model.PPAperOfficeList.ProgType = GlobalFunction.ReturnEmptyString(progTbl.ProgType);
                model.PPAperOfficeList.FundID = GlobalFunction.ReturnEmptyInt(progTbl.FundID);
                model.PPAperOfficeList.ProgStartDate = GlobalFunction.ReturnEmptyDateTime(progTbl.ProgStartDate);
                model.PPAperOfficeList.ProgCompletionDate = GlobalFunction.ReturnEmptyDateTime(progTbl.ProgCompletionDate);
                model.PPAperOfficeList.ProgPolicyObjective = GlobalFunction.ReturnEmptyString(progTbl.ProgPolicyObjective);
                model.PPAperOfficeList.ExpectedOutput = GlobalFunction.ReturnEmptyString(progTbl.ExpectedOutput);
                model.PPAperOfficeList.ProgOImplementingOffice = GlobalFunction.ReturnEmptyString(progTbl.ProgOImplementingOffice);
                model.PPAperOfficeList.PS = GlobalFunction.ReturnEmptyString(ppaAmounttbl.PS);
                model.PPAperOfficeList.MOOE = GlobalFunction.ReturnEmptyString(ppaAmounttbl.MOOE);
                model.PPAperOfficeList.CO = GlobalFunction.ReturnEmptyString(ppaAmounttbl.CO);
                model.PPAperOfficeList.OFExpense = GlobalFunction.ReturnEmptyString(ppaAmounttbl.OFExpense);
                model.PPAperOfficeList.PPATotal = GlobalFunction.ReturnEmptyDecimal(ppaAmounttbl.PPATotal);
                model.PPAperOfficeList.CCAdoptation = GlobalFunction.ReturnEmptyString(ppaAmounttbl.CCAdoptation);
                model.PPAperOfficeList.CCTypologyCode = GlobalFunction.ReturnEmptyString(ppaAmounttbl.CCTypologyCode);
                model.PPAperOfficeList.PPACCCostID = ppaAmounttbl.PPACCCostID;
                model.PPAperOfficeList.ProgramID = progTbl.ProgramID;

                var getSubSector = (from a in BOSSDB.Tbl_FMSector_SubSector where a.SectorID == model.PPAperOfficeList.SectorID select a).ToList();
                foreach (var item in getSubSector)
                {
                    model.SubSectorList.Add(new SelectListItem() { Text = item.SubSectorTitle, Value = item.SubSectorID.ToString() });
                }
                model.SubSectorList.Add(new SelectListItem() { Text = "N/A", Value = "0" });
            }
            else
            {
                var firstSector = (from a in BOSSDB.Tbl_FMSector_Sector orderby a.SectorTitle select a).FirstOrDefault();
                var getSubSector = (from a in BOSSDB.Tbl_FMSector_SubSector where a.SectorID == firstSector.SectorID select a).ToList();
                foreach (var item in getSubSector)
                {
                    model.SubSectorList.Add(new SelectListItem() { Text = item.SubSectorTitle, Value = item.SubSectorID.ToString() });
                }
                model.SubSectorList.Add(new SelectListItem() { Text = "N/A", Value = "000" });
            }
            model.FundList = new SelectList(BOSSDB.Tbl_FMFund_Fund, "FundID", "FundTitle");
            model.FundList = (from li in model.FundList orderby li.Text select li).ToList();

            model.SectorList = new SelectList(BOSSDB.Tbl_FMSector_Sector, "SectorID", "SectorTitle");
            model.SectorList = (from li in model.SectorList orderby li.Text select li).ToList();

            model.ActionID = ActionID;

            return PartialView("1PROGRAM/_FormProgram", model);
        }
        public ActionResult GetProgramlistDTable()
        {
            PPAperOfficeModel model = new PPAperOfficeModel();
            var SQLQuery = @"SELECT [ProgramID]
                         ,[AIPCode]
                         ,[ProgDescription]
                         ,[BPAIP_PPAperOffice].[PPACCCostID]
	                     ,PPATotal
                        FROM [BOSS].[dbo].[BPAIP_PPAperOffice], BP_PPA_Amount
                        where [BPAIP_PPAperOffice].[PPACCCostID] = BP_PPA_Amount.PPACCCostID";

            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_FMResponsibility]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        model.getPPAperOfficeList.Add(new PPAperOfficeList()
                        {
                            ProgramID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            AIPCode = GlobalFunction.ReturnEmptyString(dr[1]),
                            ProgDescription = GlobalFunction.ReturnEmptyString(dr[2]),
                            PPATotal = GlobalFunction.ReturnEmptyDecimal(dr[4])
                        });
                    }
                }
                Connection.Close();
            }
            return PartialView("1PROGRAM/_TableProgram", model.getPPAperOfficeList);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveProgramPPA(PPAperOfficeModel model)
        {
            var isExist = "";

            if (ModelState.IsValid)
            {
                var ProgDescription = GlobalFunction.RemoveSpaces(model.PPAperOfficeList.ProgDescription);
                var ProgramID = model.PPAperOfficeList.ProgramID;
                var PPACCCostID = model.PPAperOfficeList.PPACCCostID;
                var deptID = model.PPAperOfficeList.DeptID;
                var sectorID = model.PPAperOfficeList.SectorID;
                int? subSectorID = model.PPAperOfficeList.SubSectorID;
                if (subSectorID == 000 || subSectorID == 0)
                {
                    subSectorID = null;
                }
                else
                {
                    subSectorID = model.PPAperOfficeList.SubSectorID;
                }

                List<BPAIP_PPAperOffice> checkProgram = (from a in BOSSDB.BPAIP_PPAperOffice where (a.ProgramID == ProgramID && a.PPACCCostID == PPACCCostID && a.SubSectorID == subSectorID) select a).ToList();
                BPAIP_PPAperOffice progTbl = (from a in BOSSDB.BPAIP_PPAperOffice where a.ProgramID == ProgramID select a).FirstOrDefault();
                var save = false;
                if (checkProgram.Count > 0)
                {
                    foreach (var item in checkProgram)
                    {
                        if (progTbl != null)
                        {
                            List<BPAIP_PPAperOffice> progDecCount = (from e in BOSSDB.BPAIP_PPAperOffice where e.ProgDescription == ProgDescription select e).ToList();
                            if (progTbl.ProgDescription != ProgDescription && progDecCount.Count >= 1)
                            {
                                save = false;
                            }
                            else
                            {
                                save = true;
                                break;
                            }
                        }
                        else
                        {
                            if (GlobalFunction.AutoCaps_RemoveSpaces(item.ProgDescription) != GlobalFunction.AutoCaps_RemoveSpaces(ProgDescription))
                            {
                                save = true;
                            }
                            else
                            {
                                save = false;
                                break;
                            }
                        }

                    }
                }
                else
                {
                    save = true;
                }

                switch (save)
                {
                    case true:
                        switch (model.ActionID)
                        {
                            case 1:
                                BP_PPA_Amount ppaamountadd = new BP_PPA_Amount();
                                ppaamountadd.PS = model.PPAperOfficeList.PS;
                                ppaamountadd.CO = model.PPAperOfficeList.CO;
                                ppaamountadd.MOOE = model.PPAperOfficeList.MOOE;
                                ppaamountadd.OFExpense = model.PPAperOfficeList.OFExpense;
                                ppaamountadd.PPATotal = Convert.ToString(model.PPAperOfficeList.PPATotal);
                                ppaamountadd.CCAdoptation = model.PPAperOfficeList.CCAdoptation;
                                ppaamountadd.CCMitigation = model.PPAperOfficeList.CCMitigation;
                                ppaamountadd.CCTypologyCode = model.PPAperOfficeList.CCTypologyCode;
                                BOSSDB.BP_PPA_Amount.Add(ppaamountadd);
                                BOSSDB.SaveChanges();

                                BPAIP_PPAperOffice progAdd = new BPAIP_PPAperOffice();
                                progAdd.DeptID = deptID;
                                progAdd.BudgetYear = model.PPAperOfficeList.BudgetYear;
                                progAdd.isInfrastructure = model.PPAperOfficeList.isInfrastructure;
                                progAdd.AIPCode = model.PPAperOfficeList.AIPCode;
                                progAdd.ProgDescription = ProgDescription;
                                progAdd.ProgType = model.PPAperOfficeList.ProgType;
                                progAdd.ProgStartDate = Convert.ToDateTime(model.PPAperOfficeList.ProgStartDate);
                                progAdd.ProgCompletionDate = Convert.ToDateTime(model.PPAperOfficeList.ProgCompletionDate);
                                progAdd.ProgPolicyObjective = model.PPAperOfficeList.ProgPolicyObjective;
                                progAdd.ExpectedOutput = model.PPAperOfficeList.ExpectedOutput;
                                progAdd.ProgOImplementingOffice = model.PPAperOfficeList.ProgOImplementingOffice;
                                progAdd.SubSectorID = model.PPAperOfficeList.SubSectorID;
                                progAdd.PPACCCostID = model.PPAperOfficeList.PPACCCostID;

                                BOSSDB.BPAIP_PPAperOffice.Add(progAdd);
                                BOSSDB.SaveChanges();
                                isExist = "false";
                                break;

                            case 2:
                                BP_PPA_Amount ppaamountupdate = new BP_PPA_Amount();
                                ppaamountupdate.PS = model.PPAperOfficeList.PS;
                                ppaamountupdate.CO = model.PPAperOfficeList.CO;
                                ppaamountupdate.MOOE = model.PPAperOfficeList.MOOE;
                                ppaamountupdate.OFExpense = model.PPAperOfficeList.OFExpense;
                                ppaamountupdate.PPATotal = Convert.ToString(model.PPAperOfficeList.PPATotal);
                                ppaamountupdate.CCAdoptation = model.PPAperOfficeList.CCAdoptation;
                                ppaamountupdate.CCMitigation = model.PPAperOfficeList.CCMitigation;
                                ppaamountupdate.CCTypologyCode = model.PPAperOfficeList.CCTypologyCode;
                                BOSSDB.Entry(ppaamountupdate);

                                BPAIP_PPAperOffice progupdate = new BPAIP_PPAperOffice();
                                progupdate.DeptID = deptID;
                                progupdate.BudgetYear = model.PPAperOfficeList.BudgetYear;
                                progupdate.isInfrastructure = model.PPAperOfficeList.isInfrastructure;
                                progupdate.AIPCode = model.PPAperOfficeList.AIPCode;
                                progupdate.ProgDescription = ProgDescription;
                                progupdate.ProgType = model.PPAperOfficeList.ProgType;
                                progupdate.ProgStartDate = Convert.ToDateTime(model.PPAperOfficeList.ProgStartDate);
                                progupdate.ProgCompletionDate = Convert.ToDateTime(model.PPAperOfficeList.ProgCompletionDate);
                                progupdate.ProgPolicyObjective = model.PPAperOfficeList.ProgPolicyObjective;
                                progupdate.ExpectedOutput = model.PPAperOfficeList.ExpectedOutput;
                                progupdate.ProgOImplementingOffice = model.PPAperOfficeList.ProgOImplementingOffice;
                                progupdate.SubSectorID = model.PPAperOfficeList.SubSectorID;
                                progupdate.PPACCCostID = model.PPAperOfficeList.PPACCCostID;

                                BOSSDB.Entry(progupdate);
                                BOSSDB.SaveChanges();
                                isExist = "justUpdate";
                                break;
                        }
                        break;
                    default:
                        isExist = "true";
                        break;
                }
            }
            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new { isExist = isExist }
            };
        }




        public ActionResult GetViewEditProgram(int ID)
        {
            return PartialView("1PROGRAM/_ViewEditProgram");
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