using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BOSS.Models.FMAccounts
{
    public class GeneralAccountsModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();

        public GeneralAccountsModel()
        {
            getGenAcctList = new List<GeneralAccountList>();
            getGenAcctColumns = new Tbl_FMGeneralAccount();
            getGenAcctColumns2 = new Tbl_FMGeneralAccount();
        }
        public List<GeneralAccountList> getGenAcctList { get; set; }
        public Tbl_FMGeneralAccount getGenAcctColumns { get; set; }
        public Tbl_FMGeneralAccount getGenAcctColumns2 { get; set; }
        public int AllotmentID { get; set; }
        public int AllotmentID2 { get; set; }
        public string AllotmentClassTitle { get; set; }
        public bool isReserve { get; set; }
        public int ReservePercent { get; set; }
        public bool isFullRelease { get; set; }
        public bool isContinuing { get; set; }
        public bool isOBRCashAdvance { get; set; }
        public int GenAccountID { get; set; }
        public int isReserve2 { get; set; }
        public int allotTempID { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> AllotmentClassList
        {
            get
            {
                List<Tbl_FMAllotmentClass> allotmentClasses = BOSSDB.Tbl_FMAllotmentClass.ToList();
                return new System.Web.Mvc.SelectList(allotmentClasses, "AllotmentID", "AllotmentClassTitle");
            }
        }
        //PIN
        public int PinID { get; set; }
        public string Pin_Acct { get; set; }
        public string newPIN { get; set; }
        public string confirm_newPIN { get; set; }
        public string pintemp { get; set; }
    }
    public class GeneralAccountList
    {
        public int GenAccountID { get; set; }
        public string GenAccountCode { get; set; }
        public string GenAccountTitle { get; set; }
        public int isReserve { get; set; }
        public int ReservePercent { get; set; }
        public int isFullRelease { get; set; }
        public int isContinuing { get; set; }
        public int isOBRCashAdvance { get; set; }

    }
}