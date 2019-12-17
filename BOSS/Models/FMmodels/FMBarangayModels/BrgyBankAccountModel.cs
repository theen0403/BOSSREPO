using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace BOSS.Models.FMmodels.FMBarangayModels
{
    public class BrgyBankAccountModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public BrgyBankAccountModel()
        {
            getBarangayBankAccountList = new List<BarangayBankAccountList>();
            BarangayBankAccountList = new BarangayBankAccountList();
        }
        public int ActionID { get; set; }
        public List<BarangayBankAccountList> getBarangayBankAccountList { get; set; }
        public BarangayBankAccountList BarangayBankAccountList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> BrgyList
        {
            get
            {
                List<Tbl_FMBrgy_Barangay> brgy = BOSSDB.Tbl_FMBrgy_Barangay.ToList();
                return new System.Web.Mvc.SelectList(brgy, "BrgyID", "BrgyName");
            }
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> BankList
        {
            get
            {
                List<Tbl_FMBank_Banks> banks = BOSSDB.Tbl_FMBank_Banks.ToList();
                return new System.Web.Mvc.SelectList(banks, "BankID", "BankTitle");
            }
        }
    }
    public class BarangayBankAccountList
    {
        public int BrgyBankAccntID { get; set; }
        public int BrgyID { get; set; }
        public int BankID { get; set; }
        public int BankAccntID { get; set; }
        public string BrgyName { get; set; }
        public string BankName { get; set; }
        public string AccntNo { get; set; }
    }
}