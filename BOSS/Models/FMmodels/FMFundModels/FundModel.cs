using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BOSS.Models.FMmodels.FMFundModels
{
    public class FundModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public FundModel()
        {
            getFundList = new List<FundList>();
            getFundColumns = new Tbl_FMFund();
        }
        public List<FundList> getFundList { get; set; }
        public Tbl_FMFund getFundColumns { get; set; }
        //Fields
        public int FundID { get; set; }
        public string FundTitle { get; set; }
        public string FundCode { get; set; }
    }
    //Fields From DB table
    public class FundList
    {
        public int FundID { get; set; }
        public string FundTitle { get; set; }
        public string FundCode { get; set; }
    }
}