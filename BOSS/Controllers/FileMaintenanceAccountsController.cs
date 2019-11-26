using BOSS.GlobalFunctions;
using BOSS.Models;
using BOSS.Models.FMmodels.FMAccountsModels;
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
    public class FileMaintenanceAccountsController : Controller
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        DatatypeValidation GlobalFunction = new DatatypeValidation();
        [Authorize]
        public ActionResult FileAccounts()
        {
            GeneralAccountModel model = new GeneralAccountModel();
            return View(model);
        }
        public ActionResult RevisionCOATab()
        {
            RevisionYearModel model = new RevisionYearModel();
            return PartialView("RevisionOfCOA/IndexRCOA", model);
        }
        public ActionResult AllotmentClassTab()
        {
            FMMainChartofAccountModel model = new FMMainChartofAccountModel();
            return PartialView("AllotmentClass/IndexAllotmentClass", model);
        }
        public ActionResult AccountGroupTab()
        {
            FMMainChartofAccountModel model = new FMMainChartofAccountModel();
            return PartialView("AccountGroup/IndexAccountGroup", model);
        }
        public ActionResult MajorAccountGroupTab()
        {
            RevisionYearModel model = new RevisionYearModel();
            return PartialView("MajorAccountGroup/IndexMajorAccountGroup", model);
        }
        public ActionResult SubMajorAccountGroupTab()
        {
            RevisionYearModel model = new RevisionYearModel();
            return PartialView("SubMajorAccountGroup/IndexSubMajorAccountGroup", model);
        }
        public ActionResult GeneralAccountTab()
        {
            GeneralAccountModel model = new GeneralAccountModel();
            return PartialView("GeneralAccount/IndexGeneralAccount", model);
        }
        public ActionResult SubsiLedgerTab()
        {
            GeneralAccountModel model = new GeneralAccountModel();
            return PartialView("SubsidiaryLedger/_IndexSubsidiaryLedger", model);
        }
        //--------------------------------------------------------------------------------------------------------------------
        //Revision Year
        //--------------------------------------------------------------------------------------------------------------------
        public ActionResult GetRevisionCOADT()
        {
            RevisionYearModel model = new RevisionYearModel();
            List<RevisionList> getRevisionYearList = new List<RevisionList>();
            var SQLQuery = "";
            SQLQuery = "SELECT [RevID], [RevYEar], [isUsed], [Remarks] FROM [BOSS].[dbo].[Tbl_FMCOA_RevisionYear]";

            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_FMAccounts]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        getRevisionYearList.Add(new RevisionList()
                        {
                            RevID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            RevYEar = GlobalFunction.ReturnEmptyInt(dr[1]),
                            isUsed = GlobalFunction.ReturnEmptyBool(dr[2]),
                            Remarks = GlobalFunction.ReturnEmptyString(dr[3])
                        });
                    }
                }
                Connection.Close();
            }
            model.getRevisionYearList = getRevisionYearList.ToList();
            return PartialView("RevisionOfCOA/_TableRCOA", getRevisionYearList);
        }
        public ActionResult GetAddRevision()
        {
            RevisionYearModel model = new RevisionYearModel();
            return PartialView("RevisionOfCOA/_AddRCOA", model);

        }
        public ActionResult AddNewRevisionCOA(RevisionYearModel model)
        {
            Tbl_FMCOA_AllotmentClass tblAllotment = new Tbl_FMCOA_AllotmentClass();
            tblAllotment.RevID = model.RevID;
            tblAllotment.AllotmentClassTitle = "N/A";
            BOSSDB.Tbl_FMCOA_AllotmentClass.Add(tblAllotment);


            Tbl_FMCOA_RevisionYear RevCOATable = new Tbl_FMCOA_RevisionYear();
            RevCOATable.RevYear = GlobalFunction.ReturnEmptyInt(model.getRevYearColumns.RevYear);
            RevCOATable.isUsed = model.isUsed;
            RevCOATable.Remarks = GlobalFunction.ReturnEmptyString(model.getRevYearColumns.Remarks);
            BOSSDB.Tbl_FMCOA_RevisionYear.Add(RevCOATable);

            BOSSDB.SaveChanges();
            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Get_UpdateRevCOA(RevisionYearModel model, int RevID)
        {
            Tbl_FMCOA_RevisionYear revCOATable = (from e in BOSSDB.Tbl_FMCOA_RevisionYear where e.RevID == RevID select e).FirstOrDefault();

            model.getRevYearColumns.RevYear = revCOATable.RevYear;
            model.isUsed = Convert.ToBoolean(revCOATable.isUsed);
            model.getRevYearColumns.Remarks = revCOATable.Remarks;
            model.RevID = RevID;
            return PartialView("RevisionOfCOA/_UpdateRCOA", model);
        }
        public ActionResult UpdateRevCOA(RevisionYearModel model)
        {
            Tbl_FMCOA_RevisionYear revCOATbl = (from e in BOSSDB.Tbl_FMCOA_RevisionYear where e.RevID == model.RevID select e).FirstOrDefault();
            revCOATbl.RevYear = GlobalFunction.ReturnEmptyInt(model.getRevYearColumns.RevYear);
            revCOATbl.isUsed = model.isUsed;
            revCOATbl.Remarks = GlobalFunction.ReturnEmptyString(model.getRevYearColumns.Remarks);

            BOSSDB.Entry(revCOATbl);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteRCOA(RevisionYearModel model, int RevID)
        {
            Tbl_FMCOA_RevisionYear RCOAtbl = (from e in BOSSDB.Tbl_FMCOA_RevisionYear where e.RevID == RevID select e).FirstOrDefault();
            BOSSDB.Tbl_FMCOA_RevisionYear.Remove(RCOAtbl);
            BOSSDB.SaveChanges();
            return RedirectToAction("FileAccounts");
        }
        //--------------------------------------------------------------------------------------------------------------------
        //Allotment Class
        //--------------------------------------------------------------------------------------------------------------------
        public ActionResult GetAllotmentClassDT()
        {
            AllotmentClassModel model = new AllotmentClassModel();
            List<AllotmentClassList> getAllotmentClassList = new List<AllotmentClassList>();
            var SQLQuery = "";
            SQLQuery = "SELECT [AllotmentClassID],[AllotmentClassTitle],[RevYear] FROM [BOSS].[dbo].[Tbl_FMCOA_AllotmentClass],[Tbl_FMCOA_RevisionYear] where [Tbl_FMCOA_AllotmentClass].RevID = [Tbl_FMCOA_RevisionYear].RevID and [Tbl_FMCOA_AllotmentClass].AllotmentClassTitle != 'N/A'";

            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_FMAccounts]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        getAllotmentClassList.Add(new AllotmentClassList()
                        {
                            AllotmentClassID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            AllotmentClassTitle = GlobalFunction.ReturnEmptyString(dr[1]),
                            RevYear = GlobalFunction.ReturnEmptyString(dr[2])
                        });
                    }
                }
                Connection.Close();
            }
            model.getAllotmentClassList = getAllotmentClassList.ToList();
            return PartialView("AllotmentClass/_TableAllotment", getAllotmentClassList);
        }
        public ActionResult GetAddAllotmentClass()
        {
            FMMainChartofAccountModel model = new FMMainChartofAccountModel();
            return PartialView("AllotmentClass/_AddAllotment", model);
        }
        public ActionResult AddNewAllotmentClass(FMMainChartofAccountModel model)
        {
            Tbl_FMCOA_AllotmentClass AllotTable = new Tbl_FMCOA_AllotmentClass();

            AllotTable.RevID = model.AllotClassModel.RevID;
            AllotTable.AllotmentClassTitle = GlobalFunction.ReturnEmptyString(model.AllotClassModel.getAllotmentClassColumns.AllotmentClassTitle);

            BOSSDB.Tbl_FMCOA_AllotmentClass.Add(AllotTable);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDynamicRevYear()
        {
            FMMainChartofAccountModel model = new FMMainChartofAccountModel();
            model.AllotClassModel.RevYearDropDownList = new SelectList((from s in BOSSDB.Tbl_FMCOA_RevisionYear.Where(a => a.isUsed == true).ToList() select new { RevID = s.RevID, RevYear = s.RevYear }), "RevID", "RevYear");

            return PartialView("DynamicFields/_DynamicRevYear", model);
        }
        public ActionResult GetDynamicRevYearUpdate(int RevIDtemp)
        {
            FMMainChartofAccountModel model = new FMMainChartofAccountModel();
            model.AllotClassModel.RevYearDropDownList = new SelectList((from s in BOSSDB.Tbl_FMCOA_RevisionYear.Where(a => a.isUsed == true || a.RevID == RevIDtemp).ToList() select new { RevID = s.RevID, RevYear = s.RevYear }), "RevID", "RevYear");
            model.AllotClassModel.RevID = RevIDtemp;
            return PartialView("DynamicFields/_DynamicRevYear", model);
        }
        public ActionResult Get_UpdateAllotmentClass(FMMainChartofAccountModel model, int AllotmentClassID)
        {
            Tbl_FMCOA_AllotmentClass allotClassTBL = (from e in BOSSDB.Tbl_FMCOA_AllotmentClass where e.AllotmentClassID == AllotmentClassID select e).FirstOrDefault();

            model.AllotClassModel.AllotmentClassID = AllotmentClassID;
            model.AllotClassModel.getAllotmentClassColumns.AllotmentClassTitle = allotClassTBL.AllotmentClassTitle;
            model.AllotClassModel.RevIDtemp = Convert.ToInt32(allotClassTBL.RevID);
            return PartialView("AllotmentClass/_UpdateAllotmentClass", model);
        }
        public ActionResult UpdateAllotment(FMMainChartofAccountModel model)
        {
            Tbl_FMCOA_AllotmentClass allotClassTbl = (from e in BOSSDB.Tbl_FMCOA_AllotmentClass where e.AllotmentClassID == model.AllotClassModel.AllotmentClassID select e).FirstOrDefault();
            allotClassTbl.AllotmentClassTitle = GlobalFunction.ReturnEmptyString(model.AllotClassModel.getAllotmentClassColumns.AllotmentClassTitle);
            allotClassTbl.RevID = GlobalFunction.ReturnEmptyInt(model.AllotClassModel.RevID);

            BOSSDB.Entry(allotClassTbl);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteAllotmentClass(FMMainChartofAccountModel model, int AllotmentClassID)
        {
            Tbl_FMCOA_AllotmentClass allotmenttbl = (from e in BOSSDB.Tbl_FMCOA_AllotmentClass where e.AllotmentClassID == AllotmentClassID select e).FirstOrDefault();
            BOSSDB.Tbl_FMCOA_AllotmentClass.Remove(allotmenttbl);
            BOSSDB.SaveChanges();
            return RedirectToAction("FileAccounts");
        }
        //--------------------------------------------------------------------------------------------------------------------
        //Account Group Tab
        //--------------------------------------------------------------------------------------------------------------------
        public ActionResult GetAccountGroupDT()
        {
            AccountGroupModel model = new AccountGroupModel();
            List<AccountGroupList> getAccountGroupList = new List<AccountGroupList>();
            var SQLQuery = "";
            SQLQuery = "SELECT [AGID],[RevYear],[AllotmentClassTitle],[AGTitle],[AGCode] FROM [BOSS].[dbo].[Tbl_FMCOA_AccountGroup], [Tbl_FMCOA_RevisionYear],[Tbl_FMCOA_AllotmentClass] where [Tbl_FMCOA_AccountGroup].AllotmentClassID = [Tbl_FMCOA_AllotmentClass].AllotmentClassID and [Tbl_FMCOA_AllotmentClass].RevID = [Tbl_FMCOA_RevisionYear].RevID";

            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_FMAccounts]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        getAccountGroupList.Add(new AccountGroupList()
                        {
                            AGID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            RevYear = GlobalFunction.ReturnEmptyString(dr[1]),
                            AllotmentClassTitle = GlobalFunction.ReturnEmptyString(dr[2]),
                            AGTitle = GlobalFunction.ReturnEmptyString(dr[3]),
                            AGCode = GlobalFunction.ReturnEmptyString(dr[4])
                        });
                    }
                }
                Connection.Close();
            }
            model.getAccountGroupList = getAccountGroupList.ToList();
            return PartialView("AccountGroup/_TableAccountGroup", getAccountGroupList);
        }
        public ActionResult GetAddAccountGroup()
        {
            FMMainChartofAccountModel model = new FMMainChartofAccountModel();
            return PartialView("AccountGroup/_AddAccountGroup", model);
        }
        //public ActionResult GetDynamicRevYearAG()
        //{
        //    FMMainChartofAccountModel model = new FMMainChartofAccountModel();
        //    model.AllotClassModel.RevYearDropDownList = new SelectList((from s in BOSSDB.Tbl_FMCOA_RevisionYear.Where(a => a.isUsed == true).ToList() select new { RevID = s.RevID, RevYear = s.RevYear }), "RevID", "RevYear");

        //    return PartialView("DynamicFields/_DynamicRevYear", model);
        //}
        public ActionResult GetDynamicAllotClass(int RevID)
        {
            FMMainChartofAccountModel model = new FMMainChartofAccountModel();
            model.AccountGrpModel.AllotClasslist = new SelectList((from s in BOSSDB.Tbl_FMCOA_AllotmentClass.Where(a => a.RevID == RevID).ToList() select new { AllotmentClassID = s.AllotmentClassID, AllotmentClassTitle = s.AllotmentClassTitle }), "AllotmentClassID", "AllotmentClassTitle");

            return PartialView("DynamicFields/_DynamicAllotmentClass", model);
        }
        public ActionResult AddNewAccountGroup(FMMainChartofAccountModel model)
        {
            Tbl_FMCOA_AccountGroup AccntGrpTbl = new Tbl_FMCOA_AccountGroup();

            AccntGrpTbl.AllotmentClassID = model.AccountGrpModel.AllotmentClassID;
            AccntGrpTbl.AGTitle = GlobalFunction.ReturnEmptyString(model.AccountGrpModel.getAccountGroupColumns.AGTitle);
            AccntGrpTbl.AGCode = GlobalFunction.ReturnEmptyString(model.AccountGrpModel.getAccountGroupColumns.AGCode);

            BOSSDB.Tbl_FMCOA_AccountGroup.Add(AccntGrpTbl);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Get_UpdateAccountGroup(FMMainChartofAccountModel model, int AGID)
        {
            Tbl_FMCOA_AccountGroup AccntGrpTBL = (from e in BOSSDB.Tbl_FMCOA_AccountGroup where e.AGID == AGID select e).FirstOrDefault();

            model.AccountGrpModel.AGID = AGID;
            model.AccountGrpModel.getAccountGroupColumns.AGTitle = AccntGrpTBL.AGTitle;
            model.AccountGrpModel.getAccountGroupColumns.AGCode = AccntGrpTBL.AGCode;
            model.AccountGrpModel.allotclasssTempID = Convert.ToInt32(AccntGrpTBL.AllotmentClassID);
            model.AccountGrpModel.RevTempID = Convert.ToInt32(AccntGrpTBL.Tbl_FMCOA_AllotmentClass.Tbl_FMCOA_RevisionYear.RevID);
            return PartialView("AccountGroup/_UpdateAccountGroup", model);
        }
        public ActionResult GetDynamicRevYearAGUpdate(int RevTempID)
        {
            FMMainChartofAccountModel model = new FMMainChartofAccountModel();
            model.AllotClassModel.RevYearDropDownList = new SelectList((from s in BOSSDB.Tbl_FMCOA_RevisionYear.Where(a => a.isUsed == true || a.RevID == RevTempID).ToList() select new { RevID = s.RevID, RevYear = s.RevYear }), "RevID", "RevYear");
            model.AllotClassModel.RevID = RevTempID;
            return PartialView("DynamicFields/_DynamicRevYear", model);
        }
        public ActionResult GetDynamicAllotClassAGUpdate(FMMainChartofAccountModel model, int RevID, int allotclasssTempID)
        {
            model.AccountGrpModel.AllotClasslist = new SelectList((from s in BOSSDB.Tbl_FMCOA_AllotmentClass.Where(a => a.RevID == RevID).ToList() select new { AllotmentClassID = s.AllotmentClassID, AllotmentClassTitle = s.AllotmentClassTitle }), "AllotmentClassID", "AllotmentClassTitle");
            model.AccountGrpModel.AllotmentClassID = allotclasssTempID;
            return PartialView("DynamicFields/_DynamicAllotmentClass", model);
        }
        public ActionResult UpdateAccountGroup(FMMainChartofAccountModel model)
        {
            Tbl_FMCOA_AccountGroup accntgrp = (from e in BOSSDB.Tbl_FMCOA_AccountGroup where e.AGID == model.AccountGrpModel.AGID select e).FirstOrDefault();
            accntgrp.AGTitle = GlobalFunction.ReturnEmptyString(model.AccountGrpModel.getAccountGroupColumns.AGTitle);
            accntgrp.AGCode = GlobalFunction.ReturnEmptyString(model.AccountGrpModel.getAccountGroupColumns.AGCode);
            accntgrp.AllotmentClassID = GlobalFunction.ReturnEmptyInt(model.AccountGrpModel.AllotmentClassID);
            BOSSDB.Entry(accntgrp);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteAccountGroup(FMMainChartofAccountModel model, int AGID)
        {
            Tbl_FMCOA_AccountGroup Accntgrptbl = (from e in BOSSDB.Tbl_FMCOA_AccountGroup where e.AGID == AGID select e).FirstOrDefault();
            BOSSDB.Tbl_FMCOA_AccountGroup.Remove(Accntgrptbl);
            BOSSDB.SaveChanges();
            return RedirectToAction("FileAccounts");
        }
        //--------------------------------------------------------------------------------------------------------------------
        //Major Account Group Tab
        //--------------------------------------------------------------------------------------------------------------------
        public ActionResult GetMajorAccntGrpDT()
        {
            MajorAccountGroupModel model = new MajorAccountGroupModel();
            var SQLQuery ="SELECT [MAGID],[RevYear],[AllotmentClassTitle],[AGTitle],[MAGTitle],[AGCode]+' - '+[MAGCode] as MAGAccountCode FROM [BOSS].[dbo].[Tbl_FMCOA_MajorAccountGroup],[Tbl_FMCOA_AccountGroup],[Tbl_FMCOA_RevisionYear],[Tbl_FMCOA_AllotmentClass] where [Tbl_FMCOA_MajorAccountGroup].AGID = [Tbl_FMCOA_AccountGroup].AGID and [Tbl_FMCOA_AccountGroup].AllotmentClassID = [Tbl_FMCOA_AllotmentClass].AllotmentClassID and [Tbl_FMCOA_AllotmentClass].RevID = [Tbl_FMCOA_RevisionYear].RevID";

            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_FMAccounts]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        model.getMajorAccntGrpList.Add(new MajorAccountGroupList()
                        {
                            MAGID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            RevYear = GlobalFunction.ReturnEmptyString(dr[1]),
                            AllotmentClassTitle = GlobalFunction.ReturnEmptyString(dr[2]),
                            AGTitle = GlobalFunction.ReturnEmptyString(dr[3]),
                            MAGTitle = GlobalFunction.ReturnEmptyString(dr[4]),
                            MAGAccountCode = GlobalFunction.ReturnEmptyString(dr[5])
                        });
                    }
                }
                Connection.Close();
            }
            return PartialView("MajorAccountGroup/_TableMAG", model);
        }
        public ActionResult GetAddMajorAccntGrp()
        {
            FMMainChartofAccountModel model = new FMMainChartofAccountModel();
            return PartialView("MajorAccountGroup/_AddMAG", model);
        }
        //public ActionResult GetDynamicRevYearMAG()
        //{
        //    FMMainChartofAccountModel model = new FMMainChartofAccountModel();
        //    model.AllotClassModel.RevYearDropDownList = new SelectList((from s in BOSSDB.Tbl_FMCOA_RevisionYear.Where(a => a.isUsed == true).ToList() select new { RevID = s.RevID, RevYear = s.RevYear }), "RevID", "RevYear");

        //    return PartialView("DynamicFields/_DynamicRevYear", model);
        //}
        //public ActionResult GetDynamicAllotClassMAG(int RevID)
        //{
        //    FMMainChartofAccountModel model = new FMMainChartofAccountModel();
        //    model.AccountGrpModel.AllotClasslist = new SelectList((from s in BOSSDB.Tbl_FMCOA_AllotmentClass.Where(a => a.RevID == RevID).ToList() select new { AllotmentClassID = s.AllotmentClassID, AllotmentClassTitle = s.AllotmentClassTitle }), "AllotmentClassID", "AllotmentClassTitle");

        //    return PartialView("DynamicFields/_DynamicAllotmentClass", model);
        //}
        public ActionResult GetDynamicAccountGroup(int AllotmentClassID)
        {
            FMMainChartofAccountModel model = new FMMainChartofAccountModel();
            model.MajorAccountGrpModel.AccountGrpList = new SelectList((from s in BOSSDB.Tbl_FMCOA_AccountGroup.Where(a => a.AllotmentClassID == AllotmentClassID).ToList() select new { AGID = s.AGID, AGTitle = s.AGTitle }), "AGID", "AGTitle");
            return PartialView("DynamicFields/_DynamicAccountGroup", model);
        }
        public ActionResult GetAccountGroupCode(int AllotmentClassID)
        {
            FMMainChartofAccountModel model = new FMMainChartofAccountModel();
            var AccntGrpTbl = (from e in BOSSDB.Tbl_FMCOA_AccountGroup where e.AllotmentClassID == AllotmentClassID select e).FirstOrDefault();
          
            var AGIDvalue = "0";
            if (AccntGrpTbl != null)
            {
                AGIDvalue = AccntGrpTbl.AGCode;
            }
            model.AccountGrpModel.AGCode = AGIDvalue;
            return PartialView("DynamicFields/_DynamicAccountGroupCode", model);
        }
        public ActionResult GetAccountGroupCode2(int AGID)
        {
            FMMainChartofAccountModel model = new FMMainChartofAccountModel();
            var AccntGrpTbl = (from e in BOSSDB.Tbl_FMCOA_AccountGroup where e.AGID == AGID select e).FirstOrDefault();

            var AGIDvalue = "0";
            if (AccntGrpTbl != null)
            {
                AGIDvalue = AccntGrpTbl.AGCode;
            }
            model.AccountGrpModel.AGCode = AGIDvalue;
            return PartialView("DynamicFields/_DynamicAccountGroupCode", model);
        }
        public ActionResult AddNewMajorAccntGrp(FMMainChartofAccountModel model)
        {
            Tbl_FMCOA_MajorAccountGroup MajorAccntGrpTbl = new Tbl_FMCOA_MajorAccountGroup();

            MajorAccntGrpTbl.MAGTitle = GlobalFunction.ReturnEmptyString(model.MajorAccountGrpModel.getMajorAccntGrpColumns.MAGTitle);
            MajorAccntGrpTbl.MAGCode = GlobalFunction.ReturnEmptyString(model.MajorAccountGrpModel.getMajorAccntGrpColumns.MAGCode);
            MajorAccntGrpTbl.AGID = GlobalFunction.ReturnEmptyInt(model.MajorAccountGrpModel.AGID);

            BOSSDB.Tbl_FMCOA_MajorAccountGroup.Add(MajorAccntGrpTbl);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Get_UpdateMajorAccountGroup(FMMainChartofAccountModel model, int MAGID)
        {
            Tbl_FMCOA_MajorAccountGroup MjrAccntGrpTBL = (from e in BOSSDB.Tbl_FMCOA_MajorAccountGroup where e.MAGID == MAGID select e).FirstOrDefault();

            model.MajorAccountGrpModel.MAGID = MAGID;
            model.MajorAccountGrpModel.getMajorAccntGrpColumns.MAGTitle = MjrAccntGrpTBL.MAGTitle;
            model.MajorAccountGrpModel.getMajorAccntGrpColumns.MAGCode = MjrAccntGrpTBL.MAGCode;
            model.MajorAccountGrpModel.AGIDMag = Convert.ToInt32(MjrAccntGrpTBL.AGID);
            model.MajorAccountGrpModel.allotclasssTempID = Convert.ToInt32(MjrAccntGrpTBL.Tbl_FMCOA_AccountGroup.Tbl_FMCOA_AllotmentClass.AllotmentClassID);
            model.MajorAccountGrpModel.RevIDMAG = Convert.ToInt32(MjrAccntGrpTBL.Tbl_FMCOA_AccountGroup.Tbl_FMCOA_AllotmentClass.Tbl_FMCOA_RevisionYear.RevID);
            return PartialView("MajorAccountGroup/_UpdateMAG", model);
        }
        public ActionResult GetDynamicRevYearUpdateMAG(int RevIDMAG)
        {
            FMMainChartofAccountModel model = new FMMainChartofAccountModel();
            model.AllotClassModel.RevYearDropDownList = new SelectList((from s in BOSSDB.Tbl_FMCOA_RevisionYear.Where(a => a.isUsed == true || a.RevID == RevIDMAG).ToList() select new { RevID = s.RevID, RevYear = s.RevYear }), "RevID", "RevYear");
            model.AllotClassModel.RevID = RevIDMAG;
            return PartialView("DynamicFields/_DynamicRevYear", model);
        }
        public ActionResult GetDynamicAllotClassUpdateMAG(FMMainChartofAccountModel model, int RevIDMAG, int allotclasssTempID)
        {
            model.AccountGrpModel.AllotClasslist = new SelectList((from s in BOSSDB.Tbl_FMCOA_AllotmentClass.Where(a => a.RevID == RevIDMAG).ToList() select new { AllotmentClassID = s.AllotmentClassID, AllotmentClassTitle = s.AllotmentClassTitle }), "AllotmentClassID", "AllotmentClassTitle");
            model.AccountGrpModel.AllotmentClassID = allotclasssTempID;
            return PartialView("DynamicFields/_DynamicAllotmentClass", model);
        }
        public ActionResult GetDynamicAccntGroupUpdateMAG(FMMainChartofAccountModel model, int allotclasssTempID, int AGIDMag)
        {
            model.MajorAccountGrpModel.AccountGrpList = new SelectList((from s in BOSSDB.Tbl_FMCOA_AccountGroup.Where(a => a.AllotmentClassID == allotclasssTempID).ToList() select new { AGID = s.AGID, AGTitle = s.AGTitle }), "AGID", "AGTitle");
            model.MajorAccountGrpModel.AGID = AGIDMag;
            return PartialView("DynamicFields/_DynamicAccountGroup", model);
        }
        public ActionResult UpdateMajorAccountGroup(FMMainChartofAccountModel model)
        {
            Tbl_FMCOA_MajorAccountGroup Majoraccntgrp = (from e in BOSSDB.Tbl_FMCOA_MajorAccountGroup where e.MAGID == model.MajorAccountGrpModel.MAGID select e).FirstOrDefault();
            Majoraccntgrp.MAGTitle = GlobalFunction.ReturnEmptyString(model.MajorAccountGrpModel.getMajorAccntGrpColumns.MAGTitle);
            Majoraccntgrp.MAGCode = GlobalFunction.ReturnEmptyString(model.MajorAccountGrpModel.getMajorAccntGrpColumns.MAGCode);
            Majoraccntgrp.AGID = GlobalFunction.ReturnEmptyInt(model.MajorAccountGrpModel.AGID);

            BOSSDB.Entry(Majoraccntgrp);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteMjorAccountGroup(FMMainChartofAccountModel model, int MAGID)
        {
            Tbl_FMCOA_MajorAccountGroup MjorAccntgrptbl = (from e in BOSSDB.Tbl_FMCOA_MajorAccountGroup where e.MAGID == MAGID select e).FirstOrDefault();
            BOSSDB.Tbl_FMCOA_MajorAccountGroup.Remove(MjorAccntgrptbl);
            BOSSDB.SaveChanges();
            return RedirectToAction("FileAccounts");
        }
        //--------------------------------------------------------------------------------------------------------------------
        //Sub Major Account Group Tab
        //--------------------------------------------------------------------------------------------------------------------
        public ActionResult GetSubMajorAccntGrpDT()
        {
            SubMajorAccountGroupModel model = new SubMajorAccountGroupModel();
            List<SubMajorAccountGroupList> getSubMajorAccntGrpList = new List<SubMajorAccountGroupList>();
            var SQLQuery = "";

            SQLQuery = "SELECT [SMAGID],[RevYear],[AllotmentClassTitle],[AGTitle],[MAGTitle],[SMAGTitle],[AGCode]+' - '+[MAGCode]+' - '+[SMAGCode] as SMAGAccountCode FROM [BOSS].[dbo].[Tbl_FMCOA_SubMajorAccountGroup] ,[Tbl_FMCOA_MajorAccountGroup],[Tbl_FMCOA_AccountGroup],[Tbl_FMCOA_RevisionYear],[Tbl_FMCOA_AllotmentClass] where [Tbl_FMCOA_SubMajorAccountGroup].MAGID = [Tbl_FMCOA_MajorAccountGroup].MAGID and [Tbl_FMCOA_MajorAccountGroup].AGID = [Tbl_FMCOA_AccountGroup].AGID and [Tbl_FMCOA_AccountGroup].AllotmentClassID = [Tbl_FMCOA_AllotmentClass].AllotmentClassID and [Tbl_FMCOA_AllotmentClass].RevID = [Tbl_FMCOA_RevisionYear].RevID";

            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_FMAccounts]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        getSubMajorAccntGrpList.Add(new SubMajorAccountGroupList()
                        {
                            SMAGID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            RevYear = GlobalFunction.ReturnEmptyString(dr[1]),
                            AllotmentClassTitle =GlobalFunction.ReturnEmptyString(dr[2]),
                            AGTitle = GlobalFunction.ReturnEmptyString(dr[3]),
                            MAGTitle = GlobalFunction.ReturnEmptyString(dr[4]),
                            SMAGTitle = GlobalFunction.ReturnEmptyString(dr[5]),
                            SMAGAccountCode = GlobalFunction.ReturnEmptyString(dr[6])
                        });
                    }
                }
                Connection.Close();
            }
            model.getSubMajorAccntGrpList = getSubMajorAccntGrpList.ToList();
            return PartialView("SubMajorAccountGroup/_TableSMAG", getSubMajorAccntGrpList);
        }
        public ActionResult GetAddSubMajorAccntGrp()
        {
            FMMainChartofAccountModel model = new FMMainChartofAccountModel();
            return PartialView("SubMajorAccountGroup/_AddSMAG", model);
        }
        //Get Dynamic Major Account Group
        public ActionResult GetDynamicMajorAccountGroup(string AGID) 
        {
            FMMainChartofAccountModel model = new FMMainChartofAccountModel();
            if (AGID != null && AGID !="")
            {
                var AGIDT = Convert.ToInt32(AGID);
                model.SubMajorAccountGrpModel.MajorAccountGrpList = new SelectList((from s in BOSSDB.Tbl_FMCOA_MajorAccountGroup.Where(a => a.AGID == AGIDT).ToList() select new { MAGID = s.MAGID, MAGTitle = s.MAGTitle }), "MAGID", "MAGTitle");
            }
            else
            {
                model.SubMajorAccountGrpModel.MajorAccountGrpList = new SelectList((from s in BOSSDB.Tbl_FMCOA_MajorAccountGroup.Where(a => a.AGID == null).ToList() select new { MAGID = s.MAGID, MAGTitle = s.MAGTitle }), "MAGID", "MAGTitle");
            }
            return PartialView("DynamicFields/_DynamicMajorAccountGroup", model);
        }
        public ActionResult GetMajorAccountGroupCode(string AGID)
        { 
            FMMainChartofAccountModel model = new FMMainChartofAccountModel();
            var MAGIDvalue = "00";
            if (AGID != null && AGID != "")
            {
                var AGIDT = Convert.ToInt32(AGID);
                var MajorAccntGrpTbl = (from e in BOSSDB.Tbl_FMCOA_MajorAccountGroup where e.AGID == AGIDT select e).FirstOrDefault();
              
                MAGIDvalue = MajorAccntGrpTbl.MAGCode;
            }
            model.MajorAccountGrpModel.MAGCode = MAGIDvalue;
            return PartialView("DynamicFields/_DynamicMajorAccountGroupCode", model);
        }
        public ActionResult GetMajorAccountGroupCode2(int MAGID)
        {
            FMMainChartofAccountModel model = new FMMainChartofAccountModel();
            var MajorAccntGrpTbl = (from e in BOSSDB.Tbl_FMCOA_MajorAccountGroup where e.MAGID == MAGID select e).FirstOrDefault();

            var MAGIDvalue = "00";
            if (MajorAccntGrpTbl != null)
            {
                MAGIDvalue = MajorAccntGrpTbl.MAGCode;
            }
            model.MajorAccountGrpModel.MAGCode = MAGIDvalue;
            return PartialView("DynamicFields/_DynamicMajorAccountGroupCode", model);
        }
        public ActionResult AddNewSubMajorAccntGrp(FMMainChartofAccountModel model)
        {
            Tbl_FMCOA_SubMajorAccountGroup subMajorAccntGrpTbl = new Tbl_FMCOA_SubMajorAccountGroup();

            subMajorAccntGrpTbl.SMAGTitle = GlobalFunction.ReturnEmptyString(model.SubMajorAccountGrpModel.getSubMajorAccntGrpColumns.SMAGTitle);
            subMajorAccntGrpTbl.SMAGCode = GlobalFunction.ReturnEmptyString(model.SubMajorAccountGrpModel.getSubMajorAccntGrpColumns.SMAGCode);
            subMajorAccntGrpTbl.MAGID = GlobalFunction.ReturnEmptyInt(model.SubMajorAccountGrpModel.MAGID);

            BOSSDB.Tbl_FMCOA_SubMajorAccountGroup.Add(subMajorAccntGrpTbl);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Get_UpdateSubMajorAccountGroup(FMMainChartofAccountModel model, int SMAGID)
        {
            Tbl_FMCOA_SubMajorAccountGroup subMjrAccntGrpTBL = (from e in BOSSDB.Tbl_FMCOA_SubMajorAccountGroup where e.SMAGID == SMAGID select e).FirstOrDefault();

            model.SubMajorAccountGrpModel.SMAGID = SMAGID;
            model.SubMajorAccountGrpModel.getSubMajorAccntGrpColumns.SMAGTitle = subMjrAccntGrpTBL.SMAGTitle;
            model.SubMajorAccountGrpModel.getSubMajorAccntGrpColumns.SMAGCode = subMjrAccntGrpTBL.SMAGCode;
            model.SubMajorAccountGrpModel.MAGIDSMag = Convert.ToInt32(subMjrAccntGrpTBL.MAGID);
            model.SubMajorAccountGrpModel.AGIDSMag = Convert.ToInt32(subMjrAccntGrpTBL.Tbl_FMCOA_MajorAccountGroup.Tbl_FMCOA_AccountGroup.AGID);
            model.SubMajorAccountGrpModel.allotclasssTempIDSMAG = Convert.ToInt32(subMjrAccntGrpTBL.Tbl_FMCOA_MajorAccountGroup.Tbl_FMCOA_AccountGroup.Tbl_FMCOA_AllotmentClass.AllotmentClassID);
            model.SubMajorAccountGrpModel.RevIDSMAG = Convert.ToInt32(subMjrAccntGrpTBL.Tbl_FMCOA_MajorAccountGroup.Tbl_FMCOA_AccountGroup.Tbl_FMCOA_AllotmentClass.Tbl_FMCOA_RevisionYear.RevID);
            
            return PartialView("SubMajorAccountGroup/_UpdateSMAG", model);
        }
        public ActionResult GetDynamicRevYearUpdateSMAG(int RevIDSMAG)
        {
            FMMainChartofAccountModel model = new FMMainChartofAccountModel();
            model.AllotClassModel.RevYearDropDownList = new SelectList((from s in BOSSDB.Tbl_FMCOA_RevisionYear.Where(a => a.isUsed == true || a.RevID == RevIDSMAG).ToList() select new { RevID = s.RevID, RevYear = s.RevYear }), "RevID", "RevYear");
            model.AllotClassModel.RevID = RevIDSMAG;
            return PartialView("DynamicFields/_DynamicRevYear", model);
        }
        public ActionResult GetDynamicAllotClassUpdateSMAG(FMMainChartofAccountModel model, int RevIDSMAG , int allotclasssTempIDSMAG)
        {
            model.AccountGrpModel.AllotClasslist = new SelectList((from s in BOSSDB.Tbl_FMCOA_AllotmentClass.Where(a => a.RevID == RevIDSMAG).ToList() select new { AllotmentClassID = s.AllotmentClassID, AllotmentClassTitle = s.AllotmentClassTitle }), "AllotmentClassID", "AllotmentClassTitle");
            model.AccountGrpModel.AllotmentClassID = allotclasssTempIDSMAG;
            return PartialView("DynamicFields/_DynamicAllotmentClass", model);
        }
        public ActionResult GetDynamicAccntGroupUpdateSMAG(FMMainChartofAccountModel model, int allotclasssTempIDSMAG , int AGIDSMag) 
        {
            model.MajorAccountGrpModel.AccountGrpList = new SelectList((from s in BOSSDB.Tbl_FMCOA_AccountGroup.Where(a => a.AllotmentClassID == allotclasssTempIDSMAG).ToList() select new { AGID = s.AGID, AGTitle = s.AGTitle }), "AGID", "AGTitle");
            model.MajorAccountGrpModel.AGID = AGIDSMag;
            return PartialView("DynamicFields/_DynamicAccountGroup", model);
        }
        public ActionResult GetDynamicMajorAccntGroupUpdateMAG(FMMainChartofAccountModel model, int AGIDSMag, int MAGIDSMag)  
        {
            model.SubMajorAccountGrpModel.MajorAccountGrpList = new SelectList((from s in BOSSDB.Tbl_FMCOA_MajorAccountGroup.Where(a => a.AGID == AGIDSMag).ToList() select new { MAGID = s.MAGID, MAGTitle = s.MAGTitle }), "MAGID", "MAGTitle");
            model.SubMajorAccountGrpModel.MAGID = MAGIDSMag;
            return PartialView("DynamicFields/_DynamicMajorAccountGroup", model);
        }
        public ActionResult UpdateSubMajorAccountGroup(FMMainChartofAccountModel model)
        {
            Tbl_FMCOA_SubMajorAccountGroup subMajoraccntgrp = (from e in BOSSDB.Tbl_FMCOA_SubMajorAccountGroup where e.SMAGID == model.SubMajorAccountGrpModel.SMAGID select e).FirstOrDefault();
            subMajoraccntgrp.SMAGTitle = GlobalFunction.ReturnEmptyString(model.SubMajorAccountGrpModel.getSubMajorAccntGrpColumns.SMAGTitle);
            subMajoraccntgrp.SMAGCode = GlobalFunction.ReturnEmptyString(model.SubMajorAccountGrpModel.getSubMajorAccntGrpColumns.SMAGCode);
            subMajoraccntgrp.MAGID = GlobalFunction.ReturnEmptyInt(model.SubMajorAccountGrpModel.MAGID);
            
            BOSSDB.Entry(subMajoraccntgrp);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteSubMajorAccountGroup(FMMainChartofAccountModel model, int SMAGID)
        {
            Tbl_FMCOA_SubMajorAccountGroup subMjorAccntgrptbl = (from e in BOSSDB.Tbl_FMCOA_SubMajorAccountGroup where e.SMAGID == SMAGID select e).FirstOrDefault();
            BOSSDB.Tbl_FMCOA_SubMajorAccountGroup.Remove(subMjorAccntgrptbl);
            BOSSDB.SaveChanges();
            return RedirectToAction("FileAccounts");
        }
        //--------------------------------------------------------------------------------------------------------------------
        //General Account Tab
        //--------------------------------------------------------------------------------------------------------------------
        public ActionResult GetGeneralAccountDT()
        {
            //MajorAccountGroupModel model = new MajorAccountGroupModel();
            //var SQLQuery = "SELECT [MAGID],[RevYear],[AllotmentClassTitle],[AGTitle],[MAGTitle],[AGCode]+' - '+[MAGCode] as MAGAccountCode FROM [BOSS].[dbo].[Tbl_FMCOA_MajorAccountGroup],[Tbl_FMCOA_AccountGroup],[Tbl_FMCOA_RevisionYear],[Tbl_FMCOA_AllotmentClass] where [Tbl_FMCOA_MajorAccountGroup].AGID = [Tbl_FMCOA_AccountGroup].AGID and [Tbl_FMCOA_AccountGroup].AllotmentClassID = [Tbl_FMCOA_AllotmentClass].AllotmentClassID and [Tbl_FMCOA_AllotmentClass].RevID = [Tbl_FMCOA_RevisionYear].RevID";

            //using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            //{
            //    Connection.Open();
            //    using (SqlCommand command = new SqlCommand("[dbo].[SP_FMAccounts]", Connection))
            //    {
            //        command.CommandType = CommandType.StoredProcedure;
            //        command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
            //        SqlDataReader dr = command.ExecuteReader();
            //        while (dr.Read())
            //        {
            //            model.getMajorAccntGrpList.Add(new MajorAccountGroupList()
            //            {
            //                MAGID = GlobalFunction.ReturnEmptyInt(dr[0]),
            //                RevYear = GlobalFunction.ReturnEmptyString(dr[1]),
            //                AllotmentClassTitle = GlobalFunction.ReturnEmptyString(dr[2]),
            //                AGTitle = GlobalFunction.ReturnEmptyString(dr[3]),
            //                MAGTitle = GlobalFunction.ReturnEmptyString(dr[4]),
            //                MAGAccountCode = GlobalFunction.ReturnEmptyString(dr[5])
            //            });
            //        }
            //    }
            //    Connection.Close();
            //}
            return PartialView("GeneralAccount/_TableGeneralAccount"/*, model*/);
        }
        public ActionResult GetAddGeneralAccount()
        {
            FMMainChartofAccountModel model = new FMMainChartofAccountModel();
            return PartialView("GeneralAccount/_AddGeneralAccount", model);
        }
        //Get Dynamic Sub Major Account Group
        public ActionResult GetDynamicSubMajorAccountGroup(string MAGID)
        {
            FMMainChartofAccountModel model = new FMMainChartofAccountModel();
            if (MAGID != null && MAGID != "")
            {
                var MAGIDT = Convert.ToInt32(MAGID);
                model.GeneralAccntModel.SubMajorAccountGrpList = new SelectList((from s in BOSSDB.Tbl_FMCOA_SubMajorAccountGroup.Where(a => a.MAGID == MAGIDT).ToList() select new { SMAGID = s.SMAGID, SMAGTitle = s.SMAGTitle }), "SMAGID", "SMAGTitle");
            }
            else
            {
                model.GeneralAccntModel.SubMajorAccountGrpList = new SelectList((from s in BOSSDB.Tbl_FMCOA_SubMajorAccountGroup.Where(a => a.MAGID == null).ToList() select new { SMAGID = s.SMAGID, SMAGTitle = s.SMAGTitle }), "SMAGID", "SMAGTitle");
            }
            return PartialView("DynamicFields/_DynamicSubMajorAccountGroup", model);
        }
        public ActionResult GetSubMajorAccountGroupCode(string MAGID)
        {
            FMMainChartofAccountModel model = new FMMainChartofAccountModel();
            var SMAGIDvalue = "00";
            if (MAGID != null && MAGID != "")
            {
                var MAGIDT = Convert.ToInt32(MAGID);
                var SubMajorAccntGrpTbl = (from e in BOSSDB.Tbl_FMCOA_SubMajorAccountGroup where e.MAGID == MAGIDT select e).FirstOrDefault();

                SMAGIDvalue = SubMajorAccntGrpTbl.SMAGCode;
            }
            model.SubMajorAccountGrpModel.SMAGCode = SMAGIDvalue;
            return PartialView("DynamicFields/_DynamicSubMajorAccountGroupCode", model);
        }
        public ActionResult GetSubMajorAccountGroupCode2(int SMAGID)
        {
            FMMainChartofAccountModel model = new FMMainChartofAccountModel();
            var SubMajorAccntGrpTbl = (from e in BOSSDB.Tbl_FMCOA_SubMajorAccountGroup where e.SMAGID == SMAGID select e).FirstOrDefault();

            var SMAGIDvalue = "00";
            if (SubMajorAccntGrpTbl != null)
            {
                SMAGIDvalue = SubMajorAccntGrpTbl.SMAGCode;
            }
            model.SubMajorAccountGrpModel.SMAGCode = SMAGIDvalue;
            return PartialView("DynamicFields/_DynamicSubMajorAccountGroupCode", model);
        }
        public ActionResult GetDyanmicGeneralLedgerAccountnoselect()
        {
            FMMainChartofAccountModel model = new FMMainChartofAccountModel();
            model.GeneralAccntModel.GeneralAccountList = new SelectList((from s in BOSSDB.Tbl_FMCOA_GeneralAccount select new { GAID = s.GAID, GATitle = s.GATitle }), "GAID", "GATitle");
            return PartialView("GeneralAccount/_DynamicGeneralLedgerAccount", model);
        }
        public ActionResult GetDyanmicGeneralLedgerAccount(int isSelected)
        {
            FMMainChartofAccountModel model = new FMMainChartofAccountModel();
            if (isSelected == 1)
            {
                model.GeneralAccntModel.GeneralAccountList = new SelectList((from s in BOSSDB.Tbl_FMCOA_GeneralAccount.Where(a => a.isContraAccount != true).ToList() select new { GAID = s.GAID, GATitle = s.GATitle }), "GAID", "GATitle");
            }
            else if (isSelected == 2)
            {
                model.GeneralAccntModel.GeneralAccountList = new SelectList((from s in BOSSDB.Tbl_FMCOA_GeneralAccount.Where(a => a.isSubAccount != true).ToList() select new { GAID = s.GAID, GATitle = s.GATitle }), "GAID", "GATitle");
            } 
            return PartialView("GeneralAccount/_DynamicGeneralLedgerAccount", model);
        }
















































        ////Get General Account Partial View
        //public ActionResult GetGeneralAccountView()
        //{
        //    GeneralAccountModel model = new GeneralAccountModel();

        //    return PartialView("ChartOfAccounts/GeneralAccount/GeneralAccountTabIndex", model);
        //}
        //public ActionResult GetAddGeneralAccountOreig()
        //{
        //    GeneralAccountModel model = new GeneralAccountModel();

        //    return PartialView("ChartOfAccounts/GeneralAccount/_AddGeneralAccount", model);
        //}
        //public ActionResult GetAddGeneralAccountforUpdate(int AllotmentID)
        //{
        //    GeneralAccountModel model = new GeneralAccountModel();
        //    model.AllotmentID = AllotmentID;
        //    return PartialView("ChartOfAccounts/GeneralAccount/_AddGeneralAccount", model);
        //}
        //public ActionResult GetDynamicAllotmentClass()
        //{
        //    GeneralAccountModel model = new GeneralAccountModel();
        //    return PartialView("ChartOfAccounts/GeneralAccount/_DynamicAllotmentClass", model);
        //}
        //public ActionResult GetDynamicAllotmentClassForUpdate(int AllotmentID)
        //{
        //    GeneralAccountModel model = new GeneralAccountModel();
        //    model.AllotmentID = AllotmentID;
        //    return PartialView("ChartOfAccounts/GeneralAccount/_DynamicAllotmentClassForUpdate", model);
        //}
        //public ActionResult GetGeneralAccountDT0rig(int AllotmentID)
        //{
        //    GeneralAccountModel model = new GeneralAccountModel();
        //    List<GeneralAccountList> getGenAcctList = new List<GeneralAccountList>();
        //    var SQLQuery = "";
        //    SQLQuery = "SELECT [GenAccountID] ,[GenAccountCode], [GenAccountTitle] ,[isReserve] ,[ReservePercent] ,[isFullRelease] ,[isContinuing] ,[isOBRCashAdvance] ,[Tbl_FMCOA_GeneralAccount].[AllotmentID] FROM [dbo].[Tbl_FMCOA_GeneralAccount], [dbo].[AllotmentClass] WHERE [Tbl_FMCOA_GeneralAccount].AllotmentID = [AllotmentClass].AllotmentID and[Tbl_FMCOA_GeneralAccount].AllotmentID =" + AllotmentID;

        //    using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
        //    {
        //        Connection.Open();
        //        using (SqlCommand command = new SqlCommand("[dbo].[SP_FMAccounts]", Connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
        //            SqlDataReader dr = command.ExecuteReader();
        //            while (dr.Read())
        //            {
        //                getGenAcctList.Add(new GeneralAccountList()
        //                {
        //                    GenAccountID = GlobalFunction.ReturnEmptyInt(dr[0]),
        //                    GenAccountCode = GlobalFunction.ReturnEmptyString(dr[1]),
        //                    GenAccountTitle = GlobalFunction.ReturnEmptyString(dr[2]),
        //                    isReserve = GlobalFunction.ReturnEmptyInt(dr[3]),
        //                    ReservePercent = GlobalFunction.ReturnEmptyInt(dr[4]),
        //                    isFullRelease = GlobalFunction.ReturnEmptyInt(dr[5]),
        //                    isContinuing = GlobalFunction.ReturnEmptyInt(dr[6]),
        //                    isOBRCashAdvance = GlobalFunction.ReturnEmptyInt(dr[7])
        //                });
        //            }
        //        }
        //        Connection.Close();
        //    }
        //    model.getGenAcctList = getGenAcctList.ToList();
        //    return PartialView("ChartOfAccounts/GeneralAccount/_TableGeneralAccount", getGenAcctList);
        //}
        //public ActionResult AddNewGenAcct(GeneralAccountModel model)
        //{
        //    Tbl_FMCOA_GeneralAccount GenAcctTable = new Tbl_FMCOA_GeneralAccount();

        //    GenAcctTable.GenAccountTitle = GlobalFunction.ReturnEmptyString(model.getGenAcctColumns.GenAccountTitle);
        //    GenAcctTable.GenAccountCode = GlobalFunction.ReturnEmptyString(model.getGenAcctColumns.GenAccountCode);
        //    GenAcctTable.isReserve = model.isReserve;
        //    GenAcctTable.ReservePercent = GlobalFunction.ReturnEmptyInt(model.getGenAcctColumns.ReservePercent);
        //    GenAcctTable.isFullRelease = model.isFullRelease;
        //    GenAcctTable.isContinuing = model.isContinuing;
        //    GenAcctTable.isOBRCashAdvance = model.isOBRCashAdvance;
        //    GenAcctTable.AllotmentID = GlobalFunction.ReturnEmptyInt(model.AllotmentID);

        //    BOSSDB.Tbl_FMCOA_GeneralAccount.Add(GenAcctTable);
        //    BOSSDB.SaveChanges();

        //    var result = "";
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult Get_UpdateGenAcct(GeneralAccountModel model, int GenAccountID)
        //{
        //    Tbl_FMCOA_GeneralAccount GenAcctTable = (from e in BOSSDB.Tbl_FMCOA_GeneralAccount where e.GenAccountID == GenAccountID select e).FirstOrDefault();

        //    model.getGenAcctColumns2.GenAccountTitle = GenAcctTable.GenAccountTitle;
        //    model.getGenAcctColumns2.GenAccountCode = GenAcctTable.GenAccountCode;
        //    model.isReserve = Convert.ToBoolean(GenAcctTable.isReserve);
        //    model.getGenAcctColumns2.ReservePercent = GenAcctTable.ReservePercent;
        //    model.isFullRelease = Convert.ToBoolean(GenAcctTable.isFullRelease);
        //    model.isContinuing = Convert.ToBoolean(GenAcctTable.isContinuing);
        //    model.isOBRCashAdvance = Convert.ToBoolean(GenAcctTable.isOBRCashAdvance);
        //    model.allotTempID = Convert.ToInt32(GenAcctTable.AllotmentID);
        //    model.GenAccountID = GenAccountID;
        //    return PartialView("ChartOfAccounts/GeneralAccount/_UpdateGeneralAccount", model);
        //}
        //public ActionResult UpdateGenAcct(GeneralAccountModel model)
        //{
        //    Tbl_FMCOA_GeneralAccount genacctTBL = (from e in BOSSDB.Tbl_FMCOA_GeneralAccount where e.GenAccountID == model.GenAccountID select e).FirstOrDefault();
        //    genacctTBL.GenAccountTitle = GlobalFunction.ReturnEmptyString(model.getGenAcctColumns2.GenAccountTitle);
        //    genacctTBL.GenAccountCode = GlobalFunction.ReturnEmptyString(model.getGenAcctColumns2.GenAccountCode);
        //    genacctTBL.isReserve = model.isReserve;
        //    genacctTBL.ReservePercent = GlobalFunction.ReturnEmptyInt(model.getGenAcctColumns2.ReservePercent);
        //    genacctTBL.isFullRelease = model.isFullRelease;
        //    genacctTBL.isContinuing = model.isContinuing;
        //    genacctTBL.isOBRCashAdvance = model.isOBRCashAdvance;
        //    genacctTBL.AllotmentID = GlobalFunction.ReturnEmptyInt(model.AllotmentID);

        //    BOSSDB.Entry(genacctTBL);
        //    BOSSDB.SaveChanges();

        //    var result = "";
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult DeleteGenAcct(GeneralAccountModel model, int GenAccountID)
        //{
        //    Tbl_FMCOA_GeneralAccount genacctTBL = (from e in BOSSDB.Tbl_FMCOA_GeneralAccount where e.GenAccountID == GenAccountID select e).FirstOrDefault();
        //    BOSSDB.Tbl_FMCOA_GeneralAccount.Remove(genacctTBL);
        //    BOSSDB.SaveChanges();
        //    return RedirectToAction("FileAccounts");
        //}

        //public ActionResult GetChangeAcctDetailsModal()
        //{
        //    return PartialView("ChartOfAccounts/GeneralAccount/_UpdateGeneralAccount");
        //}
        //=======================
        //Get Sub Account Partial View
        //public ActionResult GetSubAccountView()
        //{
        //    SubAccountsModel model = new SubAccountsModel();
        //    return PartialView("ChartOfAccounts/SubAccount/SubAccountTabIndex", model);
        //}
        //public ActionResult GetDynamicGenAccnt(int AllotmentID)
        //{
        //    SubAccountsModel model = new SubAccountsModel();
        //    model.GeneralAccntList = new SelectList((from s in BOSSDB.Tbl_FMCOA_GeneralAccount.ToList() where s.AllotmentID == AllotmentID select new { GenAccountID = s.GenAccountID, GenAccountTitle =  s.GenAccountCode + " - " + s.GenAccountTitle   }), "GenAccountID", "GenAccountTitle");
        //    return PartialView("ChartOfAccounts/SubAccount/_DynamicGeneralAccount", model);
        //}
        //public ActionResult GetSubAccountDT(int AllotmentID, int GenAccountID)
        //{
        //    SubAccountsModel model = new SubAccountsModel();
        //    List<SubAccountList> getSubAcctList = new List<SubAccountList>();
        //    var SQLQuery = "SELECT [SubAccountID] ,[SubAccountCode] ,[SubAccountTitle] ,[Tbl_FMSubAccount].[isReserve] ,[Tbl_FMSubAccount].[ReservePercent] ,[Tbl_FMSubAccount].[isFullRelease] ,[Tbl_FMSubAccount].[isContinuing] ,[Tbl_FMSubAccount].[isOBRCashAdvance] ,[Tbl_FMSubAccount].[GenAccountID],(Tbl_FMCOA_GeneralAccount.GenAccountCode + ' - ' + [SubAccountCode]) as GenCode FROM[BOSS].[dbo].[Tbl_FMSubAccount], [BOSS].[dbo].AllotmentClass,Tbl_FMCOA_GeneralAccount where[dbo].[Tbl_FMSubAccount].AllotmentID = [dbo].AllotmentClass.AllotmentID and Tbl_FMCOA_GeneralAccount.GenAccountID=[Tbl_FMSubAccount].GenAccountID and [Tbl_FMSubAccount].AllotmentID = " + AllotmentID + " and[Tbl_FMSubAccount].GenAccountID = " + GenAccountID;

        //    using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
        //    {
        //        Connection.Open();
        //        using (SqlCommand command = new SqlCommand("[dbo].[SP_FMAccounts]", Connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
        //            SqlDataReader dr = command.ExecuteReader();
        //            while (dr.Read())
        //            {
        //                getSubAcctList.Add(new SubAccountList()
        //                {
        //                    SubAccountID = GlobalFunction.ReturnEmptyInt(dr[0]),
        //                    SubAccountCode = GlobalFunction.ReturnEmptyString(dr[9]),
        //                    SubAccountTitle = GlobalFunction.ReturnEmptyString(dr[2]),
        //                    isReserve = GlobalFunction.ReturnEmptyInt(dr[3]),
        //                    ReservePercent = GlobalFunction.ReturnEmptyInt(dr[4]),
        //                    isFullRelease = GlobalFunction.ReturnEmptyInt(dr[5]),
        //                    isContinuing = GlobalFunction.ReturnEmptyInt(dr[6]),
        //                    isOBRCashAdvance = GlobalFunction.ReturnEmptyInt(dr[7])
        //                });
        //            }
        //        }
        //        Connection.Close();
        //    }
        //    model.getSubAcctList = getSubAcctList.ToList();

        //    return PartialView("ChartOfAccounts/SubAccount/_TableSubAccount", model.getSubAcctList);
        //}
        //public ActionResult GetAddSubAccount(int AllotmentID, int GenAccountID)
        //{
        //    SubAccountsModel model = new SubAccountsModel();
        //    model.AllotmentID = AllotmentID;
        //    model.GenAccountID = GenAccountID;
        //    return PartialView("ChartOfAccounts/SubAccount/_AddSubAccount", model);
        //}
        //public ActionResult GetGenAccntCodeField(int GenAccountID)
        //{
        //    SubAccountsModel model = new SubAccountsModel();

        //    var SubAccntTable = (from e in BOSSDB.Tbl_FMCOA_GeneralAccount where e.GenAccountID == GenAccountID select e).FirstOrDefault();
        //    model.GenAccountCode = SubAccntTable.GenAccountCode;

        //    return PartialView("ChartOfAccounts/SubAccount/_GenAcctCodeView", model);
        //}
        //public ActionResult AddNewSubAccount(SubAccountsModel model)
        //{
        //    Tbl_FMSubAccount subAcctTable = new Tbl_FMSubAccount();

        //    subAcctTable.SubAccountCode =GlobalFunction.ReturnEmptyString(model.getSubAcctColumns.SubAccountCode);
        //    subAcctTable.SubAccountTitle = GlobalFunction.ReturnEmptyString(model.getSubAcctColumns.SubAccountTitle);
        //    subAcctTable.isReserve = model.isReserve;
        //    subAcctTable.ReservePercent = GlobalFunction.ReturnEmptyInt(model.getSubAcctColumns.ReservePercent);
        //    subAcctTable.isFullRelease = model.isFullRelease;
        //    subAcctTable.isContinuing = model.isContinuing;
        //    subAcctTable.isOBRCashAdvance = model.isOBRCashAdvance;
        //    subAcctTable.GenAccountID = GlobalFunction.ReturnEmptyInt(model.GenAccountID);
        //    subAcctTable.AllotmentID = GlobalFunction.ReturnEmptyInt(model.AllotmentID);
        //    BOSSDB.Tbl_FMSubAccount.Add(subAcctTable);
        //    BOSSDB.SaveChanges();
        //    var result = "";
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult Get_UpdateSubAcct(SubAccountsModel model, int SubAccountID)
        //{
        //    Tbl_FMSubAccount SubAcctTable = (from e in BOSSDB.Tbl_FMSubAccount where e.SubAccountID == SubAccountID select e).FirstOrDefault();

        //    model.getSubAcctColumns2.SubAccountTitle = SubAcctTable.SubAccountTitle;
        //    model.getSubAcctColumns2.SubAccountCode = SubAcctTable.SubAccountCode;
        //    model.isReserve = Convert.ToBoolean(SubAcctTable.isReserve);
        //    model.getSubAcctColumns2.ReservePercent = SubAcctTable.ReservePercent;
        //    model.isFullRelease = Convert.ToBoolean(SubAcctTable.isFullRelease);
        //    model.isContinuing = Convert.ToBoolean(SubAcctTable.isContinuing);
        //    model.isOBRCashAdvance = Convert.ToBoolean(SubAcctTable.isOBRCashAdvance);
        //    model.AllotmentID = Convert.ToInt32(SubAcctTable.AllotmentID);
        //    model.GenAccountID = Convert.ToInt32(SubAcctTable.GenAccountID);
        //    model.SubAccountID = SubAccountID;
        //    return PartialView("ChartOfAccounts/SubAccount/_UpdateSubAccount", model);
        //}
        //public ActionResult UpdateSubAcct(SubAccountsModel model)
        //{
        //    Tbl_FMSubAccount subacctTable = (from e in BOSSDB.Tbl_FMSubAccount where e.SubAccountID == model.SubAccountID select e).FirstOrDefault();
        //   subacctTable.SubAccountTitle = GlobalFunction.ReturnEmptyString(model.getSubAcctColumns2.SubAccountTitle);
        //    subacctTable.SubAccountCode = GlobalFunction.ReturnEmptyString(model.getSubAcctColumns2.SubAccountCode);
        //    subacctTable.isReserve = model.isReserve;
        //    subacctTable.ReservePercent = GlobalFunction.ReturnEmptyInt(model.getSubAcctColumns2.ReservePercent);
        //    subacctTable.isFullRelease = model.isFullRelease;
        //    subacctTable.isContinuing = model.isContinuing;
        //    subacctTable.isOBRCashAdvance = model.isOBRCashAdvance;
        //    subacctTable.AllotmentID = GlobalFunction.ReturnEmptyInt(model.AllotmentID);
        //    subacctTable.GenAccountID = GlobalFunction.ReturnEmptyInt(model.GenAccountID);

        //    BOSSDB.Entry(subacctTable);
        //    BOSSDB.SaveChanges();

        //    var result = "";
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult DeleteSubAcct(SubAccountsModel model, int SubAccountID)
        //{
        //    Tbl_FMSubAccount SubacctTBL = (from e in BOSSDB.Tbl_FMSubAccount where e.SubAccountID == SubAccountID select e).FirstOrDefault();
        //    BOSSDB.Tbl_FMSubAccount.Remove(SubacctTBL);
        //    BOSSDB.SaveChanges();
        //    return RedirectToAction("FileAccounts");
        //}
        //====================================

        //=========================================================
        //PIN
        //public ActionResult ValidatePin(string Pin_Acct)
        //{
        //    var pinStatus = 0;
        //    var pintemp = (from e in BOSSDB.PIN_Accounts where e.Pin_Acct == Pin_Acct select e).FirstOrDefault();
        //    if (pintemp != null)
        //    {
        //        pinStatus = 1;
        //    }

        //    return Json(pinStatus, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult UpdatePIN(GeneralAccountModel model)
        //{
        //    PIN_Accounts pin = (from e in BOSSDB.PIN_Accounts select e).FirstOrDefault();

        //    pin.Pin_Acct = GlobalFunction.ReturnEmptyString(model.confirm_newPIN);

        //    BOSSDB.Entry(pin);
        //    BOSSDB.SaveChanges();

        //    var result = "";
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
    }
}