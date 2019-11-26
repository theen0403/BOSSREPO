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
        //Department Tab
        //===============================================================================================
        //View Table For Department Table
        public ActionResult GetDepartmentDT()
        {
            DepartmentModel model = new DepartmentModel();

            List<DepartmentList> getDeptList = new List<DepartmentList>();

            //var SQLQuery = "SELECT [DeptID], [DeptTitle], [DeptAbbrv], [DeptOfficeCode], [SectorTitle], [FundTitle], [SubSectorID] FROM [Tbl_FMRes_Department], [FundType], [Sector] where[FundType].FundID = [Tbl_FMRes_Department].FundID and[Sector].SectorID = [Tbl_FMRes_Department].SectorID";
            var SQLQuery = "SELECT [DeptID], [DeptTitle], [DeptAbbrv], [RCcode], [FundTitle], [SectorTitle], [SubSectorID], [OfficeTypeTitle], [DeptOfficeCode] FROM [Tbl_FMRes_Department], [Tbl_FMFund_Fund], [Tbl_FMSector_Sector] ,[Tbl_FMOfficeType] where [Tbl_FMFund_Fund].FundID = [Tbl_FMRes_Department].FundID and [Tbl_FMSector_Sector].SectorID = [Tbl_FMRes_Department].SectorID and [Tbl_FMOfficeType].OfficeTypeID = [Tbl_FMRes_Department].OfficeTypeID";
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
            Tbl_FMRes_Department DepartmentTable = new Tbl_FMRes_Department();

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

            BOSSDB.Tbl_FMRes_Department.Add(DepartmentTable);
            BOSSDB.SaveChanges();
            return Json(DepartmentTable);
        }
        public ActionResult GetDynamicSubSector(int SectorID)
        {
            DepartmentModel model = new DepartmentModel();

            model.SubSectorList = new SelectList((from s in BOSSDB.Tbl_FMSector_SubSector.Where(a => a.SectorID == SectorID).ToList() select new { SubSectorID = s.SubSectorID, SubSectorTitle = s.SubSectorTitle }), "SubSectorID", "SubSectorTitle");
            return PartialView("DepartmentTab/_DynamicSubSector", model);
        }
        public ActionResult GetDynamicSubSector2(DepartmentModel model, int SectorID, int subsectorIDHidden)
        {
            model.SubSectorList = new SelectList((from s in BOSSDB.Tbl_FMSector_SubSector.Where(a => a.SectorID == SectorID).ToList() select new { SubSectorID = s.SubSectorID, SubSectorTitle = s.SubSectorTitle }), "SubSectorID", "SubSectorTitle");
            model.SubSectorID = subsectorIDHidden;
            return PartialView("DepartmentTab/_DynamicSubSector", model);
        }
        //Get Department Update Partial View
        public ActionResult Get_UpdateDepartment(DepartmentModel model, int DeptID)
        {
            Tbl_FMRes_Department departmentTable = (from e in BOSSDB.Tbl_FMRes_Department where e.DeptID == DeptID select e).FirstOrDefault();

            model.getDeptColumns.DeptTitle = departmentTable.DeptTitle;
            model.getDeptColumns.DeptAbbrv = departmentTable.DeptAbbrv;
            model.getDeptColumns.RCcode = departmentTable.RCcode;
            model.FundID = Convert.ToInt32(departmentTable.FundID);
            model.SectorID = Convert.ToInt32(departmentTable.SectorID);
            model.subsectorIDHidden = Convert.ToInt32(departmentTable.SubSectorID);
            model.getDeptColumns.DeptOfficeCode = departmentTable.DeptOfficeCode;
            model.OfficeTypeID = Convert.ToInt32(departmentTable.OfficeTypeID);
            model.DeptID = DeptID;

            return PartialView("DepartmentTab/_UpdateDepartment", model);
        }
        //Update Department
        public ActionResult UpdateDepartment(DepartmentModel model)
        {
            Tbl_FMRes_Department departmentTBL = (from e in BOSSDB.Tbl_FMRes_Department where e.DeptID == model.DeptID select e).FirstOrDefault();

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
            List<Tbl_FMRes_Section> tblOfficeSec = (from e in BOSSDB.Tbl_FMRes_Section where e.DeptID == DeptID select e).ToList();
            List<Tbl_FMRes_Function> tblfunction = (from e in BOSSDB.Tbl_FMRes_Function where e.DeptID == DeptID select e).ToList();
            if (tblOfficeSec != null)
            {
                foreach (var items in tblOfficeSec)
                {
                    BOSSDB.Tbl_FMRes_Section.Remove(items);
                    BOSSDB.SaveChanges();
                }
            } else if (tblfunction != null)
            {
                foreach (var items in tblfunction)
                {
                    BOSSDB.Tbl_FMRes_Function.Remove(items);
                    BOSSDB.SaveChanges();
                }
            }
            //List<Tbl_FMRes_Function> tblfunction = (from e in BOSSDB.Tbl_FMRes_Function where e.DeptID == DeptID select e).ToList();
            //if (tblfunction != null)
            //{
            //    foreach (var items in tblfunction)
            //    {
            //        BOSSDB.Tbl_FMRes_Function.Remove(items);
            //        BOSSDB.SaveChanges();
            //    }
            //}
            Tbl_FMRes_Department deptTBL = (from e in BOSSDB.Tbl_FMRes_Department where e.DeptID == DeptID select e).FirstOrDefault();
            BOSSDB.Tbl_FMRes_Department.Remove(deptTBL);
            BOSSDB.SaveChanges();
            return RedirectToAction("FileResponsibility");
        }
        //===============================================================================================
        //Function Tab
        //===============================================================================================
        //View Table For Function Table
        public ActionResult GetFunctionDT()
        {
            FunctionModel model = new FunctionModel();

            List<FunctionList> getFuncList = new List<FunctionList>();
            var SQLQuery = "";
            //SQLQuery = "SELECT [FunctionID], [FunctionTitle], [FunctionAbbrv], [FunctionCode], [SectorTitle],[Tbl_FMRes_Department].[SubSectorID],[FundTitle], [dbo].[Tbl_FMRes_Function].[DeptID]FROM [dbo].[Tbl_FMRes_Function], [dbo].[FundType],[dbo].[Tbl_FMRes_Department] ,[dbo].[Sector] where[FundType].FundID = [Tbl_FMRes_Function].FundID and [Tbl_FMRes_Department].[DeptID] = [Tbl_FMRes_Function].[DeptID] and [Sector].[SectorID] = [Tbl_FMRes_Department].[SectorID] and [Tbl_FMRes_Function].[DeptID]=" + deptID;
            SQLQuery = "SELECT [FunctionID], [DeptTitle], [FunctionTitle], [FunctionAbbrv], [FunctionCode], [FundTitle], [SectorTitle], [Tbl_FMRes_Function].[SubSectorID], [OfficeTypeTitle], [DeptOfficeCodefunc] FROM [BOSS].[dbo].[Tbl_FMRes_Function], [Tbl_FMSector_Sector], [Tbl_FMRes_Department], [Tbl_FMOfficeType], [Tbl_FMFund_Fund] where [Tbl_FMSector_Sector].SectorID = [Tbl_FMRes_Function].SectorID and [Tbl_FMRes_Department].DeptID = [Tbl_FMRes_Function].DeptID and [Tbl_FMOfficeType].OfficeTypeID = [Tbl_FMRes_Function].OfficeTypeID and [Tbl_FMFund_Fund].FundID = [Tbl_FMRes_Department].FundID";
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
            var funcTable = (from e in BOSSDB.Tbl_FMRes_Department where e.DeptID == DeptID select e).FirstOrDefault();
            //model.FundTitle = funcTable.Fund.FundTitle;

            var departmentFund = (from e in BOSSDB.Tbl_FMFund_Fund where e.FundID == funcTable.FundID select e).FirstOrDefault();

            var fundtitle = "N/A";
            if (departmentFund != null)
            {
                fundtitle = departmentFund.FundTitle;
            }
            model.FundTitle = fundtitle;
            return PartialView("FunctionTab/_DynamicFundTitle", model);
        }
        //Viewing of Department / Office Code
        public ActionResult GetDeptOfficeCode(int DeptID)
        {
            FunctionModel model = new FunctionModel();
            var deptTable = (from e in BOSSDB.Tbl_FMRes_Department where e.DeptID == DeptID select e).FirstOrDefault();
            model.DeptOfficeCodefunc = deptTable.DeptOfficeCode;
            
            return PartialView("FunctionTab/_DynamicDeptOfficeCode", model);
        }
        ////Viewing of Sector Code
        //public ActionResult GetSectorfromDept(int DeptID)
        //{
        //    FunctionModel model = new FunctionModel();
        //    var deptTable = (from e in BOSSDB.Tbl_FMRes_Department where e.DeptID == DeptID select e).FirstOrDefault();
           
        //    model.SectorID = Convert.ToInt32(deptTable.SectorID);

        //    return PartialView("FunctionTab/_DynamicSectorField", model);
        //}
        ////Viewing of SubSector Code
        //public ActionResult GetSubSectorfromDept(int DeptID)
        //{
        //    FunctionModel model = new FunctionModel();
        //    var deptTable = (from e in BOSSDB.Tbl_FMRes_Department where e.DeptID == DeptID select e).FirstOrDefault();

        //    model.SubSectorID = deptTable.SubSector.SubSectorID;

        //    return PartialView("FunctionTab/_DynamicSubSectorfunc", model);
        //}
        //Add Function
        public ActionResult AddNewFunction(FunctionModel model)
        {
            Tbl_FMRes_Function FunctionTable = new Tbl_FMRes_Function();

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
            FunctionTable.DeptOfficeCodefunc = GlobalFunction.ReturnEmptyString(model.DeptOfficeCodefunc);
            FunctionTable.DeptID = GlobalFunction.ReturnEmptyInt(model.DeptID);
            BOSSDB.Tbl_FMRes_Function.Add(FunctionTable);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDynamicSubSectorfunc(int SectorID)
        {
            FunctionModel model = new FunctionModel();

            model.SubSectorListfunc = new SelectList((from s in BOSSDB.Tbl_FMSector_SubSector.Where(a => a.SectorID == SectorID).ToList() select new { SubSectorID = s.SubSectorID, SubSectorTitle = s.SubSectorTitle }), "SubSectorID", "SubSectorTitle");

            return PartialView("FunctionTab/_DynamicSubSectorfunc", model);
        }
        public ActionResult GetDynamicSubSectorfunc2(FunctionModel model, int SectorID, int subsectorIDHiddenfunc)
        {
            model.SubSectorListfunc = new SelectList((from s in BOSSDB.Tbl_FMSector_SubSector.Where(a => a.SectorID == SectorID).ToList() select new { SubSectorID = s.SubSectorID, SubSectorTitle = s.SubSectorTitle }), "SubSectorID", "SubSectorTitle");
            model.SubSectorID = subsectorIDHiddenfunc;
            return PartialView("FunctionTab/_DynamicSubSector", model);
        }
        //Get Function Update Partial View
        public ActionResult Get_UpdateFunction(FunctionModel model, int FunctionID)
        {
            Tbl_FMRes_Function functionTable = (from e in BOSSDB.Tbl_FMRes_Function where e.FunctionID == FunctionID select e).FirstOrDefault();

            model.getFunctionColumns.FunctionTitle = functionTable.FunctionTitle;
            model.getFunctionColumns.FunctionAbbrv = functionTable.FunctionAbbrv;
            model.getFunctionColumns.FunctionCode = functionTable.FunctionCode;
            model.SectorID = Convert.ToInt32(functionTable.SectorID);
            model.subsectorIDHiddenfunc = Convert.ToInt32(functionTable.SubSectorID);
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

            model.SubSectorList = new SelectList((from s in BOSSDB.Tbl_FMSector_SubSector.Where(a => a.SectorID == SectorID).ToList() select new { SubSectorID = s.SubSectorID, SubSectorTitle = s.SubSectorTitle }), "SubSectorID", "SubSectorTitle");

            return PartialView("DynamicFields/_UpdateDynamicSubSector", model);
        }
        //Update Function
        public ActionResult UpdateFunction(FunctionModel model)
        {
            Tbl_FMRes_Function functionTBL = (from e in BOSSDB.Tbl_FMRes_Function where e.FunctionID == model.FunctionID select e).FirstOrDefault();

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
            List<Tbl_FMRes_Section> tblOfficeSec = (from e in BOSSDB.Tbl_FMRes_Section where e.FunctionID == FunctionID select e).ToList();
            if (tblOfficeSec != null)
            {
                foreach (var items in tblOfficeSec)
                {
                    BOSSDB.Tbl_FMRes_Section.Remove(items);
                    BOSSDB.SaveChanges();
                }
            }
            Tbl_FMRes_Function FuncTBL = (from e in BOSSDB.Tbl_FMRes_Function where e.FunctionID == FunctionID select e).FirstOrDefault();
            BOSSDB.Tbl_FMRes_Function.Remove(FuncTBL);
            BOSSDB.SaveChanges();
            return RedirectToAction("FileResponsibility");
        }
        //===============================================================================================
        //Section Tab
        //===============================================================================================
        //View Table For Office Section Table
        public ActionResult GetSectionDT()
        {
            SectionModel model = new SectionModel();
            List<SectionList> getOfficeSecList = new List<SectionList>();
            var SQLQuery = "SELECT [SectionID], [SectionTitle], [DeptTitle], [FunctionTitle] FROM [BOSS].[dbo].[Tbl_FMRes_Section],Tbl_FMRes_Department,Tbl_FMRes_Function where Tbl_FMRes_Department.DeptID = [Tbl_FMRes_Section].DeptID and Tbl_FMRes_Function.FunctionID = [Tbl_FMRes_Section].FunctionID";
            
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

            model.FunctionList = new SelectList((from s in BOSSDB.Tbl_FMRes_Function.Where(a => a.DeptID == DeptID).ToList() select new { FunctionID = s.FunctionID, FunctionTitle = s.FunctionTitle }), "FunctionID", "FunctionTitle");

            return PartialView("SectionTab/_DynamicFunction", model);
        }
        public ActionResult GetDynamicFunction2(SectionModel model, int DeptID, int FunctionIDHidden)
        {
            model.FunctionList = new SelectList((from s in BOSSDB.Tbl_FMRes_Function.Where(a => a.DeptID == DeptID).ToList() select new { FunctionID = s.FunctionID, FunctionTitle = s.FunctionTitle }), "FunctionID", "FunctionTitle");
            model.FunctionID = FunctionIDHidden;

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
            Tbl_FMRes_Section sectionTable = new Tbl_FMRes_Section();

            sectionTable.SectionTitle = GlobalFunction.ReturnEmptyString(model.getSectionColumns.SectionTitle);
            sectionTable.DeptID = GlobalFunction.ReturnEmptyInt(model.DeptID);
            sectionTable.FunctionID = GlobalFunction.ReturnEmptyInt(model.FunctionID);
            BOSSDB.Tbl_FMRes_Section.Add(sectionTable);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Get Function Update Partial View
        public ActionResult Get_UpdateSection(SectionModel model, int SectionID)
        {
            Tbl_FMRes_Section officeSecTable = (from e in BOSSDB.Tbl_FMRes_Section where e.SectionID == SectionID select e).FirstOrDefault();
           
            model.getSectionColumns.SectionTitle = officeSecTable.SectionTitle;
            model.DeptID = Convert.ToInt32(officeSecTable.DeptID);
            model.FunctionIDHidden = Convert.ToInt32(officeSecTable.FunctionID);

            return PartialView("SectionTab/_UpdateSection", model);
        }
        //Update Office Section 
        public ActionResult UpdateSection(SectionModel model)
        {
            Tbl_FMRes_Section officeSecTBL = (from e in BOSSDB.Tbl_FMRes_Section where e.SectionID == model.SectionID select e).FirstOrDefault();

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
            Tbl_FMRes_Section officeSecTBL = (from e in BOSSDB.Tbl_FMRes_Section where e.SectionID == SectionID select e).FirstOrDefault();
            BOSSDB.Tbl_FMRes_Section.Remove(officeSecTBL);
            BOSSDB.SaveChanges();
            return RedirectToAction("FileResponsibility");
        }
    }
}

