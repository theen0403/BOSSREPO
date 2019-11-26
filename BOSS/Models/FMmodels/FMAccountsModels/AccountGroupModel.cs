using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BOSS.Models.FMmodels.FMAccountsModels
{
    public class AccountGroupModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public AccountGroupModel()
        {
            getAccountGroupColumns = new Tbl_FMCOA_AccountGroup();
            getAccountGroupList = new List<AccountGroupList>();
            allotModel = new AllotmentClassModel();
        }
        public Tbl_FMCOA_AccountGroup getAccountGroupColumns { get; set; }
        public List<AccountGroupList> getAccountGroupList { get; set; }
        public AllotmentClassModel allotModel { get; set; }
        public int AGID { get; set; }
        public int allotclasssTempID { get; set; }
        public int RevTempID { get; set; }
        public string AGTitle { get; set; }
        public string AGCode { get; set; }
        //public int RevIDdynamic { get; set; }
        public int AllotmentClassID { get; set; }
       //public IEnumerable<System.Web.Mvc.SelectListItem> RevYearDropDownListAG { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> AllotClasslist { get; set; }

    }
    public class AccountGroupList
    {
        public int AGID { get; set; }
        public string AGTitle { get; set; }
        public string AGCode { get; set; }
        public string RevYear { get; set; }
        public string AllotmentClassTitle { get; set; }
    }
}