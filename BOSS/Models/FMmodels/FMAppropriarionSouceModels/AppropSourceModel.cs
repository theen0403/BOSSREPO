using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace BOSS.Models.FMmodels.FMAppropriarionSouceModels
{
    public class AppropSourceModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public AppropSourceModel()
        {
            getAppropSourceList = new List<AppropriationSourceList>();
            getAppropriationSourceColumns = new Tbl_FMAppropriationSource();
        }
        public Tbl_FMAppropriationSource getAppropriationSourceColumns { get; set; }
        public List<AppropriationSourceList> getAppropSourceList { get; set; }
        public int AppropriationID { get; set; }
        public int FundSourceID { get; set; }
        public string Description { get; set; }
        public int FundSourceIDHidden { get; set; }
        public int ApproIDHidden { get; set; }
        public int AppropSourceTypeID { get; set; }
        public int BudgetYearID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> FundSourceList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> BudgetYearList
        {
            get
            {
                List<BudgetYear> BudgetYearLists = BOSSDB.BudgetYears.ToList();
                return new System.Web.Mvc.SelectList(BudgetYearLists, "BudgetYearID", "BudgetYearTitle");
            }
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> AppropriationSourceTypeList
        {
            get
            {
                List<AppropriationSourceType> AppropriationSourceTypeLists = BOSSDB.AppropriationSourceTypes.ToList();
                return new System.Web.Mvc.SelectList(AppropriationSourceTypeLists, "AppropSourceTypeID", "AppropSourceTypeTitle");
            }
        }
    }
    public class AppropriationSourceList
    {
        public int AppropriationID { get; set; }
        public string Description { get; set; }
        public string AppropSourceTypeTitle { get; set; }
        public string FundSourceTitle { get; set; }
        public string BudgetYearTitle { get; set; }
    }
}