using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BOSS.Models.FMmodels.FMAppropriarionSouceModels
{
    public class FundSourceModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public FundSourceModel()
        {
            getFundSourceList = new List<FundSourceList>();
            FundSourceList = new FundSourceList();
        }
        public List<FundSourceList> getFundSourceList { get; set; }
        public FundSourceList FundSourceList { get; set; }
        public int ActionID { get; set; }
        public int AppropSourceTypeID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> AppropriationSourceTypeList
        {
            get
            {
                List<FMApprop_AppropriationSourceType> AppropriationSourceTypeLists = BOSSDB.FMApprop_AppropriationSourceType.ToList();
                return new System.Web.Mvc.SelectList(AppropriationSourceTypeLists, "AppropSourceTypeID", "AppropSourceTypeTitle");
            }
        }
    }
    public class FundSourceList
    {
        public int FundSourceID { get; set; }
        [Required(ErrorMessage = "Please enter Fund Source Title")]
        public string FundSourceTitle { get; set; }
        public string AppropSourceTypeTitle { get; set; }
    }

}