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
        //===============================================================================================
        //Department Tab
        //===============================================================================================
        public ActionResult GetDeptTab()
        {
            return PartialView("DepartmentTab/IndexDeptTab");
        }
        public ActionResult GetDeptForm(int ActionID, int DeptID)
        {
            DepartmentModel model = new DepartmentModel();

            if (ActionID == 2)
            {
                var deptTbl = (from a in BOSSDB.Tbl_FMRes_Department where a.DeptID == DeptID select a).FirstOrDefault();
                model.DepartmentList.DeptTitle = deptTbl.DeptTitle;
                model.DepartmentList.DeptAbbrv = deptTbl.DeptAbbrv;
                model.DepartmentList.DeptOfficeCode = deptTbl.DeptOfficeCode;
                model.DepartmentList.RCcode = deptTbl.RCcode;
                model.DepartmentList.FundID = GlobalFunction.ReturnEmptyInt(deptTbl.FundID);
                model.DepartmentList.SectorID = GlobalFunction.ReturnEmptyInt(deptTbl.SectorID);
                model.DepartmentList.SubSectorID = GlobalFunction.ReturnEmptyInt(deptTbl.SubSectorID);
                model.DepartmentList.OfficeTypeID = GlobalFunction.ReturnEmptyInt(deptTbl.OfficeTypeID);
                model.DepartmentList.DeptID = deptTbl.DeptID;

                var getSubSector = (from a in BOSSDB.Tbl_FMSector_SubSector where a.SectorID == model.DepartmentList.SectorID select a).ToList();
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

            model.OfficeTypeList = new SelectList(BOSSDB.Tbl_FMOfficeType, "OfficeTypeID", "OfficeTypeTitle");
            model.OfficeTypeList = (from li in model.OfficeTypeList orderby li.Text select li).ToList();



            model.ActionID = ActionID;

            return PartialView("DepartmentTab/_DeptForm", model);
        }
        public ActionResult GetDeptDTable()
        {
            DepartmentModel model = new DepartmentModel();
            
            var SQLQuery = @"SELECT Tbl_FMRes_Department.*, Tbl_FMSector_Sector.SectorTitle, Tbl_FMSector_SubSector.SubSectorTitle,
                            Tbl_FMFund_Fund.FundTitle, Tbl_FMOfficeType.OfficeTypeTitle
                            FROM Tbl_FMRes_Department
                            INNER JOIN Tbl_FMFund_Fund ON
                            Tbl_FMRes_Department.FundID = Tbl_FMFund_Fund.FundID
                            INNER JOIN Tbl_FMSector_Sector ON
                            Tbl_FMRes_Department.SectorID = Tbl_FMSector_Sector.SectorID
                            LEFT JOIN Tbl_FMSector_SubSector ON 
                            Tbl_FMRes_Department.SubSectorID = Tbl_FMSector_SubSector.SubSectorID
                            INNER JOIN Tbl_FMOfficeType ON
                            Tbl_FMRes_Department.OfficeTypeID = Tbl_FMOfficeType.OfficeTypeID";

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

                        model.getDepartmentList.Add(new DepartmentList()
                        {
                            DeptID = GlobalFunction.ReturnEmptyInt(dr[0]), //okie
                            DeptTitle = GlobalFunction.ReturnEmptyString(dr[1]), //okie
                            DeptAbbrv = GlobalFunction.ReturnEmptyString(dr[2]), //okie
                            DeptOfficeCode = GlobalFunction.ReturnEmptyString(dr[3]),
                            RCcode = GlobalFunction.ReturnEmptyString(dr[4]),
                            SectorTitle = GlobalFunction.ReturnEmptyString(dr[9]),
                            SubSectorID = GlobalFunction.ReturnEmptyInt(dr[6]),
                            FundTitle = GlobalFunction.ReturnEmptyString(dr[11]),
                            OfficeTypeTitle = GlobalFunction.ReturnEmptyString(dr[12]),

                        });
                    }
                }
                Connection.Close();
            }
            return PartialView("DepartmentTab/_TableDepartment", model.getDepartmentList);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveDept(DepartmentModel model)
        {
            var isExist = "";

            if (ModelState.IsValid)
            {
                var deptTitle = GlobalFunction.RemoveSpaces(model.DepartmentList.DeptTitle);
                var deptAbbrv = GlobalFunction.RemoveSpaces(model.DepartmentList.DeptAbbrv);
                var DeptOfficeCode = GlobalFunction.RemoveSpaces(model.DepartmentList.DeptOfficeCode);
                var RCcode = GlobalFunction.RemoveSpaces(model.DepartmentList.RCcode);
                var fundID = model.DepartmentList.FundID;
                var sectorID = model.DepartmentList.SectorID;
                var officeTypeID = model.DepartmentList.OfficeTypeID;


                int? subSectorID = model.DepartmentList.SubSectorID;
                if (subSectorID == 000 || subSectorID == 0)
                {
                    subSectorID = null;
                }
                else
                {
                    subSectorID = model.DepartmentList.SubSectorID;
                }

                List<Tbl_FMRes_Department> deptFields = (from a in BOSSDB.Tbl_FMRes_Department where (a.FundID == fundID && a.SectorID == sectorID) && (a.SubSectorID == subSectorID && a.OfficeTypeID == officeTypeID) select a).ToList();
                Tbl_FMRes_Department deptTbl = (from a in BOSSDB.Tbl_FMRes_Department where a.DeptID == model.DepartmentList.DeptID select a).FirstOrDefault();
                var save = false;
                if (deptFields.Count > 0)
                {
                    foreach (var item in deptFields)
                    {
                        if (deptTbl != null)
                        {
                            if (GlobalFunction.AutoCaps_RemoveSpaces(item.DeptTitle) == GlobalFunction.AutoCaps_RemoveSpaces(deptTitle) &&
                                GlobalFunction.AutoCaps_RemoveSpaces(item.DeptOfficeCode) == GlobalFunction.AutoCaps_RemoveSpaces(DeptOfficeCode) &&
                                item.DeptID == deptTbl.DeptID)  // walang binago
                            {
                                save = true;
                            }
                            else if (GlobalFunction.AutoCaps_RemoveSpaces(item.DeptTitle) != GlobalFunction.AutoCaps_RemoveSpaces(deptTitle) &&
                                GlobalFunction.AutoCaps_RemoveSpaces(item.DeptOfficeCode) != GlobalFunction.AutoCaps_RemoveSpaces(DeptOfficeCode) || item.DeptID == deptTbl.DeptID) // may binago pero walang kaparehas
                            {
                                save = true;
                            }
                            else if (GlobalFunction.AutoCaps_RemoveSpaces(item.DeptTitle) == GlobalFunction.AutoCaps_RemoveSpaces(deptTitle) ||
                                GlobalFunction.AutoCaps_RemoveSpaces(item.DeptOfficeCode) == GlobalFunction.AutoCaps_RemoveSpaces(DeptOfficeCode)) // may binago pero may kaparehas
                            {
                                save = false;
                                break;
                            }
                        }
                        else
                        {
                            if (GlobalFunction.AutoCaps_RemoveSpaces(item.DeptTitle) != GlobalFunction.AutoCaps_RemoveSpaces(deptTitle) &&
                                GlobalFunction.AutoCaps_RemoveSpaces(item.DeptOfficeCode) != GlobalFunction.AutoCaps_RemoveSpaces(DeptOfficeCode)) // for adding
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
                                Tbl_FMRes_Department deptTblAdd = new Tbl_FMRes_Department();
                                deptTblAdd.DeptTitle = deptTitle;
                                deptTblAdd.DeptAbbrv = deptAbbrv;
                                deptTblAdd.DeptOfficeCode = DeptOfficeCode;
                                deptTblAdd.RCcode = RCcode;
                                deptTblAdd.FundID = fundID;
                                deptTblAdd.SectorID = sectorID;
                                deptTblAdd.SubSectorID = subSectorID;
                                deptTblAdd.OfficeTypeID = officeTypeID;

                                BOSSDB.Tbl_FMRes_Department.Add(deptTblAdd);
                                BOSSDB.SaveChanges();
                                isExist = "false";
                                break;

                            case 2:
                                deptTbl.DeptTitle = deptTitle;
                                deptTbl.DeptAbbrv = deptAbbrv;
                                deptTbl.DeptOfficeCode = DeptOfficeCode;
                                deptTbl.RCcode = RCcode;
                                deptTbl.FundID = fundID;
                                deptTbl.SectorID = sectorID;
                                deptTbl.SubSectorID = subSectorID;
                                deptTbl.OfficeTypeID = officeTypeID;

                                BOSSDB.Entry(deptTbl);
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
        public ActionResult DeleteDept(int PrimaryID)
        {
            Tbl_FMRes_Department deptTbl = (from a in BOSSDB.Tbl_FMRes_Department where a.DeptID == PrimaryID select a).FirstOrDefault();
            Tbl_FMRes_Function functTbl = (from a in BOSSDB.Tbl_FMRes_Function where a.DeptID == PrimaryID select a).FirstOrDefault();
            var confirmDelete = "";
            if (deptTbl != null)
            {
                if (functTbl != null)
                {
                    confirmDelete = "restricted";
                }

                else
                {
                    confirmDelete = "false";
                }

            }
            var result = new { confirmDelete = confirmDelete };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConfirmDeleteDept(int PrimaryID)
        {
            Tbl_FMRes_Department deptTbl = (from a in BOSSDB.Tbl_FMRes_Department where a.DeptID == PrimaryID select a).FirstOrDefault();

            BOSSDB.Tbl_FMRes_Department.Remove(deptTbl);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //===============================================================================================
        //Function Tab
        //===============================================================================================
        public ActionResult GetFunctionTab()
        {
            return PartialView("FunctionTab/IndexFuncTab");
        }
        public ActionResult GetFuncForm(int ActionID, int FunctionID)
        {
            FunctionModel model = new FunctionModel();

            if (ActionID == 2)
            {
                Tbl_FMRes_Function funcTbl = (from a in BOSSDB.Tbl_FMRes_Function where a.FunctionID == FunctionID select a).FirstOrDefault();
                model.FunctionList.FunctionTitle = funcTbl.FunctionTitle;
                model.FunctionList.FunctionAbbrv = funcTbl.FunctionAbbrv;
                model.FunctionList.FunctionCode = funcTbl.FunctionCode;
                model.FunctionList.SectorID = GlobalFunction.ReturnEmptyInt(funcTbl.SectorID);
                model.FunctionList.SubSectorID = GlobalFunction.ReturnEmptyInt(funcTbl.SubSectorID);
                model.FunctionList.OfficeTypeID = GlobalFunction.ReturnEmptyInt(funcTbl.OfficeTypeID);
                model.FunctionList.DeptID = GlobalFunction.ReturnEmptyInt(funcTbl.DeptID);
                model.FunctionList.FundTitle = GlobalFunction.ReturnEmptyString(funcTbl.Tbl_FMRes_Department.Tbl_FMFund_Fund.FundTitle);

                model.FunctionList.DeptOfficeCodefunc = funcTbl.DeptOfficeCodefunc;
                model.FunctionList.FunctionID = funcTbl.FunctionID;

                var getSubSector = (from a in BOSSDB.Tbl_FMSector_SubSector where a.SectorID == model.FunctionList.SectorID select a).ToList();
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
                var getFirstDept = (from a in BOSSDB.Tbl_FMRes_Department orderby a.DeptTitle select a).FirstOrDefault();
                if (getFirstDept != null)
                {
                    model.FunctionList.FundTitle = getFirstDept.Tbl_FMFund_Fund.FundTitle;
                    model.FunctionList.DeptOfficeCodefunc = getFirstDept.DeptOfficeCode;
                }


            }

            model.DeptList = new SelectList(BOSSDB.Tbl_FMRes_Department, "DeptID", "DeptTitle");
            model.DeptList = (from li in model.DeptList orderby li.Text select li).ToList();

            model.SectorList = new SelectList(BOSSDB.Tbl_FMSector_Sector, "SectorID", "SectorTitle");
            model.SectorList = (from li in model.SectorList orderby li.Text select li).ToList();

            model.OfficeTypeList = new SelectList(BOSSDB.Tbl_FMOfficeType, "OfficeTypeID", "OfficeTypeTitle");
            model.OfficeTypeList = (from li in model.OfficeTypeList orderby li.Text select li).ToList();




            model.ActionID = ActionID;

            return PartialView("FunctionTab/_FuncForm", model);
        }
        public ActionResult GetFuncDTable()
        {
            FunctionModel model = new FunctionModel();

            var SQLQuery = @"SELECT Tbl_FMRes_Function.*, Tbl_FMSector_Sector.SectorTitle, Tbl_FMSector_SubSector.SubSectorTitle,
                            Tbl_FMRes_Department.DeptTitle, Tbl_FMOfficeType.OfficeTypeTitle, Tbl_FMFund_Fund.FundTitle
                            FROM Tbl_FMRes_Function
							INNER JOIN Tbl_FMRes_Department ON
                            Tbl_FMRes_Function.DeptID = Tbl_FMRes_Department.DeptID
							INNER JOIN Tbl_FMFund_Fund ON
                            Tbl_FMRes_Department.FundID = Tbl_FMFund_Fund.FundID
                            INNER JOIN Tbl_FMSector_Sector ON
                            Tbl_FMRes_Function.SectorID = Tbl_FMSector_Sector.SectorID
                            LEFT JOIN Tbl_FMSector_SubSector ON 
                            Tbl_FMRes_Function.SubSectorID = Tbl_FMSector_SubSector.SubSectorID
                            INNER JOIN Tbl_FMOfficeType ON
                            Tbl_FMRes_Function.OfficeTypeID = Tbl_FMOfficeType.OfficeTypeID";

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

                        model.getFunctionList.Add(new FunctionList()
                        {

                            FunctionID = GlobalFunction.ReturnEmptyInt(dr[0]), //okie
                            FunctionTitle = GlobalFunction.ReturnEmptyString(dr[1]), //okie
                            FunctionAbbrv = GlobalFunction.ReturnEmptyString(dr[2]), //okie
                            FunctionCode = GlobalFunction.ReturnEmptyString(dr[3]),
                            DeptOfficeCodefunc = GlobalFunction.ReturnEmptyString(dr[7]),

                            SectorTitle = GlobalFunction.ReturnEmptyString(dr[9]),
                            SubSectorID = GlobalFunction.ReturnEmptyInt(dr[5]),
                            DeptTitle = GlobalFunction.ReturnEmptyString(dr[11]),
                            OfficeTypeTitle = GlobalFunction.ReturnEmptyString(dr[12]),
                            FundTitle = GlobalFunction.ReturnEmptyString(dr[13]),
                        });
                    }
                }
                Connection.Close();
            }
            return PartialView("FunctionTab/_TableFunc", model.getFunctionList);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveFunction(FunctionModel model)
        {
            var isExist = "";

            if (ModelState.IsValid)
            {
                var FunctionTitle = GlobalFunction.RemoveSpaces(model.FunctionList.FunctionTitle);
                var FunctionAbbrv = GlobalFunction.RemoveSpaces(model.FunctionList.FunctionAbbrv);
                var FunctionCode = GlobalFunction.RemoveSpaces(model.FunctionList.FunctionCode);
                var DeptOfficeCodefunc = GlobalFunction.RemoveSpaces(model.FunctionList.DeptOfficeCodefunc);
                var SectorID = model.FunctionList.SectorID;
                var OfficeTypeID = model.FunctionList.OfficeTypeID;
                var DeptID = model.FunctionList.DeptID;
                var FunctionID = model.FunctionList.FunctionID;

                int? SubSectorID = model.FunctionList.SubSectorID;
                if (SubSectorID == 000 || SubSectorID == 0)
                {
                    SubSectorID = null;
                }
                else
                {
                    SubSectorID = model.FunctionList.SubSectorID;
                }

                List<Tbl_FMRes_Function> functionList = (from a in BOSSDB.Tbl_FMRes_Function where (a.DeptID == DeptID) select a).ToList();
                Tbl_FMRes_Function functionRecord = (from a in BOSSDB.Tbl_FMRes_Function where a.FunctionID == FunctionID select a).FirstOrDefault();
                var save = false;
                if (functionList.Count > 0)
                {
                    foreach (var item in functionList)
                    {
                        if (functionRecord != null)
                        {
                            if (GlobalFunction.AutoCaps_RemoveSpaces(item.FunctionTitle) == GlobalFunction.AutoCaps_RemoveSpaces(FunctionTitle) &&
                                GlobalFunction.AutoCaps_RemoveSpaces(item.FunctionCode) == GlobalFunction.AutoCaps_RemoveSpaces(FunctionCode) &&
                                item.DeptID == functionRecord.DeptID)  // walang binago
                            {
                                save = true;
                            }
                            else if (GlobalFunction.AutoCaps_RemoveSpaces(item.FunctionTitle) != GlobalFunction.AutoCaps_RemoveSpaces(FunctionTitle) &&
                               GlobalFunction.AutoCaps_RemoveSpaces(item.FunctionCode) != GlobalFunction.AutoCaps_RemoveSpaces(FunctionCode) ||
                               item.DeptID == functionRecord.DeptID) // may binago pero walang kaparehas
                            {
                                save = true;
                            }
                            else if (GlobalFunction.AutoCaps_RemoveSpaces(item.FunctionTitle) == GlobalFunction.AutoCaps_RemoveSpaces(FunctionTitle) ||
                                GlobalFunction.AutoCaps_RemoveSpaces(item.FunctionCode) == GlobalFunction.AutoCaps_RemoveSpaces(FunctionCode)) // may binago pero may kaparehas
                            {
                                save = false;
                                break;
                            }
                        }
                        else
                        {
                            if (GlobalFunction.AutoCaps_RemoveSpaces(item.FunctionTitle) != GlobalFunction.AutoCaps_RemoveSpaces(FunctionTitle) &&
                                GlobalFunction.AutoCaps_RemoveSpaces(item.FunctionCode) != GlobalFunction.AutoCaps_RemoveSpaces(FunctionCode)) // for adding
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
                                Tbl_FMRes_Function functionAdd = new Tbl_FMRes_Function();
                                functionAdd.FunctionTitle = FunctionTitle;
                                functionAdd.FunctionAbbrv = FunctionAbbrv;
                                functionAdd.FunctionCode = FunctionCode;
                                functionAdd.DeptOfficeCodefunc = DeptOfficeCodefunc;
                                functionAdd.SectorID = SectorID;
                                functionAdd.SubSectorID = SubSectorID;
                                functionAdd.OfficeTypeID = OfficeTypeID;
                                functionAdd.DeptID = DeptID;

                                BOSSDB.Tbl_FMRes_Function.Add(functionAdd);
                                BOSSDB.SaveChanges();
                                isExist = "false";
                                break;

                            case 2:
                                functionRecord.FunctionTitle = FunctionTitle;
                                functionRecord.FunctionAbbrv = FunctionAbbrv;
                                functionRecord.FunctionCode = FunctionCode;
                                functionRecord.DeptOfficeCodefunc = DeptOfficeCodefunc;
                                functionRecord.SectorID = SectorID;
                                functionRecord.SubSectorID = SubSectorID;
                                functionRecord.OfficeTypeID = OfficeTypeID;
                                functionRecord.DeptID = DeptID;

                                BOSSDB.Entry(functionRecord);
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
        public ActionResult DeleteFunction(int PrimaryID)
        {
            Tbl_FMRes_Function functionTbl = (from a in BOSSDB.Tbl_FMRes_Function where a.FunctionID == PrimaryID select a).FirstOrDefault();
            Tbl_FMRes_Section sectionRecord = (from a in BOSSDB.Tbl_FMRes_Section where a.FunctionID == PrimaryID select a).FirstOrDefault();
            var confirmDelete = "";
            if (functionTbl != null)
            {
                if (sectionRecord != null)
                {
                    confirmDelete = "restricted";
                }

                else
                {
                    confirmDelete = "false";
                }

            }
            var result = new { confirmDelete = confirmDelete };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConfirmDeleteFunction(int PrimaryID)
        {
            Tbl_FMRes_Function functionTbl = (from a in BOSSDB.Tbl_FMRes_Function where a.FunctionID == PrimaryID select a).FirstOrDefault();

            BOSSDB.Tbl_FMRes_Function.Remove(functionTbl);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //===============================================================================================
        //Section Tab
        //===============================================================================================
        public ActionResult GetSecTab()
        {
            return PartialView("SectionTab/IndexSectionTab");
        }
        public ActionResult GetSectionForm(int ActionID, int SectionID)
        {
            SectionModel model = new SectionModel();

            if (ActionID == 2)
            {
                Tbl_FMRes_Section sectionTbl = (from a in BOSSDB.Tbl_FMRes_Section where a.SectionID == SectionID select a).FirstOrDefault();
                model.SectionList.SectionTitle = sectionTbl.SectionTitle;
                model.SectionList.FunctionID = GlobalFunction.ReturnEmptyInt(sectionTbl.FunctionID);
                model.SectionList.DeptID = GlobalFunction.ReturnEmptyInt(sectionTbl.Tbl_FMRes_Function.Tbl_FMRes_Department.DeptID);
                model.SectionList.SectionID = sectionTbl.SectionID;
                var functionList = (from a in BOSSDB.Tbl_FMRes_Function where a.DeptID == model.SectionList.DeptID orderby a.FunctionTitle select a).ToList();
                model.FunctionList = new SelectList(functionList, "FunctionID", "FunctionTitle");
            }
            else
            {
                model.SectionList.DeptID = (from a in BOSSDB.Tbl_FMRes_Department orderby a.DeptTitle select a.DeptID).FirstOrDefault();
                var functionList = (from a in BOSSDB.Tbl_FMRes_Function where a.DeptID == model.SectionList.DeptID orderby a.FunctionTitle select a).ToList();
                model.FunctionList = new SelectList(functionList, "FunctionID", "FunctionTitle");
            }

            model.DeptList = new SelectList(BOSSDB.Tbl_FMRes_Department, "DeptID", "DeptTitle");
            model.DeptList = (from li in model.DeptList orderby li.Text select li).ToList();


            model.ActionID = ActionID;
            return PartialView("SectionTab/_SectionForm", model);
        }
        public ActionResult GetSectionDTable()
        {
            SectionModel model = new SectionModel();

            var SQLQuery = @"SELECT Tbl_FMRes_Section.*, Tbl_FMRes_Function.FunctionTitle, Tbl_FMRes_Department.DeptTitle
                            FROM Tbl_FMRes_Section
                            INNER JOIN Tbl_FMRes_Function ON 
                            Tbl_FMRes_Section.FunctionID = Tbl_FMRes_Function.FunctionID
                            INNER JOIN Tbl_FMRes_Department ON 
                            Tbl_FMRes_Function.DeptID = Tbl_FMRes_Department.DeptID  ";

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

                        model.getSectionList.Add(new SectionList()
                        {
                            SectionID = GlobalFunction.ReturnEmptyInt(dr[0]), 
                            SectionTitle = GlobalFunction.ReturnEmptyString(dr[1]),
                            FunctionTitle = GlobalFunction.ReturnEmptyString(dr[4]),
                            DeptTitle = GlobalFunction.ReturnEmptyString(dr[5]),
                        });
                    }
                }
                Connection.Close();
            }
            return PartialView("SectionTab/_TableSection", model.getSectionList);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSection(SectionModel model)
        {
            var isExist = "";

            if (ModelState.IsValid)
            {
                var SectionTitle = GlobalFunction.RemoveSpaces(model.SectionList.SectionTitle);
                var FunctionID = model.SectionList.FunctionID;
                var SectionID = model.SectionList.SectionID;
                var DeptID = model.SectionList.DeptID;


                List<Tbl_FMRes_Section> sectionList = (from a in BOSSDB.Tbl_FMRes_Section where (a.FunctionID == FunctionID && a.Tbl_FMRes_Function.DeptID == DeptID) select a).ToList();
                Tbl_FMRes_Section sectionRecord = (from a in BOSSDB.Tbl_FMRes_Section where a.SectionID == SectionID select a).FirstOrDefault();
                var save = false;
                if (sectionList.Count > 0)
                {
                    foreach (var item in sectionList)
                    {
                        if (sectionRecord != null)
                        {
                            if (GlobalFunction.AutoCaps_RemoveSpaces(item.SectionTitle) == GlobalFunction.AutoCaps_RemoveSpaces(SectionTitle) && item.SectionID == sectionRecord.SectionID)  // walang binago
                            {
                                save = true;
                            }
                            else if (GlobalFunction.AutoCaps_RemoveSpaces(item.SectionTitle) != GlobalFunction.AutoCaps_RemoveSpaces(SectionTitle) || item.SectionID == sectionRecord.SectionID) // may binago pero walang kaparehas
                            {
                                save = true;
                            }
                            else if (GlobalFunction.AutoCaps_RemoveSpaces(item.SectionTitle) == GlobalFunction.AutoCaps_RemoveSpaces(SectionTitle)) // may binago pero may kaparehas
                            {
                                save = false;
                                break;
                            }
                        }
                        else
                        {
                            if (GlobalFunction.AutoCaps_RemoveSpaces(item.SectionTitle) != GlobalFunction.AutoCaps_RemoveSpaces(SectionTitle)) // for adding
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
                                Tbl_FMRes_Section sectionAdd = new Tbl_FMRes_Section();
                                sectionAdd.SectionTitle = SectionTitle;
                                sectionAdd.FunctionID = FunctionID;

                                BOSSDB.Tbl_FMRes_Section.Add(sectionAdd);
                                BOSSDB.SaveChanges();
                                isExist = "false";
                                break;

                            case 2:
                                sectionRecord.SectionTitle = SectionTitle;
                                sectionRecord.FunctionID = FunctionID;

                                BOSSDB.Entry(sectionRecord);
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

        public ActionResult DeleteSection(int PrimaryID)
        {
            Tbl_FMRes_Section section = (from a in BOSSDB.Tbl_FMRes_Section where a.SectionID == PrimaryID select a).FirstOrDefault();
            //Tbl_FMRes_Section sectionRecord = (from a in BOSSDB.Tbl_FMRes_Section where a.FunctionID == PrimaryID select a).FirstOrDefault();
            var confirmDelete = "";
            if (section != null)
            {
                //if (sectionRecord != null)
                //{
                //    confirmDelete = "restricted";
                //}

                //else
                //{
                //    confirmDelete = "false";
                //}
                confirmDelete = "false";

            }
            var result = new { confirmDelete = confirmDelete };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConfirmDeleteSection(int PrimaryID)
        {
            Tbl_FMRes_Section section = (from a in BOSSDB.Tbl_FMRes_Section where a.SectionID == PrimaryID select a).FirstOrDefault();

            BOSSDB.Tbl_FMRes_Section.Remove(section);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //=========================================================================================
        //ONCHANGE
        //=========================================================================================
        public ActionResult ChangeSector_SubSector(int SectorID, DepartmentModel model)
        {
            var subSectorTbl = (from a in BOSSDB.Tbl_FMSector_SubSector where a.SectorID == SectorID orderby a.SubSectorTitle select a).ToList();
            //foreach (var item in subSectorTbl)
            //{
            //    model.SubSectorList.Add(new SelectListItem() { Text = item.SubSectorTitle, Value = item.SubSectorID.ToString() });

            //}
            return Json(new SelectList(subSectorTbl, "SubSectorID", "SubSectorTitle"), JsonRequestBehavior.AllowGet);

        }

        public ActionResult ChangeDepartment_FundDeptCode(int DeptID, DepartmentModel model)
        {
            var DeptOfficeCodefunc = "";
            var fundTitle = "";
            var deptTbl = (from a in BOSSDB.Tbl_FMRes_Department where a.DeptID == DeptID select a).FirstOrDefault();
            if (deptTbl != null)
            {
                DeptOfficeCodefunc = deptTbl.DeptOfficeCode;
                fundTitle = deptTbl.Tbl_FMFund_Fund.FundTitle;
            }
            var result = new { DeptOfficeCodefunc = DeptOfficeCodefunc, fundTitle = fundTitle };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ChangeDepartment_Function(int DeptID, DepartmentModel model)
        {
            var function = (from a in BOSSDB.Tbl_FMRes_Function where a.DeptID == DeptID orderby a.FunctionTitle select a).ToList();
            return Json(new SelectList(function, "FunctionID", "FunctionTitle"), JsonRequestBehavior.AllowGet);

        }
    }
}

