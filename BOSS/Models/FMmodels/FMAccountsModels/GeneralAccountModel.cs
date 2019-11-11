using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BOSS.Models.FMmodels.FMAccountsModels
{
    public class GeneralAccountModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public GeneralAccountModel()
        {
            getGenAcctList = new List<GeneralAccountList>();
            getGenAcctColumns = new Tbl_FMGeneralAccount();
        }
        public List<GeneralAccountList> getGenAcctList { get; set; }
        public Tbl_FMGeneralAccount getGenAcctColumns { get; set; }

        public int RevID { get; set; }
        public int AllotmentID { get; set; }
        public int AGID { get; set; }
        public int MAGID { get; set; }
        public int SMAGID { get; set; }
        public int GAID { get; set; }
        public string GATitle { get; set; }
        public string GACode { get; set; }
        public int isSelected { get; set; }



        public bool isReserve { get; set; }
        public int ReservePercent { get; set; }
        public bool isFullRelease { get; set; }
        public bool isContinuing { get; set; }
        public bool isOBRCashAdvance { get; set; }
        public int BalanceID { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> SubMajorAccountGrpList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> GeneralAccountList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> NormalBalanceList
        {
            get
            {
                List<Tbl_FMBalance> BalanceList = BOSSDB.Tbl_FMBalance.ToList();
                return new System.Web.Mvc.SelectList(BalanceList, "BalanceID", "BalanceTitle");
            }
        }
    }
    public class GeneralAccountList
    {
        public int GAID { get; set; }
        public string GACode { get; set; }
        public string GATitle { get; set; }
        public int isReserve { get; set; }
        public int ReservePercent { get; set; }
        public int isFullRelease { get; set; }
        public int isContinuing { get; set; }
        public int isOBRCashAdvance { get; set; }
        public int BalanceID { get; set; }

    }
}