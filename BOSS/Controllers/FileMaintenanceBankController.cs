using BOSS.GlobalFunctions;
using BOSS.Models;
using BOSS.Models.FMmodels.FMBanksModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class FileMaintenanceBankController : Controller
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        DatatypeValidation GlobalFunction = new DatatypeValidation();
        [Authorize]
        // GET: FileMaintenanceBank
        public ActionResult FileMaintenanceBank()
        {
            BanksModel model = new BanksModel();
            return View(model);
        }
        //---------------------------------------------------------------------------------------------------------------------
        //Bank Tab
        //---------------------------------------------------------------------------------------------------------------------
        public ActionResult GetBankTab()
        {
            BanksModel model = new BanksModel();
            return PartialView("BankTab/IndexBank", model);
        }
        public ActionResult GetBankForm(int ActionID, int PrimaryID)
        {
            BanksModel model = new BanksModel();

            if (ActionID == 2)
            {
                var bank = (from a in BOSSDB.Tbl_FMBank_Banks where a.BankID == PrimaryID select a).FirstOrDefault();
                model.BankList.BankID = bank.BankID;
                model.BankList.BankCode = bank.BankCode;
                model.BankList.BankAddress = bank.BankAddress;
                model.BankList.BankTitle = bank.BankTitle;
            }
            model.ActionID = ActionID;
            return PartialView("BankTab/_BankForm", model);
        }
        public ActionResult GetBankDTable()
        {
            BanksModel model = new BanksModel();
            List<BankList> getBankList = new List<BankList>();
            var SQLQuery = "SELECT * FROM [Tbl_FMBank_Banks]";
            //BankID, BankTitle, BankCode, BankAddress
            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_Banks]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        getBankList.Add(new BankList()
                        {
                            BankID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            BankTitle = GlobalFunction.ReturnEmptyString(dr[1]),
                            BankCode = GlobalFunction.ReturnEmptyString(dr[2]),
                            BankAddress = GlobalFunction.ReturnEmptyString(dr[3])
                        });
                    }
                }
                Connection.Close();
            }
            model.getBankList = getBankList.ToList();
            return PartialView("BankTab/_TableBank", getBankList);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveBank(BanksModel model)
        {
            var isExist = "";
            if (ModelState.IsValid)
            {
                var bankTitle = model.BankList.BankTitle;
                var bankCode = model.BankList.BankCode;
                var bankAdd = model.BankList.BankAddress;

                Tbl_FMBank_Banks checkBanks = (from a in BOSSDB.Tbl_FMBank_Banks where (a.BankTitle == bankTitle && a.BankAddress == bankAdd) select a).FirstOrDefault();

                if (model.ActionID == 1)
                {
                    if (checkBanks == null)
                    {
                        Tbl_FMBank_Banks bank = new Tbl_FMBank_Banks();
                        bank.BankTitle = bankTitle;
                        bank.BankCode = bankCode;
                        bank.BankAddress = bankAdd;
                        BOSSDB.Tbl_FMBank_Banks.Add(bank);
                        BOSSDB.SaveChanges();
                        isExist = "false";
                    }
                    else if (checkBanks != null)
                    {
                        isExist = "true";
                    }
                }
                else if (model.ActionID == 2)
                {
                    Tbl_FMBank_Banks bankii = (from a in BOSSDB.Tbl_FMBank_Banks where a.BankID == model.BankList.BankID select a).FirstOrDefault();
                    List<Tbl_FMBank_Banks> banktitleCount = (from e in BOSSDB.Tbl_FMBank_Banks where e.BankTitle == bankTitle select e).ToList();
                    List<Tbl_FMBank_Banks> bankaddCount = (from e in BOSSDB.Tbl_FMBank_Banks where e.BankAddress == bankAdd select e).ToList();
                    if (checkBanks != null)
                    {
                        if (bankii.BankTitle == bankTitle && bankii.BankCode == bankCode && bankii.BankAddress == bankAdd)
                        {
                            isExist = "justUpdate";
                        }
                        else
                        {
                            if (bankii.BankTitle != bankTitle && banktitleCount.Count >= 1 || bankii.BankAddress != bankAdd && bankaddCount.Count >= 1)
                            {
                                isExist = "true";
                            }
                            else
                            {
                                isExist = "justUpdate";
                            }
                        }
                    }
                    else if (checkBanks == null)
                    {
                        isExist = "justUpdate";
                    }

                    if(isExist == "justUpdate")
                    {
                        bankii.BankTitle = bankTitle;
                        bankii.BankCode = bankCode;
                        bankii.BankAddress = bankAdd;
                        BOSSDB.Entry(bankii);
                        BOSSDB.SaveChanges();
                    }
                }
            }
            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new { isExist = isExist }
            };
        }
        public ActionResult DeleteBank(int PrimaryID)
        {
            Tbl_FMBank_Banks banks = (from a in BOSSDB.Tbl_FMBank_Banks where a.BankID == PrimaryID select a).FirstOrDefault();
            Tbl_FMBank_BankAccounts bankaccnt = (from a in BOSSDB.Tbl_FMBank_BankAccounts where a.BankID == PrimaryID select a).FirstOrDefault();
            //Tbl_FMBrgy_BrgyBankAccount brgy = (from a in BOSSDB.Tbl_FMBrgy_BrgyBankAccount where a.BankID == PrimaryID select a).FirstOrDefault();

            var confirmDelete = "";
            if (banks != null)
            {
                if (bankaccnt != null)
                {
                    confirmDelete = "true";
                }
                else
                {
                    confirmDelete = "false";
                }
            }
            var result = new { confirmDelete = confirmDelete };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConfirmDeleteBank(int PrimaryID)
        {
            Tbl_FMBank_Banks bankss = (from a in BOSSDB.Tbl_FMBank_Banks where a.BankID == PrimaryID select a).FirstOrDefault();
            BOSSDB.Tbl_FMBank_Banks.Remove(bankss);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //---------------------------------------------------------------------------------------------------------------------
        //Account Type Tab
        //---------------------------------------------------------------------------------------------------------------------
        public ActionResult GetAccntTypeTab()
        {
            BanksModel model = new BanksModel();
            return PartialView("AccountTypeTab/IndexAccountType", model);
        }
        public ActionResult GetAccntTypeForm(int ActionID, int PrimaryID)
        {
            AccountTypeModel model = new AccountTypeModel();

            if (ActionID == 2)
            {
                var bank = (from a in BOSSDB.Tbl_FMBank_AccountType where a.AccntTypeID == PrimaryID select a).FirstOrDefault();
                model.AccntTypeList.AccntType = bank.AccntType;
                model.AccntTypeList.AccntTypeID = bank.AccntTypeID;
            }
            model.ActionID = ActionID;
            return PartialView("AccountTypeTab/_AccountTypeForm", model);
        }
        public ActionResult GetAccntTypeDTable()
        {
            AccountTypeModel model = new AccountTypeModel();
            List<AccntTypeList> getAccntTypeList = new List<AccntTypeList>();
            var SQLQuery = "SELECT * FROM [Tbl_FMBank_AccountType]";
            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_Banks]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        getAccntTypeList.Add(new AccntTypeList()
                        {
                            AccntTypeID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            AccntType = GlobalFunction.ReturnEmptyString(dr[1])
                        });
                    }
                }
                Connection.Close();
            }
            model.getAccntTypeList = getAccntTypeList.ToList();
            return PartialView("AccountTypeTab/_TableAccountType", getAccntTypeList);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAccntType(AccountTypeModel model)
        {
            var isExist = "";
            if (ModelState.IsValid)
            {
                var accntType = model.AccntTypeList.AccntType;

                Tbl_FMBank_AccountType checkAccntType = (from a in BOSSDB.Tbl_FMBank_AccountType where (a.AccntType == accntType) select a).FirstOrDefault();

                if (model.ActionID == 1)
                {
                    if (checkAccntType == null)
                    {
                        Tbl_FMBank_AccountType AT = new Tbl_FMBank_AccountType();
                        AT.AccntType = accntType;
                        BOSSDB.Tbl_FMBank_AccountType.Add(AT);
                        BOSSDB.SaveChanges();
                        isExist = "false";
                    }
                    else if (checkAccntType != null)
                    {
                        isExist = "true";
                    }
                }
                else if (model.ActionID == 2)
                {
                    Tbl_FMBank_AccountType AType = (from a in BOSSDB.Tbl_FMBank_AccountType where a.AccntTypeID == model.AccntTypeList.AccntTypeID select a).FirstOrDefault();
                    List<Tbl_FMBank_AccountType> acctypeCount = (from e in BOSSDB.Tbl_FMBank_AccountType where e.AccntType == accntType select e).ToList();
                    if (checkAccntType != null)
                    {
                        if (AType.AccntType != accntType && acctypeCount.Count >= 1)
                        {
                            isExist = "true";
                        }
                        else
                        {
                            isExist = "justUpdate";
                        }
                    }
                    else if (checkAccntType == null)
                    {
                        isExist = "justUpdate";
                    }

                    if (isExist == "justUpdate")
                    {
                        AType.AccntType = accntType;
                        BOSSDB.Entry(AType);
                        BOSSDB.SaveChanges();
                    }
                }
            }
            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new { isExist = isExist }
            };
        }
        public ActionResult DeleteAccntType(int PrimaryID)
        {
            Tbl_FMBank_AccountType AccntType = (from a in BOSSDB.Tbl_FMBank_AccountType where a.AccntTypeID == PrimaryID select a).FirstOrDefault();
            Tbl_FMBank_BankAccounts bankaccnt = (from a in BOSSDB.Tbl_FMBank_BankAccounts where a.AccntTypeID == PrimaryID select a).FirstOrDefault();
            
            var confirmDelete = "";
            if (AccntType != null)
            {
                if (bankaccnt != null)
                {
                    confirmDelete = "true";
                }
                else
                {
                    confirmDelete = "false";
                }
            }
            var result = new { confirmDelete = confirmDelete };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConfirmDeleteAccntType(int PrimaryID)
        {
            Tbl_FMBank_AccountType acctype = (from a in BOSSDB.Tbl_FMBank_AccountType where a.AccntTypeID == PrimaryID select a).FirstOrDefault();
            BOSSDB.Tbl_FMBank_AccountType.Remove(acctype);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //---------------------------------------------------------------------------------------------------------------------
        //Bank Accounts Tab
        //---------------------------------------------------------------------------------------------------------------------
        public ActionResult GetBankAccntTab()
        {
            BanksModel model = new BanksModel();
            return PartialView("BankAccountsTab/IndexBankAccount", model);
        }
        public ActionResult GetBankAccntForm(int ActionID, int PrimaryID)
        {
            BankAccountsModel model = new BankAccountsModel();

            if (ActionID == 2)
            {
                var bank = (from a in BOSSDB.Tbl_FMBank_BankAccounts where a.BankAccntID == PrimaryID select a).FirstOrDefault();
                model.BankAccntList.AccntNo = bank.AccntNo;
                model.BankAccntList.AccntName = bank.AccntName;
                model.BankAccntList.BankID = Convert.ToInt32(bank.BankID);
                model.BankAccntList.FundID = Convert.ToInt32(bank.FundID);
                model.BankAccntList.GAID = Convert.ToInt32(bank.GAID);
                model.BankAccntList.AccntTypeID = Convert.ToInt32(bank.AccntTypeID);
                model.BankAccntList.BankAccntID = bank.BankAccntID;
            }
            model.ActionID = ActionID;
            return PartialView("BankAccountsTab/_BankAccountForm", model);
        }
        public ActionResult onchangeGACode(int GAID, BankAccountsModel model)
        {
            var genAccnt = (from a in BOSSDB.Tbl_FMCOA_GeneralAccount where a.GAID == GAID select a).FirstOrDefault();
            var passCon = "";
            if (genAccnt == null)
            {
                passCon = "";
            }
            else
            {
                var subMajAccntGrp = (from a in BOSSDB.Tbl_FMCOA_SubMajorAccountGroup where a.SMAGID == genAccnt.SMAGID select a).FirstOrDefault();
                var majAccntGrp = (from a in BOSSDB.Tbl_FMCOA_MajorAccountGroup where a.MAGID == subMajAccntGrp.MAGID select a).FirstOrDefault();
                var accntGrp = (from a in BOSSDB.Tbl_FMCOA_AccountGroup where a.AGID == majAccntGrp.AGID select a).FirstOrDefault();


                if (accntGrp != null)
                {
                    passCon = (accntGrp.AGCode + "-" + majAccntGrp.MAGCode + "-" + subMajAccntGrp.SMAGCode + "-" + genAccnt.GACode);
                }
            }
            var result = new { passCon = passCon };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBankAccntDTable()
        {
            BankAccountsModel model = new BankAccountsModel();
            List<BankAccntList> getBankAccntList = new List<BankAccntList>();
            var SQLQuery = "SELECT [BankAccntID] , Tbl_FMBank_Banks.BankTitle, [AccntNo],[AccntName],[Tbl_FMCOA_GeneralAccount].GATitle,CONCAT(Tbl_FMCOA_AccountGroup.AGCode , '-', Tbl_FMCOA_MajorAccountGroup.MAGCode, '-', Tbl_FMCOA_SubMajorAccountGroup.SMAGCode , '-', Tbl_FMCOA_GeneralAccount.GACode) as concatCode,Tbl_FMBank_AccountType.AccntType,Tbl_FMFund_Fund.FundTitle FROM [Tbl_FMBank_BankAccounts], Tbl_FMBank_AccountType, Tbl_FMBank_Banks, Tbl_FMCOA_GeneralAccount, Tbl_FMFund_Fund, Tbl_FMCOA_SubMajorAccountGroup, Tbl_FMCOA_MajorAccountGroup, Tbl_FMCOA_AccountGroup where Tbl_FMFund_Fund.FundID = [Tbl_FMBank_BankAccounts].FundID and Tbl_FMBank_AccountType.AccntTypeID = [Tbl_FMBank_BankAccounts].AccntTypeID and Tbl_FMCOA_GeneralAccount.GAID = [Tbl_FMBank_BankAccounts].GAID and Tbl_FMBank_Banks.BankID = [Tbl_FMBank_BankAccounts].BankID and Tbl_FMCOA_SubMajorAccountGroup.SMAGID = Tbl_FMCOA_GeneralAccount.SMAGID and Tbl_FMCOA_MajorAccountGroup.MAGID = Tbl_FMCOA_SubMajorAccountGroup.MAGID and Tbl_FMCOA_AccountGroup.AGID = Tbl_FMCOA_MajorAccountGroup.AGID";
            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            { 
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_Banks]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        getBankAccntList.Add(new BankAccntList()
                        {
                            BankAccntID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            BankTitle = GlobalFunction.ReturnEmptyString(dr[1]),
                            AccntNo = GlobalFunction.ReturnEmptyString(dr[2]),
                            AccntName = GlobalFunction.ReturnEmptyString(dr[3]),
                            GATitle = GlobalFunction.ReturnEmptyString(dr[4]),
                            concatCode = GlobalFunction.ReturnEmptyString(dr[5]),
                            AccntType = GlobalFunction.ReturnEmptyString(dr[6]),
                            FundTitle = GlobalFunction.ReturnEmptyString(dr[7])
                        });
                    }
                }
                Connection.Close();
            }
            model.getBankAccntList = getBankAccntList.ToList();
            return PartialView("BankAccountsTab/_TableBankAccount", getBankAccntList);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveBankAccnt(BankAccountsModel model)
        {
            var isExist = "";
            if (ModelState.IsValid)
            {
                var accntname = model.BankAccntList.AccntName;
                var bankid = model.BankAccntList.BankID;
                var accntnumber = model.BankAccntList.AccntNo;

                Tbl_FMBank_BankAccounts checkbankaccnt = (from a in BOSSDB.Tbl_FMBank_BankAccounts where (a.AccntNo == accntnumber) select a).FirstOrDefault();

                if (model.ActionID == 1)
                {
                    if (checkbankaccnt == null)
                    {
                        Tbl_FMBank_BankAccounts BankAccnt = new Tbl_FMBank_BankAccounts();
                        BankAccnt.AccntName = accntname;
                        BankAccnt.AccntNo = accntnumber;
                        BankAccnt.BankID = bankid;
                        BankAccnt.FundID = model.BankAccntList.FundID;
                        BankAccnt.GAID = model.BankAccntList.GAID;
                        BankAccnt.AccntTypeID = model.BankAccntList.AccntTypeID;
                        BOSSDB.Tbl_FMBank_BankAccounts.Add(BankAccnt);
                        BOSSDB.SaveChanges();
                        isExist = "false";
                    }
                    else if (checkbankaccnt != null)
                    {
                        isExist = "true";
                    }
                }
                else if (model.ActionID == 2)
                {
                    Tbl_FMBank_BankAccounts bankTypii = (from a in BOSSDB.Tbl_FMBank_BankAccounts where a.BankAccntID == model.BankAccntList.BankAccntID select a).FirstOrDefault();
                    if (checkbankaccnt != null)
                    {
                        if (bankTypii.AccntNo == accntnumber && bankTypii.BankAccntID == model.BankAccntList.BankAccntID)
                        {
                            isExist = "justUpdate";
                        }
                        else
                        {
                            isExist = "true";
                        }
                    }
                    else if (checkbankaccnt == null)
                    {
                        isExist = "justUpdate";
                    }

                    if (isExist == "justUpdate")
                    {
                        bankTypii.AccntName = accntname;
                        bankTypii.AccntNo = accntnumber;
                        bankTypii.BankID = bankid;
                        bankTypii.FundID = model.BankAccntList.FundID;
                        bankTypii.GAID = model.BankAccntList.GAID;
                        bankTypii.AccntTypeID = model.BankAccntList.AccntTypeID;
                        bankTypii.BankAccntID = model.BankAccntList.BankAccntID;
                        BOSSDB.Entry(bankTypii);
                        BOSSDB.SaveChanges();
                    }
                }
            }
            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new { isExist = isExist }
            };
        }
        public ActionResult DeleteBankAccnt(int PrimaryID)
        {
            Tbl_FMBank_BankAccounts bankAccnt = (from a in BOSSDB.Tbl_FMBank_BankAccounts where a.BankAccntID == PrimaryID select a).FirstOrDefault();
            var confirmDelete = "";
            if (bankAccnt != null)
            {
                confirmDelete = "false";
            }
            var result = new { confirmDelete = confirmDelete };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConfirmDeleteBankAccnt(int PrimaryID)
        {
            Tbl_FMBank_BankAccounts Bacctype = (from a in BOSSDB.Tbl_FMBank_BankAccounts where a.BankAccntID == PrimaryID select a).FirstOrDefault();
            BOSSDB.Tbl_FMBank_BankAccounts.Remove(Bacctype);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}