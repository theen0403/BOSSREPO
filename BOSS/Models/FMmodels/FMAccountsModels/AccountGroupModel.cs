using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Models.FMmodels.FMAccountsModels
{
    public class AccountGroupModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public AccountGroupModel()
        {
            getAccountGroupList = new List<AccountGrpList>();
            AllotClassList = new List<SelectListItem>();
            AccountGrpList = new AccountGrpList();
        }
        public int ActionID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> RevYearList { get; set; }
        public List<SelectListItem> AllotClassList { get; set; }
        public List<AccountGrpList> getAccountGroupList { get; set; }
        public AccountGrpList AccountGrpList { get; set; }

    }
    public class AccountGrpList
    {
        public int AGID { get; set; }
        [Required(ErrorMessage = "Please enter Account Group Title")]
        public string AGTitle { get; set; }
        [Required(ErrorMessage = "Please enter Account Group Code")]
        public string AGCode { get; set; }
        public string RevYear { get; set; }
        public string AllotmentClassTitle { get; set; }

        public int RevID { get; set; }
        public int AllotmentClassID { get;set;}
    }
}