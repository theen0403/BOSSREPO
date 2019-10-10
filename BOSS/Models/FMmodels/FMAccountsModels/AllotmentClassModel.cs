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
        public IEnumerable<System.Web.Mvc.SelectListItem> RevYearDropDownList { get; set; }
        public int AllotmentClassID { get; set; }
        public string AllotmentClassTitle { get; set; }
        public int RevID { get; set; }
        public int RevIDtemp { get; set; }
        public string RevYear { get; set; }
    }
    public class AllotmentClassList
    {
        public int AllotmentClassID { get; set; }
        public string AllotmentClassTitle { get; set; }
        public string RevYear { get; set; }
    }

}