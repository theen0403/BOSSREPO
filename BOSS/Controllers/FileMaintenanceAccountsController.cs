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
            AllotmentClassModel model = new AllotmentClassModel();
            return PartialView("AllotmentClass/IndexAllotmentClass", model);
        }
        public ActionResult AccountGroupTab()
        {
            RevisionYearModel model = new RevisionYearModel();
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
        public ActionResult COATab()
        {
            GeneralAccountModel model = new GeneralAccountModel();
            return PartialView("GeneralAccount/IndexGeneralAccount", model);
        }
        public ActionResult SubsiLedgerTab()
        {
            GeneralAccountModel model = new GeneralAccountModel();
            return PartialView("SubsidiaryLedger/_IndexSubsidiaryLedger", model);
        }
        //=======================================================================================
        //Revision Year
        public ActionResult GetRevisionCOADT()
        {
            RevisionYearModel model = new RevisionYearModel();
            List<RevisionList> getRevisionYearList = new List<RevisionList>();
            var SQLQuery = "";
            SQLQuery = "SELECT [RevID], [RevYEar], [isUsed], [Remarks] FROM [BOSS].[dbo].[Tbl_FMRevisionYear]";

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
                            isUsed = GlobalFunction.ReturnEmptyInt(dr[2]),
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
            Tbl_FMRevisionYear RevCOATable = new Tbl_FMRevisionYear();

            RevCOATable.RevYear = GlobalFunction.ReturnEmptyInt(model.getRevYearColumns.RevYear);
            RevCOATable.isUsed = model.isUsed;
            RevCOATable.Remarks = GlobalFunction.ReturnEmptyString(model.getRevYearColumns.Remarks);

            BOSSDB.Tbl_FMRevisionYear.Add(RevCOATable);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Get_UpdateRevCOA(RevisionYearModel model, int RevID)
        {
            Tbl_FMRevisionYear revCOATable = (from e in BOSSDB.Tbl_FMRevisionYear where e.RevID == RevID select e).FirstOrDefault();

            model.getRevYearColumns.RevYear = revCOATable.RevYear;
            model.isUsed = Convert.ToBoolean(revCOATable.isUsed);
            model.getRevYearColumns.Remarks = revCOATable.Remarks;
            model.RevID = RevID;
            return PartialView("RevisionOfCOA/_UpdateRCOA", model);
        }
        public ActionResult UpdateRevCOA(RevisionYearModel model)
        {
            Tbl_FMRevisionYear revCOATbl = (from e in BOSSDB.Tbl_FMRevisionYear where e.RevID == model.RevID select e).FirstOrDefault();
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
            Tbl_FMRevisionYear RCOAtbl = (from e in BOSSDB.Tbl_FMRevisionYear where e.RevID == RevID select e).FirstOrDefault();
            BOSSDB.Tbl_FMRevisionYear.Remove(RCOAtbl);
            BOSSDB.SaveChanges();
            return RedirectToAction("FileAccounts");
        }
        //=======================================================================================
        //Allotment Class
        //Revision Year
        public ActionResult GetAllotmentClassDT()
        {
            AllotmentClassModel model = new AllotmentClassModel();
            List<AllotmentClassList> getAllotmentClassList = new List<AllotmentClassList>();
            var SQLQuery = "";
            SQLQuery = "SELECT [AllotmentClassID],[AllotmentClassTitle],[RevYear] FROM [BOSS].[dbo].[Tbl_FMAllotmentClass],[Tbl_FMRevisionYear] where [Tbl_FMAllotmentClass].RevID = [Tbl_FMRevisionYear].RevID";

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
            AllotmentClassModel model = new AllotmentClassModel();
            return PartialView("AllotmentClass/_AddAllotment", model);
        }
        public ActionResult AddNewAllotmentClass(AllotmentClassModel model)
        {
            Tbl_FMAllotmentClass AllotTable = new Tbl_FMAllotmentClass();

            AllotTable.RevID = model.RevID;
            AllotTable.AllotmentClassTitle = GlobalFunction.ReturnEmptyString(model.getAllotmentClassColumns.AllotmentClassTitle);
     
            BOSSDB.Tbl_FMAllotmentClass.Add(AllotTable);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDynamicRevYear()
        {
            AllotmentClassModel model = new AllotmentClassModel();
            model.RevYearDropDownList = new SelectList((from s in BOSSDB.Tbl_FMRevisionYear.Where(a => a.isUsed == true).ToList() select new { RevID = s.RevID, RevYear = s.RevYear }), "RevID", "RevYear");

            return PartialView("DynamicFields/_RevisionYearDropDown", model);
        }
        public ActionResult Get_UpdateAllotmentClass(AllotmentClassModel model, int AllotmentClassID)
        {
            Tbl_FMAllotmentClass allotClassTBL = (from e in BOSSDB.Tbl_FMAllotmentClass where e.AllotmentClassID == AllotmentClassID select e).FirstOrDefault();

            model.AllotmentClassID = AllotmentClassID;
            model.getAllotmentClassColumns.AllotmentClassTitle = allotClassTBL.AllotmentClassTitle;
            model.RevID = Convert.ToInt32(allotClassTBL.RevID);
            return PartialView("AllotmentClass/_UpdateAllotmentClass", model);
        }
        public ActionResult UpdateAllotment(AllotmentClassModel model)
        {
            Tbl_FMAllotmentClass allotClassTbl = (from e in BOSSDB.Tbl_FMAllotmentClass where e.AllotmentClassID == model.AllotmentClassID select e).FirstOrDefault();
            allotClassTbl.AllotmentClassTitle = GlobalFunction.ReturnEmptyString(model.getAllotmentClassColumns.AllotmentClassTitle);
            allotClassTbl.RevID = GlobalFunction.ReturnEmptyInt(model.RevID);

            BOSSDB.Entry(allotClassTbl);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
         public ActionResult DeleteAllotmentClass(AllotmentClassModel model, int AllotmentClassID)
        {
            Tbl_FMAllotmentClass allotmenttbl = (from e in BOSSDB.Tbl_FMAllotmentClass where e.AllotmentClassID == AllotmentClassID select e).FirstOrDefault();
            BOSSDB.Tbl_FMAllotmentClass.Remove(allotmenttbl);
            BOSSDB.SaveChanges();
            return RedirectToAction("FileAccounts");
        }





























        //Get General Account Partial View
        public ActionResult GetGeneralAccountView()
        {
            GeneralAccountModel model = new GeneralAccountModel();

            return PartialView("ChartOfAccounts/GeneralAccount/GeneralAccountTabIndex", model);
        }
        public ActionResult GetAddGeneralAccount()
        {
            GeneralAccountModel model = new GeneralAccountModel();

            return PartialView("ChartOfAccounts/GeneralAccount/_AddGeneralAccount", model);
        }
        public ActionResult GetAddGeneralAccountforUpdate(int AllotmentID)
        {
            GeneralAccountModel model = new GeneralAccountModel();
            model.AllotmentID = AllotmentID;
            return PartialView("ChartOfAccounts/GeneralAccount/_AddGeneralAccount", model);
        }
        public ActionResult GetDynamicAllotmentClass()
        {
            GeneralAccountModel model = new GeneralAccountModel();
            return PartialView("ChartOfAccounts/GeneralAccount/_DynamicAllotmentClass", model);
        }
        public ActionResult GetDynamicAllotmentClassForUpdate(int AllotmentID)
        {
            GeneralAccountModel model = new GeneralAccountModel();
            model.AllotmentID = AllotmentID;
            return PartialView("ChartOfAccounts/GeneralAccount/_DynamicAllotmentClassForUpdate", model);
        }
        public ActionResult GetGeneralAccountDT(int AllotmentID)
        {
            GeneralAccountModel model = new GeneralAccountModel();
            List<GeneralAccountList> getGenAcctList = new List<GeneralAccountList>(); 
             var SQLQuery = "";
            SQLQuery = "SELECT [GenAccountID] ,[GenAccountCode], [GenAccountTitle] ,[isReserve] ,[ReservePercent] ,[isFullRelease] ,[isContinuing] ,[isOBRCashAdvance] ,[Tbl_FMGeneralAccount].[AllotmentID] FROM [dbo].[Tbl_FMGeneralAccount], [dbo].[AllotmentClass] WHERE [Tbl_FMGeneralAccount].AllotmentID = [AllotmentClass].AllotmentID and[Tbl_FMGeneralAccount].AllotmentID =" + AllotmentID;

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
                        getGenAcctList.Add(new GeneralAccountList()
                        {
                            GenAccountID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            GenAccountCode = GlobalFunction.ReturnEmptyString(dr[1]),
                            GenAccountTitle = GlobalFunction.ReturnEmptyString(dr[2]),
                            isReserve = GlobalFunction.ReturnEmptyInt(dr[3]),
                            ReservePercent = GlobalFunction.ReturnEmptyInt(dr[4]),
                            isFullRelease = GlobalFunction.ReturnEmptyInt(dr[5]),
                            isContinuing = GlobalFunction.ReturnEmptyInt(dr[6]),
                            isOBRCashAdvance = GlobalFunction.ReturnEmptyInt(dr[7])
                        });
                    }
                }
                Connection.Close();
            }
            model.getGenAcctList = getGenAcctList.ToList();
            return PartialView("ChartOfAccounts/GeneralAccount/_TableGeneralAccount", getGenAcctList);
        }
        public ActionResult AddNewGenAcct(GeneralAccountModel model)
        {
            Tbl_FMGeneralAccount GenAcctTable = new Tbl_FMGeneralAccount();

            GenAcctTable.GenAccountTitle = GlobalFunction.ReturnEmptyString(model.getGenAcctColumns.GenAccountTitle);
            GenAcctTable.GenAccountCode = GlobalFunction.ReturnEmptyString(model.getGenAcctColumns.GenAccountCode);
            GenAcctTable.isReserve = model.isReserve;
            GenAcctTable.ReservePercent = GlobalFunction.ReturnEmptyInt(model.getGenAcctColumns.ReservePercent);
            GenAcctTable.isFullRelease = model.isFullRelease;
            GenAcctTable.isContinuing = model.isContinuing;
            GenAcctTable.isOBRCashAdvance = model.isOBRCashAdvance;
            GenAcctTable.AllotmentID = GlobalFunction.ReturnEmptyInt(model.AllotmentID);

            BOSSDB.Tbl_FMGeneralAccount.Add(GenAcctTable);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Get_UpdateGenAcct(GeneralAccountModel model, int GenAccountID)
        {
            Tbl_FMGeneralAccount GenAcctTable = (from e in BOSSDB.Tbl_FMGeneralAccount where e.GenAccountID == GenAccountID select e).FirstOrDefault();

            model.getGenAcctColumns2.GenAccountTitle = GenAcctTable.GenAccountTitle;
            model.getGenAcctColumns2.GenAccountCode = GenAcctTable.GenAccountCode;
            model.isReserve = Convert.ToBoolean(GenAcctTable.isReserve);
            model.getGenAcctColumns2.ReservePercent = GenAcctTable.ReservePercent;
            model.isFullRelease = Convert.ToBoolean(GenAcctTable.isFullRelease);
            model.isContinuing = Convert.ToBoolean(GenAcctTable.isContinuing);
            model.isOBRCashAdvance = Convert.ToBoolean(GenAcctTable.isOBRCashAdvance);
            model.allotTempID = Convert.ToInt32(GenAcctTable.AllotmentID);
            model.GenAccountID = GenAccountID;
            return PartialView("ChartOfAccounts/GeneralAccount/_UpdateGeneralAccount", model);
        }
        public ActionResult UpdateGenAcct(GeneralAccountModel model)
        {
            Tbl_FMGeneralAccount genacctTBL = (from e in BOSSDB.Tbl_FMGeneralAccount where e.GenAccountID == model.GenAccountID select e).FirstOrDefault();
            genacctTBL.GenAccountTitle = GlobalFunction.ReturnEmptyString(model.getGenAcctColumns2.GenAccountTitle);
            genacctTBL.GenAccountCode = GlobalFunction.ReturnEmptyString(model.getGenAcctColumns2.GenAccountCode);
            genacctTBL.isReserve = model.isReserve;
            genacctTBL.ReservePercent = GlobalFunction.ReturnEmptyInt(model.getGenAcctColumns2.ReservePercent);
            genacctTBL.isFullRelease = model.isFullRelease;
            genacctTBL.isContinuing = model.isContinuing;
            genacctTBL.isOBRCashAdvance = model.isOBRCashAdvance;
            genacctTBL.AllotmentID = GlobalFunction.ReturnEmptyInt(model.AllotmentID);

            BOSSDB.Entry(genacctTBL);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteGenAcct(GeneralAccountModel model, int GenAccountID)
        {
            Tbl_FMGeneralAccount genacctTBL = (from e in BOSSDB.Tbl_FMGeneralAccount where e.GenAccountID == GenAccountID select e).FirstOrDefault();
            BOSSDB.Tbl_FMGeneralAccount.Remove(genacctTBL);
            BOSSDB.SaveChanges();
            return RedirectToAction("FileAccounts");
        }
        public ActionResult GetChangeAcctDetailsModal()
        {
            return PartialView("ChartOfAccounts/GeneralAccount/_UpdateGeneralAccount");
        }
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
        //    model.GeneralAccntList = new SelectList((from s in BOSSDB.Tbl_FMGeneralAccount.ToList() where s.AllotmentID == AllotmentID select new { GenAccountID = s.GenAccountID, GenAccountTitle =  s.GenAccountCode + " - " + s.GenAccountTitle   }), "GenAccountID", "GenAccountTitle");
        //    return PartialView("ChartOfAccounts/SubAccount/_DynamicGeneralAccount", model);
        //}
        //public ActionResult GetSubAccountDT(int AllotmentID, int GenAccountID)
        //{
        //    SubAccountsModel model = new SubAccountsModel();
        //    List<SubAccountList> getSubAcctList = new List<SubAccountList>();
        //    var SQLQuery = "SELECT [SubAccountID] ,[SubAccountCode] ,[SubAccountTitle] ,[Tbl_FMSubAccount].[isReserve] ,[Tbl_FMSubAccount].[ReservePercent] ,[Tbl_FMSubAccount].[isFullRelease] ,[Tbl_FMSubAccount].[isContinuing] ,[Tbl_FMSubAccount].[isOBRCashAdvance] ,[Tbl_FMSubAccount].[GenAccountID],(Tbl_FMGeneralAccount.GenAccountCode + ' - ' + [SubAccountCode]) as GenCode FROM[BOSS].[dbo].[Tbl_FMSubAccount], [BOSS].[dbo].AllotmentClass,Tbl_FMGeneralAccount where[dbo].[Tbl_FMSubAccount].AllotmentID = [dbo].AllotmentClass.AllotmentID and Tbl_FMGeneralAccount.GenAccountID=[Tbl_FMSubAccount].GenAccountID and [Tbl_FMSubAccount].AllotmentID = " + AllotmentID + " and[Tbl_FMSubAccount].GenAccountID = " + GenAccountID;

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

        //    var SubAccntTable = (from e in BOSSDB.Tbl_FMGeneralAccount where e.GenAccountID == GenAccountID select e).FirstOrDefault();
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
        public ActionResult ValidatePin(string Pin_Acct)
        {
            var pinStatus = 0;
            var pintemp = (from e in BOSSDB.PIN_Accounts where e.Pin_Acct== Pin_Acct select e).FirstOrDefault();
            if(pintemp != null)
            {
                pinStatus = 1;
            }
            
            return Json(pinStatus, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdatePIN(GeneralAccountModel model)
        {
            PIN_Accounts pin = (from e in BOSSDB.PIN_Accounts select e).FirstOrDefault();

            pin.Pin_Acct = GlobalFunction.ReturnEmptyString(model.confirm_newPIN);

            BOSSDB.Entry(pin);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}