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
            getAllotmentClassList = new List<AllotmentClassList>();
            AllotmentClassList = new AllotmentClassList();
        }
        public int ActionID { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> RevYearList { get; set; }
        public List<AllotmentClassList> getAllotmentClassList { get; set; }
        public AllotmentClassList AllotmentClassList { get; set; }
    }
    public class AllotmentClassList
    {
        public int AllotmentClassID { get; set; }

        [Required(ErrorMessage = "Please enter Allotment Class Title")]
        public string AllotmentClassTitle { get; set; }

        [Required(ErrorMessage = "Please select Revision Year")]
        public int RevID { get; set; }

        public string RevYear { get; set; }
    }

}