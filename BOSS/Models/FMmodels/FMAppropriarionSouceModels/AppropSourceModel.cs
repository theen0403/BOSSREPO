using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BOSS.Models.FMmodels.FMAppropriarionSouceModels
{
    public class AppropSourceModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public AppropSourceModel()
        {
            getAppropSourceList = new List<AppropSourceList>();
            AppropSourceList = new AppropSourceList();
            FundSourceList = new List<SelectListItem>();
        }
        public int ActionID { get; set; }
        public List<AppropSourceList> getAppropSourceList { get; set; }
        public AppropSourceList AppropSourceList { get; set; }

        public int AppropSourceTypeID { get; set; }
        public int FundSourceID { get; set; }
        public int BudgetYearID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> AppropriationSourceTypeList
        {
            get
            {
                List<FMApprop_AppropriationSourceType> AppropriationSourceTypeLists = BOSSDB.FMApprop_AppropriationSourceType.ToList();
                AppropriationSourceTypeLists = (from li in AppropriationSourceTypeLists orderby li.AppropSourceTypeTitle select li).ToList();
                return new System.Web.Mvc.SelectList(AppropriationSourceTypeLists, "AppropSourceTypeID", "AppropSourceTypeTitle");
            }
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> FundSourceList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> BudgetYearList
        {
            get
            {
                List<FMApprop_BudgetYear> BudgetYearLists = BOSSDB.FMApprop_BudgetYear.ToList();
                return new System.Web.Mvc.SelectList(BudgetYearLists, "BudgetYearID", "BudgetYearTitle");
            }
        }
    }
    public class AppropSourceList
    {
        public int AppropriationID { get; set; }
        public string Description { get; set; }
        public string AppropSourceTypeTitle { get; set; }
        public string FundSourceTitle { get; set; }
        public string BudgetYearTitle { get; set; }
    }
}