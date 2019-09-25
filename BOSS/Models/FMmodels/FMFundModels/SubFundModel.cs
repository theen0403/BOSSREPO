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
            getSubFundColumns = new SubFund();
        }
        public List<SubFundList> getSubFundList { get; set; }
        public SubFund getSubFundColumns { get; set; }

        public int SubFundID { get; set; }
        public int FundID { get; set; }
        public string SubFundTitle { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> FundSelectionList
        {
            get
            {
                List<Fund> FundSelectionList = BOSSDB.Funds.ToList();
                return new System.Web.Mvc.SelectList(FundSelectionList, "FundID", "FundTitle");
            }
        }

       
    }
    public class SubFundList
    {
        public int SubFundID { get; set; }
        public int FundID { get; set; }
        public string FundTitle { get; set; }
        public string SubFundTitle { get; set; }
    }
}