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
            getFundSourceColumns = new Tbl_FMFundSource();
        }
        public List<FundSourceList> getFundSourceList { get; set; }
        public Tbl_FMFundSource getFundSourceColumns { get; set; }
        public int FundSourceID { get; set; }
        public string FundSourceTitle { get; set; }
        public string AppropSourceTypeTitle { get; set; }
        public int AppropSourceTypeID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> AppropriationSourceTypeList
        {
            get
            {
                List<AppropriationSourceType> AppropriationSourceTypeLists = BOSSDB.AppropriationSourceTypes.ToList();
                return new System.Web.Mvc.SelectList(AppropriationSourceTypeLists, "AppropSourceTypeID", "AppropSourceTypeTitle");
            }
        }
    }
    public class FundSourceList
    {
        public int FundSourceID { get; set; }
        public string FundSourceTitle { get; set; }
        public string AppropSourceTypeTitle { get; set; }
    }

}