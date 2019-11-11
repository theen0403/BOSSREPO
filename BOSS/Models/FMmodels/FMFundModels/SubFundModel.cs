using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BOSS.Models.FMmodels.FMFundModels
{
    public class SubFundModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public SubFundModel()
        {
            getSubFundList = new List<SubFundList>();
            //getSubFundColumns = new Tbl_FMSubFund();
            SubFundList = new SubFundList();
        }
        public int ActionID { get; set; }
        public List<SubFundList> getSubFundList { get; set; }
        public SubFundList SubFundList { get; set; }
        //public Tbl_FMSubFund getSubFundColumns { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> FundSelectionList
        {
            get
            {
                List<Tbl_FMFund> FundSelectionList = BOSSDB.Tbl_FMFund.ToList();
                return new System.Web.Mvc.SelectList(FundSelectionList, "FundID", "FundTitle");
            }
        }

       
    }
    public class SubFundList
    {
        public int SubFundID { get; set; }
        public int FundID { get; set; }
        public string FundTitle { get; set; }
        [Required(ErrorMessage = "Please enter SubFund Title")]
        public string SubFundTitle { get; set; }
    }
}