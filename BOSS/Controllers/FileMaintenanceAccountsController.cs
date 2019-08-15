using BOSS.GlobalFunctions;
using BOSS.Models;
using BOSS.Models.FMAccounts;
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
            GeneralAccountsModel model = new GeneralAccountsModel();
            return View(model);
        }
        public ActionResult COATab()
        {
            GeneralAccountsModel model = new GeneralAccountsModel();
            return PartialView("ChartOfAccounts/IndexCOA", model);
        }
        public ActionResult RevisionCOATab()
        {
            RevisionOfCOAModel model = new RevisionOfCOAModel();
            return PartialView("RevisionOfCOA/IndexRCOA", model);
        }
        public ActionResult PINTab()
        {
            GeneralAccountsModel model = new GeneralAccountsModel();
            return PartialView("PIN/IndexCOAPIN", model);
        }
        //=======================================================================================
        //Get General Account Partial View
        public ActionResult GetGeneralAccountView()
        {
            GeneralAccountsModel model = new GeneralAccountsModel();

            return PartialView("ChartOfAccounts/GeneralAccount/GeneralAccountTabIndex", model);
        }
        public ActionResult GetAddGeneralAccount()
        {
            GeneralAccountsModel model = new GeneralAccountsModel();

            return PartialView("ChartOfAccounts/GeneralAccount/_AddGeneralAccount", model);
        }
        public ActionResult GetAddGeneralAccountforUpdate(int AllotmentID)
        {
            GeneralAccountsModel model = new GeneralAccountsModel();
            model.AllotmentID = AllotmentID;
            return PartialView("ChartOfAccounts/GeneralAccount/_AddGeneralAccount", model);
        }
        public ActionResult GetDynamicAllotmentClass()
        {
            GeneralAccountsModel model = new GeneralAccountsModel();
            return PartialView("ChartOfAccounts/GeneralAccount/_DynamicAllotmentClass", model);
        }
        public ActionResult GetDynamicAllotmentClassForUpdate(int AllotmentID)
        {
            GeneralAccountsModel model = new GeneralAccountsModel();
            model.AllotmentID = AllotmentID;
            return PartialView("ChartOfAccounts/GeneralAccount/_DynamicAllotmentClassForUpdate", model);
        }
        public ActionResult GetGeneralAccountDT(int AllotmentID)
        {
            GeneralAccountsModel model = new GeneralAccountsModel();
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
        public ActionResult AddNewGenAcct(GeneralAccountsModel model)
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
        public ActionResult Get_UpdateGenAcct(GeneralAccountsModel model, int GenAccountID)
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
        public ActionResult UpdateGenAcct(GeneralAccountsModel model)
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
        public ActionResult DeleteGenAcct(GeneralAccountsModel model, int GenAccountID)
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
        public ActionResult GetSubAccountView()
        {
            SubAccountsModel model = new SubAccountsModel();
            return PartialView("ChartOfAccounts/SubAccount/SubAccountTabIndex", model);
        }
        public ActionResult GetDynamicGenAccnt(int AllotmentID)
        {
            SubAccountsModel model = new SubAccountsModel();
            model.GeneralAccntList = new SelectList((from s in BOSSDB.Tbl_FMGeneralAccount.ToList() where s.AllotmentID == AllotmentID select new { GenAccountID = s.GenAccountID, GenAccountTitle =  s.GenAccountCode + " - " + s.GenAccountTitle   }), "GenAccountID", "GenAccountTitle");
            return PartialView("ChartOfAccounts/SubAccount/_DynamicGeneralAccount", model);
        }
        public ActionResult GetSubAccountDT(int AllotmentID, int GenAccountID)
        {
            SubAccountsModel model = new SubAccountsModel();
            List<SubAccountList> getSubAcctList = new List<SubAccountList>();
            var SQLQuery = "SELECT [SubAccountID] ,[SubAccountCode] ,[SubAccountTitle] ,[Tbl_FMSubAccount].[isReserve] ,[Tbl_FMSubAccount].[ReservePercent] ,[Tbl_FMSubAccount].[isFullRelease] ,[Tbl_FMSubAccount].[isContinuing] ,[Tbl_FMSubAccount].[isOBRCashAdvance] ,[Tbl_FMSubAccount].[GenAccountID],(Tbl_FMGeneralAccount.GenAccountCode + ' - ' + [SubAccountCode]) as GenCode FROM[BOSS].[dbo].[Tbl_FMSubAccount], [BOSS].[dbo].AllotmentClass,Tbl_FMGeneralAccount where[dbo].[Tbl_FMSubAccount].AllotmentID = [dbo].AllotmentClass.AllotmentID and Tbl_FMGeneralAccount.GenAccountID=[Tbl_FMSubAccount].GenAccountID and [Tbl_FMSubAccount].AllotmentID = " + AllotmentID + " and[Tbl_FMSubAccount].GenAccountID = " + GenAccountID;

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
                        getSubAcctList.Add(new SubAccountList()
                        {
                            SubAccountID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            SubAccountCode = GlobalFunction.ReturnEmptyString(dr[9]),
                            SubAccountTitle = GlobalFunction.ReturnEmptyString(dr[2]),
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
            model.getSubAcctList = getSubAcctList.ToList();

            return PartialView("ChartOfAccounts/SubAccount/_TableSubAccount", model.getSubAcctList);
        }
        public ActionResult GetAddSubAccount(int AllotmentID, int GenAccountID)
        {
            SubAccountsModel model = new SubAccountsModel();
            model.AllotmentID = AllotmentID;
            model.GenAccountID = GenAccountID;
            return PartialView("ChartOfAccounts/SubAccount/_AddSubAccount", model);
        }
        public ActionResult GetGenAccntCodeField(int GenAccountID)
        {
            SubAccountsModel model = new SubAccountsModel();

            var SubAccntTable = (from e in BOSSDB.Tbl_FMGeneralAccount where e.GenAccountID == GenAccountID select e).FirstOrDefault();
            model.GenAccountCode = SubAccntTable.GenAccountCode;

            return PartialView("ChartOfAccounts/SubAccount/_GenAcctCodeView", model);
        }
        public ActionResult AddNewSubAccount(SubAccountsModel model)
        {
            Tbl_FMSubAccount subAcctTable = new Tbl_FMSubAccount();

            subAcctTable.SubAccountCode =GlobalFunction.ReturnEmptyString(model.getSubAcctColumns.SubAccountCode);
            subAcctTable.SubAccountTitle = GlobalFunction.ReturnEmptyString(model.getSubAcctColumns.SubAccountTitle);
            subAcctTable.isReserve = model.isReserve;
            subAcctTable.ReservePercent = GlobalFunction.ReturnEmptyInt(model.getSubAcctColumns.ReservePercent);
            subAcctTable.isFullRelease = model.isFullRelease;
            subAcctTable.isContinuing = model.isContinuing;
            subAcctTable.isOBRCashAdvance = model.isOBRCashAdvance;
            subAcctTable.GenAccountID = GlobalFunction.ReturnEmptyInt(model.GenAccountID);
            subAcctTable.AllotmentID = GlobalFunction.ReturnEmptyInt(model.AllotmentID);
            BOSSDB.Tbl_FMSubAccount.Add(subAcctTable);
            BOSSDB.SaveChanges();
            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Get_UpdateSubAcct(SubAccountsModel model, int SubAccountID)
        {
            Tbl_FMSubAccount SubAcctTable = (from e in BOSSDB.Tbl_FMSubAccount where e.SubAccountID == SubAccountID select e).FirstOrDefault();

            model.getSubAcctColumns2.SubAccountTitle = SubAcctTable.SubAccountTitle;
            model.getSubAcctColumns2.SubAccountCode = SubAcctTable.SubAccountCode;
            model.isReserve = Convert.ToBoolean(SubAcctTable.isReserve);
            model.getSubAcctColumns2.ReservePercent = SubAcctTable.ReservePercent;
            model.isFullRelease = Convert.ToBoolean(SubAcctTable.isFullRelease);
            model.isContinuing = Convert.ToBoolean(SubAcctTable.isContinuing);
            model.isOBRCashAdvance = Convert.ToBoolean(SubAcctTable.isOBRCashAdvance);
            model.AllotmentID = Convert.ToInt32(SubAcctTable.AllotmentID);
            model.GenAccountID = Convert.ToInt32(SubAcctTable.GenAccountID);
            model.SubAccountID = SubAccountID;
            return PartialView("ChartOfAccounts/SubAccount/_UpdateSubAccount", model);
        }
        public ActionResult UpdateSubAcct(SubAccountsModel model)
        {
            Tbl_FMSubAccount subacctTable = (from e in BOSSDB.Tbl_FMSubAccount where e.SubAccountID == model.SubAccountID select e).FirstOrDefault();
           subacctTable.SubAccountTitle = GlobalFunction.ReturnEmptyString(model.getSubAcctColumns2.SubAccountTitle);
            subacctTable.SubAccountCode = GlobalFunction.ReturnEmptyString(model.getSubAcctColumns2.SubAccountCode);
            subacctTable.isReserve = model.isReserve;
            subacctTable.ReservePercent = GlobalFunction.ReturnEmptyInt(model.getSubAcctColumns2.ReservePercent);
            subacctTable.isFullRelease = model.isFullRelease;
            subacctTable.isContinuing = model.isContinuing;
            subacctTable.isOBRCashAdvance = model.isOBRCashAdvance;
            subacctTable.AllotmentID = GlobalFunction.ReturnEmptyInt(model.AllotmentID);
            subacctTable.GenAccountID = GlobalFunction.ReturnEmptyInt(model.GenAccountID);

            BOSSDB.Entry(subacctTable);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteSubAcct(SubAccountsModel model, int SubAccountID)
        {
            Tbl_FMSubAccount SubacctTBL = (from e in BOSSDB.Tbl_FMSubAccount where e.SubAccountID == SubAccountID select e).FirstOrDefault();
            BOSSDB.Tbl_FMSubAccount.Remove(SubacctTBL);
            BOSSDB.SaveChanges();
            return RedirectToAction("FileAccounts");
        }
        //====================================
        //Revision COA
        public ActionResult GetRevisionCOADT()
        {
            RevisionOfCOAModel model = new RevisionOfCOAModel();
            List<RevisionCOAList> getRevCOAList = new List<RevisionCOAList>();
            var SQLQuery = "";
            SQLQuery = "SELECT [RevID], [RevYEar], [isUsed], [Remarks] FROM [BOSS].[dbo].[Tbl_FMRevisionOfCOA]";

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
                        getRevCOAList.Add(new RevisionCOAList()
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
            model.getRevCOAList = getRevCOAList.ToList();
            return PartialView("RevisionOfCOA/_TableRCOA", getRevCOAList);
        }
        public ActionResult GetAddRevision()
        {
            RevisionOfCOAModel model = new RevisionOfCOAModel();
            return PartialView("RevisionOfCOA/_AddRCOA",model);
        }
        public ActionResult AddNewRevisionCOA(RevisionOfCOAModel model)
        {
            Tbl_FMRevisionOfCOA RevCOATable = new Tbl_FMRevisionOfCOA();

            RevCOATable.RevYEar = GlobalFunction.ReturnEmptyInt(model.getRevCOAColumns.RevYEar);
            RevCOATable.isUsed = model.isUsed;
            RevCOATable.Remarks = GlobalFunction.ReturnEmptyString(model.getRevCOAColumns.Remarks);

            BOSSDB.Tbl_FMRevisionOfCOA.Add(RevCOATable);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Get_UpdateRevCOA(RevisionOfCOAModel model, int RevID)
        {
            Tbl_FMRevisionOfCOA revCOATable = (from e in BOSSDB.Tbl_FMRevisionOfCOA where e.RevID == RevID select e).FirstOrDefault();

            model.getRevCOAColumns2.RevYEar = revCOATable.RevYEar;
            model.isUsed = Convert.ToBoolean(revCOATable.isUsed);
            model.getRevCOAColumns2.Remarks = revCOATable.Remarks;
            model.RevID = RevID;
            return PartialView("RevisionOfCOA/_UpdateRCOA", model);
        }
        public ActionResult UpdateRevCOA(RevisionOfCOAModel model)
        {
            Tbl_FMRevisionOfCOA revCOATbl = (from e in BOSSDB.Tbl_FMRevisionOfCOA where e.RevID == model.RevID select e).FirstOrDefault();
            revCOATbl.RevYEar = GlobalFunction.ReturnEmptyInt(model.getRevCOAColumns2.RevYEar);
            revCOATbl.isUsed = model.isUsed;
            revCOATbl.Remarks = GlobalFunction.ReturnEmptyString(model.getRevCOAColumns2.Remarks);
            
            BOSSDB.Entry(revCOATbl);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteRCOA(RevisionOfCOAModel model, int RevID)
        {
            Tbl_FMRevisionOfCOA RCOAtbl = (from e in BOSSDB.Tbl_FMRevisionOfCOA where e.RevID == RevID select e).FirstOrDefault();
            BOSSDB.Tbl_FMRevisionOfCOA.Remove(RCOAtbl);
            BOSSDB.SaveChanges();
            return RedirectToAction("FileAccounts");
        }
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
        public ActionResult UpdatePIN(GeneralAccountsModel model)
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