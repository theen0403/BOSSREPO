using BOSS.GlobalFunctions;
using BOSS.Models;
using BOSS.Models.FMResponsibilityModels;
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
    public class FileMaintenanceResponsibilityController : Controller
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        DatatypeValidation GlobalFunction = new DatatypeValidation();
        // GET: FileMaintenanceResponsibility
        [Authorize]
        public ActionResult FileResponsibility()
        {
            FunctionModel model = new FunctionModel();
            return View(model);
        }

        public ActionResult DepartmentTab()
        {
            DepartmentModel model = new DepartmentModel();
            return PartialView("DepartmentTab/DepartmentTab", model);
        }

        public ActionResult FunctionTab()
        {
            FunctionModel model = new FunctionModel();
            return PartialView("FunctionTab/FunctionTab", model);
        }
        public ActionResult OfficeSectionTab()
        {
            OfficeSectionModel model = new OfficeSectionModel();
            return PartialView("OfficeSectionTab/SectionTab", model);
        }

        //===============================================================================================
        //View Table For Department Table
        public ActionResult GetDepartmentDT()
        {
            DepartmentModel model = new DepartmentModel();

            List<DepartmentList> getDeptList = new List<DepartmentList>();
            
            var SQLQuery = "SELECT [DeptID], [DeptTitle], [DeptAbbrv], [DeptOfficeCode], [SectorTitle], [FundTitle], [SubSectorID] FROM [Tbl_FMDepartment], [FundType], [Sector] where[FundType].FundID = [Tbl_FMDepartment].FundID and[Sector].SectorID = [Tbl_FMDepartment].SectorID";
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
                        getDeptList.Add(new DepartmentList()
                        { 
                            DeptID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            DeptTitle = GlobalFunction.ReturnEmptyString(dr[1]),
                            DeptAbbrv = GlobalFunction.ReturnEmptyString(dr[2]),
                            DeptOfficeCode = GlobalFunction.ReturnEmptyString(dr[3]),
                            SectorTitle = GlobalFunction.ReturnEmptyString(dr[4]),
                            FundTitle = GlobalFunction.ReturnEmptyString(dr[5]),
                            SubSectorID = GlobalFunction.ReturnEmptyInt(dr[6]),
                        });
                    }
                }
                Connection.Close();
            }
            model.getDeptList = getDeptList.ToList();

            return PartialView("DepartmentTab/_TableDepartmentList", model.getDeptList);
        }
        //Get Add Department Partial View
        public ActionResult GetAddDepartment()
        {
            DepartmentModel model = new DepartmentModel();
            return PartialView("DepartmentTab/_AddDepartment", model);
        }
        ////Add Department
        public JsonResult AddNewDepartment(DepartmentModel model)
        {
            Tbl_FMDepartment DepartmentTable = new Tbl_FMDepartment();

            DepartmentTable.DeptTitle = GlobalFunction.ReturnEmptyString(model.getDeptColumns.DeptTitle);
            DepartmentTable.DeptAbbrv = GlobalFunction.ReturnEmptyString(model.getDeptColumns.DeptAbbrv);
            DepartmentTable.DeptOfficeCode = GlobalFunction.ReturnEmptyString(model.getDeptColumns.DeptOfficeCode);
            DepartmentTable.SectorID = GlobalFunction.ReturnEmptyInt(model.SectorID);
            var SubSectorTemp = GlobalFunction.ReturnEmptyInt(model.SubSectorID);
            if (SubSectorTemp == 0)
            {
                DepartmentTable.SubSectorID = null;
            }
            else
            {
                DepartmentTable.SubSectorID = model.SubSectorID;
            }

            DepartmentTable.FundID = GlobalFunction.ReturnEmptyInt(model.FundID);
            BOSSDB.Tbl_FMDepartment.Add(DepartmentTable);
            BOSSDB.SaveChanges();
            return Json(DepartmentTable);
        }
        public ActionResult GetAddDynamicSubSector(int SectorID)
        {
            DepartmentModel model = new DepartmentModel();

            model.SubSectorList = new SelectList((from s in BOSSDB.SubSectors.Where(a => a.SectorID == SectorID).ToList() select new { SubSectorID = s.SubSectorID, SubSectorTitle = s.SubSectorTitle }), "SubSectorID", "SubSectorTitle");
            
            return PartialView("DepartmentTab/_AddDynamicSubSector", model);
        }
        //Get Dynamic SubSector For Update Partial View
        public ActionResult GetUpdateDynamicSubSector(int SectorID)
        {
            DepartmentModel model = new DepartmentModel();

            model.SubSectorList = new SelectList((from s in BOSSDB.SubSectors.Where(a => a.SectorID == SectorID).ToList() select new { SubSectorID = s.SubSectorID, SubSectorTitle = s.SubSectorTitle }), "SubSectorID", "SubSectorTitle");

            return PartialView("DepartmentTab/_UpdateDynamicSubSector", model);
        }
        //Get Department Update Partial View
        public ActionResult Get_UpdateDepartment(DepartmentModel model, int DeptID)
        {
            Tbl_FMDepartment departmentTable = (from e in BOSSDB.Tbl_FMDepartment where e.DeptID == DeptID select e).FirstOrDefault();

            model.getDeptColumns2.DeptTitle = departmentTable.DeptTitle;
            model.getDeptColumns2.DeptAbbrv = departmentTable.DeptAbbrv;
            model.getDeptColumns2.DeptOfficeCode = departmentTable.DeptOfficeCode;
            model.SectorID = Convert.ToInt32(departmentTable.SectorID);
            model.FundID = Convert.ToInt32(departmentTable.FundID);
            model.SubSectorID = Convert.ToInt32(departmentTable.SubSectorID);
            model.DeptID = Convert.ToInt32(departmentTable.DeptID);

            return PartialView("DepartmentTab/_UpdateDepartment", model);
        }
        //Update Department
        public ActionResult UpdateDepartment(DepartmentModel model)
        {
            Tbl_FMDepartment departmentTBL = (from e in BOSSDB.Tbl_FMDepartment where e.DeptID == model.DeptID select e).FirstOrDefault();

            departmentTBL.DeptTitle = GlobalFunction.ReturnEmptyString(model.getDeptColumns2.DeptTitle);
            departmentTBL.DeptAbbrv = GlobalFunction.ReturnEmptyString(model.getDeptColumns2.DeptAbbrv);
            departmentTBL.DeptOfficeCode = GlobalFunction.ReturnEmptyString(model.getDeptColumns2.DeptOfficeCode);
            departmentTBL.SectorID = GlobalFunction.ReturnEmptyInt(model.SectorID);
            var SubSectorTemp = GlobalFunction.ReturnEmptyInt(model.SubSectorID);
            if (SubSectorTemp == 0)
            {
                departmentTBL.SubSectorID = null;
            }
            else
            {
                departmentTBL.SubSectorID = model.SubSectorID;
            }
            departmentTBL.FundID = GlobalFunction.ReturnEmptyInt(model.FundID);

            BOSSDB.Entry(departmentTBL);
            BOSSDB.SaveChanges();
            return PartialView("DepartmentTab/_AddDepartment", model);
        }
        public ActionResult DeleteDepartment(DepartmentModel model, int DeptID)
        {
            List<Tbl_FMOfficeSection> tblOfficeSec = (from e in BOSSDB.Tbl_FMOfficeSection where e.DeptID == DeptID select e).ToList();
            if (tblOfficeSec != null)
            {
                foreach (var items in tblOfficeSec)
                {
                    BOSSDB.Tbl_FMOfficeSection.Remove(items);
                    BOSSDB.SaveChanges();
                }
            }
            List<Tbl_FMFunction> tblfunction = (from e in BOSSDB.Tbl_FMFunction where e.DeptID == DeptID select e).ToList();
            if (tblfunction != null)
            {
                foreach (var items in tblfunction)
                {
                    BOSSDB.Tbl_FMFunction.Remove(items);
                    BOSSDB.SaveChanges();
                }
            }
            Tbl_FMDepartment deptTBL = (from e in BOSSDB.Tbl_FMDepartment where e.DeptID == DeptID select e).FirstOrDefault();
            BOSSDB.Tbl_FMDepartment.Remove(deptTBL);
            BOSSDB.SaveChanges();
            return RedirectToAction("FileResponsibility");
        }
        //===============================================================================================
        public ActionResult GetPartialIndexFunction()
        {
            FunctionModel model = new FunctionModel();
            return PartialView("FunctionTab/_PartialIndexFunction", model);
        }
        //View Table For Function Table
        public ActionResult GetFunctionDT(int deptID)
        {
            FunctionModel model = new FunctionModel();

            List<FunctionList> getFuncList = new List<FunctionList>();
            var SQLQuery = "";
           SQLQuery = "SELECT [FunctionID], [FunctionTitle], [FunctionAbbrv], [FunctionCode], [SectorTitle],[Tbl_FMDepartment].[SubSectorID],[FundTitle], [dbo].[Tbl_FMFunction].[DeptID]FROM [dbo].[Tbl_FMFunction], [dbo].[FundType],[dbo].[Tbl_FMDepartment] ,[dbo].[Sector] where[FundType].FundID = [Tbl_FMFunction].FundID and [Tbl_FMDepartment].[DeptID] = [Tbl_FMFunction].[DeptID] and [Sector].[SectorID] = [Tbl_FMDepartment].[SectorID] and [Tbl_FMFunction].[DeptID]=" + deptID;
            
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
                        getFuncList.Add(new FunctionList()
                        {
                            FunctionID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            FunctionTitle = GlobalFunction.ReturnEmptyString(dr[1]),
                            FunctionAbbrv = GlobalFunction.ReturnEmptyString(dr[2]),
                            FunctionCode = GlobalFunction.ReturnEmptyString(dr[3]),
                            SectorTitle = GlobalFunction.ReturnEmptyString(dr[4]),
                            SubSectorID = GlobalFunction.ReturnEmptyInt(dr[5]),
                            FundTitle = GlobalFunction.ReturnEmptyString(dr[6])
                        });
                    }
                }
                Connection.Close();
            }
            model.getFuncList = getFuncList.ToList();

            return PartialView("FunctionTab/_TableFunctionList", model.getFuncList);
        }
        //Get Add Function Partial View
        public ActionResult GetAddFunction(int deptID)
        {
            FunctionModel model = new FunctionModel();
            model.DeptID2 = deptID;
            return PartialView("FunctionTab/_AddFunction", model);
        }
        public ActionResult GetSectorSubsectorFields(int deptID)
        {
            FunctionModel model = new FunctionModel();
            var departmentTable = (from e in BOSSDB.Tbl_FMDepartment where e.DeptID == deptID select e).FirstOrDefault();
            model.SectorTitle = departmentTable.Sector.SectorTitle;

            var departmentSubsector = (from e in BOSSDB.SubSectors where e.SubSectorID == departmentTable.SubSectorID select e).FirstOrDefault();

            var subsect = "N/A";
            if (departmentSubsector != null)
            {
                subsect = departmentSubsector.SubSectorTitle;
            }
            model.SubSectorTitle = subsect;
            return PartialView("FunctionTab/_SectorSubsectorFields", model);
        }
        //Viewing of Department for Update function
        public ActionResult GetDepartmentforUpdate(int DeptID2)
        {
            FunctionModel model = new FunctionModel();
            var departmentTable = (from e in BOSSDB.Tbl_FMDepartment where e.DeptID == DeptID2 select e).FirstOrDefault();
            model.DeptTitle = departmentTable.DeptTitle;
            
            return PartialView("FunctionTab/_DepartmentforUpdate", model);
        }
        //Viewing of Sector and Subsector fields for Update function
        public ActionResult GetSectorSubsectorforUpdate(int DeptID2)
        {
            FunctionModel model = new FunctionModel();
            var departmentTable = (from e in BOSSDB.Tbl_FMDepartment where e.DeptID == DeptID2 select e).FirstOrDefault();
            model.SectorTitle = departmentTable.Sector.SectorTitle;

            var departmentSubsector = (from e in BOSSDB.SubSectors where e.SubSectorID == departmentTable.SubSectorID select e).FirstOrDefault();

            var subsect = "N/A";
            if (departmentSubsector != null)
            {
                subsect = departmentSubsector.SubSectorTitle;
            }
            model.SubSectorTitle = subsect;
            return PartialView("FunctionTab/_SectorSubsectorFields", model);
        }
        //Add Function
        public ActionResult AddNewFunction(FunctionModel model)
        {
            Tbl_FMFunction FunctionTable = new Tbl_FMFunction();

            FunctionTable.FunctionTitle = GlobalFunction.ReturnEmptyString(model.getFunctionColumns.FunctionTitle);
            FunctionTable.FunctionAbbrv = GlobalFunction.ReturnEmptyString(model.getFunctionColumns.FunctionAbbrv);
            FunctionTable.FunctionCode = GlobalFunction.ReturnEmptyString(model.getFunctionColumns.FunctionCode);
            FunctionTable.FundID = GlobalFunction.ReturnEmptyInt(model.FundID);
            FunctionTable.DeptID = GlobalFunction.ReturnEmptyInt(model.DeptID2);

            BOSSDB.Tbl_FMFunction.Add(FunctionTable);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Get Function Update Partial View
        public ActionResult Get_UpdateFunction(FunctionModel model, int FunctionID)
        {
            Tbl_FMFunction functionTable = (from e in BOSSDB.Tbl_FMFunction where e.FunctionID == FunctionID select e).FirstOrDefault();

            model.getFunctionColumns2.FunctionTitle = functionTable.FunctionTitle;
            model.getFunctionColumns2.FunctionAbbrv = functionTable.FunctionAbbrv;
            model.getFunctionColumns2.FunctionCode = functionTable.FunctionCode;
            model.FundID = Convert.ToInt32(functionTable.FundID);
            model.DeptID2 = Convert.ToInt32(functionTable.DeptID);
            model.FunctionID = FunctionID;
            return PartialView("FunctionTab/_UpdateFunction", model);
        }
        //Update Function
        public ActionResult UpdateFunction(FunctionModel model)
        {
            Tbl_FMFunction functionTBL = (from e in BOSSDB.Tbl_FMFunction where e.FunctionID == model.FunctionID select e).FirstOrDefault();

            functionTBL.FunctionTitle = GlobalFunction.ReturnEmptyString(model.getFunctionColumns2.FunctionTitle);
            functionTBL.FunctionAbbrv = GlobalFunction.ReturnEmptyString(model.getFunctionColumns2.FunctionAbbrv);
            functionTBL.FunctionCode = GlobalFunction.ReturnEmptyString(model.getFunctionColumns2.FunctionCode);
            functionTBL.FundID = GlobalFunction.ReturnEmptyInt(model.FundID);
            //functionTBL.DeptID = GlobalFunction.ReturnEmptyInt(model.DeptID2);

            BOSSDB.Entry(functionTBL);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPartialIndexFunction2(int DeptID2)
        {
            FunctionModel model = new FunctionModel();
            model.DeptID2 = DeptID2;
            return PartialView("FunctionTab/_PartialIndexFunction", model);
        }
        //Delete Function
        public ActionResult DeleteFunction(FunctionModel model, int FunctionID)
        {
            List<Tbl_FMOfficeSection> tblOfficeSec = (from e in BOSSDB.Tbl_FMOfficeSection where e.FunctionID == FunctionID select e).ToList();
            if (tblOfficeSec != null)
            {
                foreach (var items in tblOfficeSec)
                {
                    BOSSDB.Tbl_FMOfficeSection.Remove(items);
                    BOSSDB.SaveChanges();
                }
            }
            Tbl_FMFunction FuncTBL = (from e in BOSSDB.Tbl_FMFunction where e.FunctionID == FunctionID select e).FirstOrDefault();
            BOSSDB.Tbl_FMFunction.Remove(FuncTBL);
            BOSSDB.SaveChanges();
            return RedirectToAction("FileResponsibility");
        }
        //===============================================================================================
        public ActionResult GetPartialIndex()
        {
            OfficeSectionModel model = new OfficeSectionModel();
            return PartialView("OfficeSectionTab/_PartiaIndexOfficeSec", model);
        }
        //View Table For Office Section Table
        public ActionResult GetOfficeSecDT(int DeptID, int FuncID)
        {
            OfficeSectionModel model = new OfficeSectionModel();
            List<OfficeSectionList> getOfficeSecList = new List<OfficeSectionList>();
            var SQLQuery = "SELECT[OfficeSecID], [OfficeSecTitle], [Tbl_FMOfficeSection].[DeptID], [Tbl_FMOfficeSection].[FunctionID] FROM [BOSS].[dbo].[Tbl_FMOfficeSection], [BOSS].[dbo].[Tbl_FMFunction] where [dbo].[Tbl_FMOfficeSection].FunctionID = [dbo].[Tbl_FMFunction].FunctionID and [Tbl_FMOfficeSection].[DeptID] = " + DeptID + " and[Tbl_FMOfficeSection].FunctionID = " + FuncID;
            
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
                        getOfficeSecList.Add(new OfficeSectionList()
                        {
                            OfficeSecID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            OfficeSecTitle = GlobalFunction.ReturnEmptyString(dr[1])
                        });
                    }
                }
                Connection.Close();
            }
            model.getOfficeSecList = getOfficeSecList.ToList();

            return PartialView("OfficeSectionTab/_TableOfficeSectionList", model.getOfficeSecList);
        }
        public ActionResult GetAddFunctionList(int DeptID)
        {
            OfficeSectionModel model = new OfficeSectionModel();

            model.FunctionList = new SelectList((from s in BOSSDB.Tbl_FMFunction.Where(a => a.DeptID == DeptID).ToList() select new { FunctionID = s.FunctionID, FunctionTitle = s.FunctionTitle }), "FunctionID", "FunctionTitle");

            return PartialView("OfficeSectionTab/_AddDynamicFunction", model);
        }
        public ActionResult GetAddOfficeSec(int DeptID, int FuncID)
        {
            OfficeSectionModel model = new OfficeSectionModel();
            model.DeptID = DeptID;
            model.FuncID = FuncID;
            return PartialView("OfficeSectionTab/_AddOfficeSec", model);
        }
        //Add Section
        public ActionResult AddNewSection(OfficeSectionModel model)
        {
            Tbl_FMOfficeSection sectionTable = new Tbl_FMOfficeSection();

            sectionTable.OfficeSecTitle = GlobalFunction.ReturnEmptyString(model.getOfficeSecColumns.OfficeSecTitle);
            sectionTable.DeptID = GlobalFunction.ReturnEmptyInt(model.DeptID);
            sectionTable.FunctionID = GlobalFunction.ReturnEmptyInt(model.FuncID);
            BOSSDB.Tbl_FMOfficeSection.Add(sectionTable);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Get Function Update Partial View
        public ActionResult Get_UpdateOfficeSection(OfficeSectionModel model, int OfficeSecID)
        {
            Tbl_FMOfficeSection officeSecTable = (from e in BOSSDB.Tbl_FMOfficeSection where e.OfficeSecID == OfficeSecID select e).FirstOrDefault();
           
            model.getOfficeSecColumns2.OfficeSecTitle = officeSecTable.OfficeSecTitle;
            model.DeptID = Convert.ToInt32(officeSecTable.DeptID);
            model.FuncID = Convert.ToInt32(officeSecTable.FunctionID);

            return PartialView("OfficeSectionTab/_UpdateOfficeSec", model);
        }
        //Get Partial View Department Title for Update only
        public ActionResult GetDeptTitleDisplay(int DeptID)
        {
            OfficeSectionModel model = new OfficeSectionModel();
            var officeSectionTable = (from e in BOSSDB.Tbl_FMDepartment where e.DeptID == DeptID select e).FirstOrDefault();

            model.DeptTitle = officeSectionTable.DeptTitle;
            
            return PartialView("OfficeSectionTab/_UpdateDisplayDeptTitle", model);
        }
        //Get Partial View function Title for Update only
        public ActionResult GetfunctionTitleDisplay(int FuncID)
        {
            OfficeSectionModel model = new OfficeSectionModel();
            var officeSectionTable = (from e in BOSSDB.Tbl_FMFunction where e.FunctionID == FuncID select e).FirstOrDefault();

            model.FunctionTitle = officeSectionTable.FunctionTitle;

            return PartialView("OfficeSectionTab/_UpdateDisplayFunctionTitle", model);
        }
        //Update Office Section 
        public ActionResult UpdateOfficeSection(OfficeSectionModel model)
        {
            Tbl_FMOfficeSection officeSecTBL = (from e in BOSSDB.Tbl_FMOfficeSection where e.OfficeSecID == model.OfficeSecID select e).FirstOrDefault();

            officeSecTBL.OfficeSecTitle = GlobalFunction.ReturnEmptyString(model.getOfficeSecColumns2.OfficeSecTitle);
            officeSecTBL.DeptID = GlobalFunction.ReturnEmptyInt(model.DeptID);
            officeSecTBL.FunctionID = GlobalFunction.ReturnEmptyInt(model.FuncID);

            BOSSDB.Entry(officeSecTBL);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPartialIndex2(int DeptID)
        {
            OfficeSectionModel model = new OfficeSectionModel();
            model.DeptID = DeptID;

            return PartialView("OfficeSectionTab/_PartiaIndexOfficeSec", model);
        }
        public ActionResult GetAddFunctionList2(int DeptID ,int FuncID)
        {
            OfficeSectionModel model = new OfficeSectionModel();
            model.DeptID = DeptID;
           
            model.FunctionList = new SelectList((from s in BOSSDB.Tbl_FMFunction.ToList() select new { FuncID = s.FunctionID, FunctionTitle = s.FunctionTitle }), "FuncID", "FunctionTitle");
            model.FuncID = FuncID;

            return PartialView("OfficeSectionTab/_AddDynamicFunction", model);
        }
        //Delete Section
        public ActionResult DeleteOfficeSection(DepartmentModel model, int OfficeSecID)
        {
            Tbl_FMOfficeSection officeSecTBL = (from e in BOSSDB.Tbl_FMOfficeSection where e.OfficeSecID == OfficeSecID select e).FirstOrDefault();
            BOSSDB.Tbl_FMOfficeSection.Remove(officeSecTBL);
            BOSSDB.SaveChanges();
            return RedirectToAction("FileResponsibility");
        }





        //CALLING MODALS ======================================================================

        //SECTOR for Dept TAB
        public ActionResult GetAddSectorModalDept()
        {
            return PartialView("Modals/_AddSectorModal");
        }
        //SUB SECTOR for Dept TAB
        public ActionResult GetAddSubSectorModalDept()
        {
            return PartialView("Modals/_AddSubSectorModal");
        }
        //FUND for Dept TAB
        public ActionResult GetAddFundModalDept()
        {
            return PartialView("Modals/_AddFundModal");
        }
        //FUND for Function TAB
        public ActionResult GetAddFundModalFunc()
        {
            return PartialView("Modals/_AddFundModal");
        }
        //Department for Function Tab
        public ActionResult GetAddDeptModalFunc()
        {
            return PartialView("Modals/_AddDepartmentModal");
        }
        //Department for Section Tab
        public ActionResult GetAddDeptModalSec()
        {
            return PartialView("Modals/_AddDepartmentModal");
        }
        //Function for Section Tab
        public ActionResult GetAddFuncModalSec()
        {
            return PartialView("Modals/_AddFunctionModal");
        }
        public ActionResult GetAddOfficeTypeModal()
        {
            return PartialView("Modals/_AddOfficeTypeModal");
        }
        
    }
}

