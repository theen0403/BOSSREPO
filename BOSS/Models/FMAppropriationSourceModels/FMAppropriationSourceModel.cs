using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BOSS.Models.FMAppropriationSourceModels
{

    public class FMAppropriationSourceModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();

        public FMAppropriationSourceModel()
        {
            getAppropList = new List<Tbl_FMAppropriationSource>();
            getAppropriationSourceList = new List<AppropriationSourceList>();
            getAppropriationSourceColumns2 = new Tbl_FMAppropriationSource();
        }
        public Tbl_FMAppropriationSource getAppropriationSourceColumns { get; set; }
        public Tbl_FMAppropriationSource getAppropriationSourceColumns2 { get; set; }
        public List<AppropriationSourceList> getAppropriationSourceList { get; set; }
        public int FundSourceIDValue { get; set; }
        public int FundSourceID { get; set; }
        public int FundSourceID2 { get; set; }
        public int FundSourceIDHidden { get; set; }
        public int ApproIDHidden { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> FundSourceList { get; set; }
        public int BudgetYearID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> BudgetYearList
        {
            get
            {
                List<BudgetYear> BudgetYearLists = BOSSDB.BudgetYears.ToList();
                return new System.Web.Mvc.SelectList(BudgetYearLists, "BudgetYearID", "BudgetYearTitle");
            }
        }
        public int AppropriationID { get; set; }
        public int AppropriationSourceID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> AppropriationSourceTypeList
        {
            get
            {
                List<AppropriationSourceType> AppropriationSourceTypeLists = BOSSDB.AppropriationSourceTypes.ToList();
                return new System.Web.Mvc.SelectList(AppropriationSourceTypeLists, "AppropriationSourceID", "AppropriationSourceType1");
            }
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> DynamicAppropriationSourceTypeList { get; set; }
        public IEnumerable<BOSS.Models.Tbl_FMAppropriationSource> getAppropList { get; set; }
        public int DynamicAppropriationSourceID { get; set; }
        
    }
    public class AppropriationSourceList
    {
        public int AppropriationID { get; set; }
        public string Description { get; set; }
        public string AppropriationSourceType { get; set; }
    }
}
   