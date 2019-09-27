using BOSS.GlobalFunctions;
using BOSS.Models;
using BOSS.Models.FMmodels.FMResCenterModels;
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
            return PartialView("DepartmentTab/_DepartmentTab", model);
        }

        public ActionResult FunctionTab()
        {
            FunctionModel model = new FunctionModel();
            return PartialView("FunctionTab/_FunctionTab", model);
        }
        public ActionResult SectionTab()
        {
            SectionModel model = new SectionModel();
            return PartialView("SectionTab/_SectionTab", model);
        }

        //===============================================================================================
        //View Table For Department Table
        public ActionResult GetDepartmentDT()
        {
            DepartmentModel model = new DepartmentModel();

            List<DepartmentList> getDeptList = new List<DepartmentList>();

            //var SQLQuery = "SELECT [DeptID], [DeptTitle], [DeptAbbrv], [DeptOfficeCode], [SectorTitle], [FundTitle], [SubSectorID] FROM [Tbl_FMDepartment], [FundType], [Sector] where[FundType].FundID = [Tbl_FMDepartment].FundID and[Sector].SectorID = [Tbl_FMDepartment].SectorID";
            var SQLQuery = "SELECT [DeptID], [DeptTitle], [DeptAbbrv], [RCcode], [FundTitle], [SectorTitle], [SubSectorID], [OfficeTypeTitle], [DeptOfficeCode] FROM [Tbl_FMDepartment], [Fund], [Sector] ,[OfficeType] where [Fund].FundID = [Tbl_FMDepartment].FundID and [Sector].SectorID = [Tbl_FMDepartment].SectorID and [OfficeType].OfficeTypeID = [Tbl_FMDepartment].OfficeTypeID";
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
                            RCcode = GlobalFunction.ReturnEmptyString(dr[3]),
                            FundTitle = GlobalFunction.ReturnEmptyString(dr[4]),
                            SectorTitle = GlobalFunction.ReturnEmptyString(dr[5]),
                            SubSectorID = GlobalFunction.ReturnEmptyInt(dr[6]),
                            OfficeTypeTitle = GlobalFunction.ReturnEmptyString(dr[7]),
                            DeptOfficeCode = GlobalFunction.ReturnEmptyString(dr[8])
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
            DepartmentTable.RCcode = GlobalFunction.ReturnEmptyString(model.getDeptColumns.RCcode);
            DepartmentTable.FundID = GlobalFunction.ReturnEmptyInt(model.FundID);
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
            DepartmentTable.OfficeTypeID = GlobalFunction.ReturnEmptyInt(model.OfficeTypeID);
            DepartmentTable.DeptOfficeCode = GlobalFunction.ReturnEmptyString(model.getDeptColumns.DeptOfficeCode);

            BOSSDB.Tbl_FMDepartment.Add(DepartmentTable);
            BOSSDB.SaveChanges();
            return Json(DepartmentTable);
        }
        public ActionResult GetAddDynamicSubSector(int SectorID)
        {
            DepartmentModel model = new DepartmentModel();

            model.SubSectorList = new SelectList((from s in BOSSDB.SubSectors.Where(a => a.SectorID == SectorID).ToList() select new { SubSectorID = s.SubSectorID, SubSectorTitle = s.SubSectorTitle }), "SubSectorID", "SubSectorTitle");
            
            return PartialView("DepartmentTab/_DynamicSubSector", model);
        }
        //Get Department Update Partial View
        public ActionResult Get_UpdateDepartment(DepartmentModel model, int DeptID)
        {
            Tbl_FMDepartment departmentTable = (from e in BOSSDB.Tbl_FMDepartment where e.DeptID == DeptID select e).FirstOrDefault();

            model.getDeptColumns.DeptTitle = departmentTable.DeptTitle;
            model.getDeptColumns.DeptAbbrv = departmentTable.DeptAbbrv;
            model.getDeptColumns.RCcode = departmentTable.RCcode;
            model.FundID = Convert.ToInt32(departmentTable.FundID);
            model.SectorID = Convert.ToInt32(departmentTable.SectorID);
            model.SubSectorID = Convert.ToInt32(departmentTable.SubSectorID);
            model.getDeptColumns.DeptOfficeCode = departmentTable.DeptOfficeCode;
            model.OfficeTypeID = Convert.ToInt32(departmentTable.OfficeTypeID);
            model.DeptID = DeptID;

            return PartialView("DepartmentTab/_UpdateDepartment", model);
        }
        //Update Department
        public ActionResult UpdateDepartment(DepartmentModel model)
        {
            Tbl_FMDepartment departmentTBL = (from e in BOSSDB.Tbl_FMDepartment where e.DeptID == model.DeptID select e).FirstOrDefault();

            departmentTBL.DeptTitle = GlobalFunction.ReturnEmptyString(model.getDeptColumns.DeptTitle);
            departmentTBL.DeptAbbrv = GlobalFunction.ReturnEmptyString(model.getDeptColumns.DeptAbbrv);
            departmentTBL.RCcode = GlobalFunction.ReturnEmptyString(model.getDeptColumns.RCcode);
            departmentTBL.FundID = GlobalFunction.ReturnEmptyInt(model.FundID);
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
            departmentTBL.OfficeTypeID = GlobalFunction.ReturnEmptyInt(model.OfficeTypeID);
            departmentTBL.DeptOfficeCode = GlobalFunction.ReturnEmptyString(model.getDeptColumns.DeptOfficeCode);
            BOSSDB.Entry(departmentTBL);
            BOSSDB.SaveChanges();
            return PartialView("DepartmentTab/_AddDepartment", model);
        }
        public ActionResult DeleteDepartment(DepartmentModel model, int DeptID)
        {
            List<Tbl_FMSection> tblOfficeSec = (from e in BOSSDB.Tbl_FMSection where e.DeptID == DeptID select e).ToList();
            List<Tbl_FMFunction> tblfunction = (from e in BOSSDB.Tbl_FMFunction where e.DeptID == DeptID select e).ToList();
            if (tblOfficeSec != null)
            {
                foreach (var items in tblOfficeSec)
                {
                    BOSSDB.Tbl_FMSection.Remove(items);
                    BOSSDB.SaveChanges();
                }
            } else if (tblfunction != null)
            {
                foreach (var items in tblfunction)
                {
                    BOSSDB.Tbl_FMFunction.Remove(items);
                    BOSSDB.SaveChanges();
                }
            }
            //List<Tbl_FMFunction> tblfunction = (from e in BOSSDB.Tbl_FMFunction where e.DeptID == DeptID select e).ToList();
            //if (tblfunction != null)
            //{
            //    foreach (var items in tblfunction)
            //    {
            //        BOSSDB.Tbl_FMFunction.Remove(items);
            //        BOSSDB.SaveChanges();
            //    }
            //}
            Tbl_FMDepartment deptTBL = (from e in BOSSDB.Tbl_FMDepartment where e.DeptID == DeptID select e).FirstOrDefault();
            BOSSDB.Tbl_FMDepartment.Remove(deptTBL);
            BOSSDB.SaveChanges();
            return RedirectToAction("FileResponsibility");
        }
        //===============================================================================================
        //View Table For Function Table
        public ActionResult GetFunctionDT()
        {
            FunctionModel model = new FunctionModel();

            List<FunctionList> getFuncList = new List<FunctionList>();
            var SQLQuery = "";
            //SQLQuery = "SELECT [FunctionID], [FunctionTitle], [FunctionAbbrv], [FunctionCode], [SectorTitle],[Tbl_FMDepartment].[SubSectorID],[FundTitle], [dbo].[Tbl_FMFunction].[DeptID]FROM [dbo].[Tbl_FMFunction], [dbo].[FundType],[dbo].[Tbl_FMDepartment] ,[dbo].[Sector] where[FundType].FundID = [Tbl_FMFunction].FundID and [Tbl_FMDepartment].[DeptID] = [Tbl_FMFunction].[DeptID] and [Sector].[SectorID] = [Tbl_FMDepartment].[SectorID] and [Tbl_FMFunction].[DeptID]=" + deptID;
            SQLQuery = "SELECT [FunctionID], [DeptTitle], [FunctionTitle], [FunctionAbbrv], [FunctionCode], [FundTitle], [SectorTitle], [Tbl_FMFunction].[SubSectorID], [OfficeTypeTitle], [DeptOfficeCodefunc] FROM [BOSS].[dbo].[Tbl_FMFunction], [Sector], [Tbl_FMDepartment], [OfficeType], [Fund]where[Sector].SectorID = [Tbl_FMFunction].SectorID and [Tbl_FMDepartment].DeptID = [Tbl_FMFunction].DeptID and [OfficeType].OfficeTypeID = [Tbl_FMFunction].OfficeTypeID and [Fund].FundID = [Tbl_FMDepartment].FundID";
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
                            DeptTitle = GlobalFunction.ReturnEmptyString(dr[1]),
                            FunctionTitle = GlobalFunction.ReturnEmptyString(dr[2]),
                            FunctionAbbrv = GlobalFunction.ReturnEmptyString(dr[3]),
                            FunctionCode = GlobalFunction.ReturnEmptyString(dr[4]),
                            FundTitle = GlobalFunction.ReturnEmptyString(dr[5]),
                            SectorTitle = GlobalFunction.ReturnEmptyString(dr[6]),
                            SubSectorID = GlobalFunction.ReturnEmptyInt(dr[7]),
                            OfficeTypeTitle =GlobalFunction.ReturnEmptyString(dr[8]),
                            DeptOfficeCodefunc = GlobalFunction.ReturnEmptyString(dr[9])
                        });
                    }
                }
                Connection.Close();
            }
            model.getFuncList = getFuncList.ToList();

            return PartialView("FunctionTab/_TableFunctionList", model.getFuncList);
        }
        //Get Add Function Partial View
        public ActionResult GetAddFunction()
        {
            FunctionModel model = new FunctionModel();
            return PartialView("FunctionTab/_AddFunction", model);
        }
        //Viewing of Fund Title
        public ActionResult GetFund(int DeptID)
        {
            FunctionModel model = new FunctionModel();
            var funcTable = (from e in BOSSDB.Tbl_FMDepartment where e.DeptID == DeptID select e).FirstOrDefault();
            //model.FundTitle = funcTable.Fund.FundTitle;

            var departmentFund = (from e in BOSSDB.Funds where e.FundID == funcTable.FundID select e).FirstOrDefault();

            var fundtitle = "N/A";
            if (departmentFund != null)
            {
                fundtitle = departmentFund.FundTitle;
            }
            model.FundTitle = fundtitle;
            return PartialView("FunctionTab/_DynamicFundTitle", model);
        }
        //Add Function
        public ActionResult AddNewFunction(FunctionModel model)
        {
            Tbl_FMFunction FunctionTable = new Tbl_FMFunction();

            FunctionTable.FunctionTitle = GlobalFunction.ReturnEmptyString(model.getFunctionColumns.FunctionTitle);
            FunctionTable.FunctionAbbrv = GlobalFunction.ReturnEmptyString(model.getFunctionColumns.FunctionAbbrv);
            FunctionTable.FunctionCode = GlobalFunction.ReturnEmptyString(model.getFunctionColumns.FunctionCode);
            FunctionTable.SectorID = GlobalFunction.ReturnEmptyInt(model.SectorID);
            var SubSectorTemp = GlobalFunction.ReturnEmptyInt(model.SubSectorID);
            if (SubSectorTemp == 0)
            {
                FunctionTable.SubSectorID = null;
            }
            else
            {
                FunctionTable.SubSectorID = model.SubSectorID;
            }
            FunctionTable.OfficeTypeID = GlobalFunction.ReturnEmptyInt(model.OfficeTypeID);
            FunctionTable.DeptOfficeCodefunc = GlobalFunction.ReturnEmptyString(model.getFunctionColumns.DeptOfficeCodefunc);
            FunctionTable.DeptID = GlobalFunction.ReturnEmptyInt(model.DeptID);
            BOSSDB.Tbl_FMFunction.Add(FunctionTable);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDynamicSubSectorfunc(int SectorID)
        {
            FunctionModel model = new FunctionModel();

            model.SubSectorListfunc = new SelectList((from s in BOSSDB.SubSectors.Where(a => a.SectorID == SectorID).ToList() select new { SubSectorID = s.SubSectorID, SubSectorTitle = s.SubSectorTitle }), "SubSectorID", "SubSectorTitle");

            return PartialView("FunctionTab/_DynamicSubSectorfunc", model);
        }
        //Get Function Update Partial View
        public ActionResult Get_UpdateFunction(FunctionModel model, int FunctionID)
        {
            Tbl_FMFunction functionTable = (from e in BOSSDB.Tbl_FMFunction where e.FunctionID == FunctionID select e).FirstOrDefault();

            model.getFunctionColumns.FunctionTitle = functionTable.FunctionTitle;
            model.getFunctionColumns.FunctionAbbrv = functionTable.FunctionAbbrv;
            model.getFunctionColumns.FunctionCode = functionTable.FunctionCode;
            model.SectorID = Convert.ToInt32(functionTable.SectorID);
            model.SubSectorID = Convert.ToInt32(functionTable.SubSectorID);
            model.OfficeTypeID = Convert.ToInt32(functionTable.OfficeTypeID);
            model.getFunctionColumns.DeptOfficeCodefunc = functionTable.DeptOfficeCodefunc;
            model.DeptID = Convert.ToInt32(functionTable.DeptID);
            model.FunctionID = FunctionID;
            return PartialView("FunctionTab/_UpdateFunction", model);
        }
        //Get Dynamic SubSector For Update Partial View
        public ActionResult GetSectorSubsectorforUpdate(int SectorID)
        {
            DepartmentModel model = new DepartmentModel();

            model.SubSectorList = new SelectList((from s in BOSSDB.SubSectors.Where(a => a.SectorID == SectorID).ToList() select new { SubSectorID = s.SubSectorID, SubSectorTitle = s.SubSectorTitle }), "SubSectorID", "SubSectorTitle");

            return PartialView("DynamicFields/_UpdateDynamicSubSector", model);
        }
        //Update Function
        public ActionResult UpdateFunction(FunctionModel model)
        {
            Tbl_FMFunction functionTBL = (from e in BOSSDB.Tbl_FMFunction where e.FunctionID == model.FunctionID select e).FirstOrDefault();

            functionTBL.FunctionTitle = GlobalFunction.ReturnEmptyString(model.getFunctionColumns.FunctionTitle);
            functionTBL.FunctionAbbrv = GlobalFunction.ReturnEmptyString(model.getFunctionColumns.FunctionAbbrv);
            functionTBL.FunctionCode = GlobalFunction.ReturnEmptyString(model.getFunctionColumns.FunctionCode);
            functionTBL.SectorID = GlobalFunction.ReturnEmptyInt(model.SectorID);
            var SubSectorTemp = GlobalFunction.ReturnEmptyInt(model.SubSectorID);
            if (SubSectorTemp == 0)
            {
                functionTBL.SubSectorID = null;
            }
            else
            {
                functionTBL.SubSectorID = model.SubSectorID;
            }
            functionTBL.OfficeTypeID = GlobalFunction.ReturnEmptyInt(model.OfficeTypeID);
            functionTBL.DeptOfficeCodefunc = GlobalFunction.ReturnEmptyString(model.getFunctionColumns.DeptOfficeCodefunc);
            functionTBL.DeptID = GlobalFunction.ReturnEmptyInt(model.DeptID);
            BOSSDB.Entry(functionTBL);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Delete Function
        public ActionResult DeleteFunction(FunctionModel model, int FunctionID)
        {
            List<Tbl_FMSection> tblOfficeSec = (from e in BOSSDB.Tbl_FMSection where e.FunctionID == FunctionID select e).ToList();
            if (tblOfficeSec != null)
            {
                foreach (var items in tblOfficeSec)
                {
                    BOSSDB.Tbl_FMSection.Remove(items);
                    BOSSDB.SaveChanges();
                }
            }
            Tbl_FMFunction FuncTBL = (from e in BOSSDB.Tbl_FMFunction where e.FunctionID == FunctionID select e).FirstOrDefault();
            BOSSDB.Tbl_FMFunction.Remove(FuncTBL);
            BOSSDB.SaveChanges();
            return RedirectToAction("FileResponsibility");
        }
        //===============================================================================================
        //View Table For Office Section Table
        public ActionResult GetSectionDT()
        {
            SectionModel model = new SectionModel();
            List<SectionList> getOfficeSecList = new List<SectionList>();
            var SQLQuery = "SELECT [SectionID], [SectionTitle], [DeptTitle], [FunctionTitle] FROM [BOSS].[dbo].[Tbl_FMSection],Tbl_FMDepartment,Tbl_FMFunction where Tbl_FMDepartment.DeptID = [Tbl_FMSection].DeptID and Tbl_FMFunction.FunctionID = [Tbl_FMSection].FunctionID";
            
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
                        getOfficeSecList.Add(new SectionList()
                        {
                            SectionID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            SectionTitle = GlobalFunction.ReturnEmptyString(dr[1]),
                            DeptTitle = GlobalFunction.ReturnEmptyString(dr[2]),
                            FunctionTitle = GlobalFunction.ReturnEmptyString(dr[3])
                        });
                    }
                }
                Connection.Close();
            }
            model.getSectionList = getOfficeSecList.ToList();

            return PartialView("SectionTab/_TableSectionList", model.getSectionList);
        }
        public ActionResult GetDynamicFunction(int DeptID)
        {
            SectionModel model = new SectionModel();

            model.FunctionList = new SelectList((from s in BOSSDB.Tbl_FMFunction.Where(a => a.DeptID == DeptID).ToList() select new { FunctionID = s.FunctionID, FunctionTitle = s.FunctionTitle }), "FunctionID", "FunctionTitle");

            return PartialView("SectionTab/_DynamicFunction", model);
        }
        public ActionResult GetAddSection()
        {
            SectionModel model = new SectionModel();
            return PartialView("SectionTab/_AddSection", model);
        }
        //Add Section
        public ActionResult AddNewSection(SectionModel model)
        {
            Tbl_FMSection sectionTable = new Tbl_FMSection();

            sectionTable.SectionTitle = GlobalFunction.ReturnEmptyString(model.getSectionColumns.SectionTitle);
            sectionTable.DeptID = GlobalFunction.ReturnEmptyInt(model.DeptID);
            sectionTable.FunctionID = GlobalFunction.ReturnEmptyInt(model.FunctionID);
            BOSSDB.Tbl_FMSection.Add(sectionTable);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Get Function Update Partial View
        public ActionResult Get_UpdateSection(SectionModel model, int SectionID)
        {
            Tbl_FMSection officeSecTable = (from e in BOSSDB.Tbl_FMSection where e.SectionID == SectionID select e).FirstOrDefault();
           
            model.getSectionColumns.SectionTitle = officeSecTable.SectionTitle;
            model.DeptID = Convert.ToInt32(officeSecTable.DeptID);
            model.FunctionID = Convert.ToInt32(officeSecTable.FunctionID);

            return PartialView("SectionTab/_UpdateSection", model);
        }
        //Update Office Section 
        public ActionResult UpdateSection(SectionModel model)
        {
            Tbl_FMSection officeSecTBL = (from e in BOSSDB.Tbl_FMSection where e.SectionID == model.SectionID select e).FirstOrDefault();

            officeSecTBL.SectionTitle = GlobalFunction.ReturnEmptyString(model.getSectionColumns.SectionTitle);
            officeSecTBL.DeptID = GlobalFunction.ReturnEmptyInt(model.DeptID);
            officeSecTBL.FunctionID = GlobalFunction.ReturnEmptyInt(model.FunctionID);

            BOSSDB.Entry(officeSecTBL);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Delete Section
        public ActionResult DeleteSection(DepartmentModel model, int SectionID)
        {
            Tbl_FMSection officeSecTBL = (from e in BOSSDB.Tbl_FMSection where e.SectionID == SectionID select e).FirstOrDefault();
            BOSSDB.Tbl_FMSection.Remove(officeSecTBL);
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

