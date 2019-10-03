using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace BOSS.Models.FMmodels.FMAccountsModels
{
    public class AllotmentClassModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public AllotmentClassModel()
        {
            getAllotmentClassColumns = new Tbl_FMAllotmentClass();
            getAllotmentClassList = new List<AllotmentClassList>();
        }
        public Tbl_FMAllotmentClass getAllotmentClassColumns { get; set; }
        public List<AllotmentClassList> getAllotmentClassList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> AllotClassDropDownList
        {
            get
            {
                List<Tbl_FMAllotmentClass> allotmentClasses = BOSSDB.Tbl_FMAllotmentClass.ToList();
                return new System.Web.Mvc.SelectList(AllotClassDropDownList, "AllotmentID", "AllotmentClassTitle");
            }
        }
        public int AllotmentClassID { get; set; }
        public string AllotmentClassTitle { get; set; }
        public int RevID { get; set; }
    }
    public class AllotmentClassList
    {
        public int AllotmentClassID { get; set; }
        public string AllotmentClassTitle { get; set; }
        public string RevYEar { get; set; }
    }

}