using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BOSS.Models.FMAccounts
{
    public class SubAccountsModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public SubAccountsModel()
        {
            getSubAcctList = new List<SubAccountList>();
            getSubAcctColumns = new Tbl_FMSubAccount();
            getSubAcctColumns2 = new Tbl_FMSubAccount();
        }
        public Tbl_FMSubAccount getSubAcctColumns { get; set; }
        public Tbl_FMSubAccount getSubAcctColumns2 { get; set; }
        public List<SubAccountList> getSubAcctList { get; set; }
        public int AllotmentID { get; set; }
        public int GenAccountID { get; set; }
        public int SubAccountID { get; set; }
        public string GenAccountCode { get; set; }
        public string GenAccountTitle { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> AllotmentClassList
        {
            get
            {
                List<Tbl_FMAllotmentClass> allotmentClasses = BOSSDB.Tbl_FMAllotmentClass.ToList();
                return new System.Web.Mvc.SelectList(allotmentClasses, "AllotmentID", "AllotmentClassTitle");
            }
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> GeneralAccntList { get; set; }
        public bool isReserve { get; set; }
        public int ReservePercent { get; set; }
        public bool isFullRelease { get; set; }
        public bool isContinuing { get; set; }
        public bool isOBRCashAdvance { get; set; }

    }
    public class SubAccountList
    {
        public int SubAccountID { get; set; }
        public string SubAccountCode { get; set; }
        public string SubAccountTitle { get; set; }
        public int isReserve { get; set; }
        public int ReservePercent { get; set; }
        public int isFullRelease { get; set; }
        public int isContinuing { get; set; }
        public int isOBRCashAdvance { get; set; }
        public int GenAccountID { get; set; }

    }


}