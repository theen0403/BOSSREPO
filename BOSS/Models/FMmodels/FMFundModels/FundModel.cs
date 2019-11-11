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
            FundList = new FundList();
        }
        public int ActionID { get; set; }
        public List<FundList> getFundList { get; set; }
        public FundList FundList { get; set; }
 
    }
    //Fields From DB table
    public class FundList
    {
        public int FundID { get; set; }
        [Required(ErrorMessage = "Please enter Fund Title")]
        public string FundTitle { get; set; }
        [Required(ErrorMessage = "Please enter Fund Code")]
        public string FundCode { get; set; }
    }
}