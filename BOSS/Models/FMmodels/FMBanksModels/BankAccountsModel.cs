using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace BOSS.Models.FMmodels.FMBanksModels
{
    public class BankAccountsModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public BankAccountsModel()
        {
            getBankAccntList = new List<BankAccntList>();
            BankAccntList = new BankAccntList();
        }
        public int ActionID { get; set; }
        public List<BankAccntList> getBankAccntList { get; set; }
        public BankAccntList BankAccntList { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> BankList
        {
            get
            {
                List<Tbl_FMBank_Banks> banks = BOSSDB.Tbl_FMBank_Banks.ToList();
                return new System.Web.Mvc.SelectList(banks, "BankID", "BankTitle");
            }
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> GenAccntList
        {
            get
            {
                List<Tbl_FMCOA_GeneralAccount> generalAccounts = BOSSDB.Tbl_FMCOA_GeneralAccount.ToList();
                return new System.Web.Mvc.SelectList(generalAccounts, "GAID", "GATitle");
            }
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> AccntTypeList
        {
            get
            {
                List<Tbl_FMBank_AccountType> accountTypes = BOSSDB.Tbl_FMBank_AccountType.ToList();
                return new System.Web.Mvc.SelectList(accountTypes, "AccntTypeID", "AccntType");
            }
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> FundtypeList
        {
            get
            {
                List<Tbl_FMFund_Fund> fund_Funds = BOSSDB.Tbl_FMFund_Fund.ToList();
                return new System.Web.Mvc.SelectList(fund_Funds, "FundID", "FundTitle");
            }
        }
    }
    public class BankAccntList
    {
        public int BankAccntID { get; set; }
        public string AccntNo { get; set; }
        public string AccntName { get; set; }
        public string BankTitle { get; set; }
        public string GATitle { get; set; }
        public string GACode { get; set; }
        public string concatCode { get; set; }
        public string FundTitle { get; set; }
        public string AccntType { get; set; }

        public int BankID { get; set; }
        public int FundID { get; set;}
        public int GAID { get; set; }
        public int AccntTypeID { get; set; }
    }
}