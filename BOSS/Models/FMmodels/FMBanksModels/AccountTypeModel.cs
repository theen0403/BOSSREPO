using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace BOSS.Models.FMmodels.FMBanksModels
{
    public class AccountTypeModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public AccountTypeModel()
        {
            getAccntTypeList  = new List<AccntTypeList>();
            AccntTypeList = new AccntTypeList();
        }
        public int ActionID { get; set; }
        public List<AccntTypeList> getAccntTypeList { get; set; }
        public AccntTypeList AccntTypeList { get; set; }
    }
    public class AccntTypeList
    {
        public int AccntTypeID { get; set; }
        [Required(ErrorMessage = "Please enter Account Type")]
        public string AccntType { get; set; }
    }
}