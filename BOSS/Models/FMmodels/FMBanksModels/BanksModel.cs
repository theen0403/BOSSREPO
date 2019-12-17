using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace BOSS.Models.FMmodels.FMBanksModels
{
    public class BanksModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public BanksModel()
        {
            getBankList = new List<BankList>();
            BankList = new BankList();
        }
        public int ActionID { get; set; }
        public List<BankList> getBankList { get; set; }
        public BankList BankList { get; set; }
    }
    public class BankList
    {
        public int BankID { get; set; }
        [Required(ErrorMessage = "Please enter Bank Name")]
        public string BankTitle { get; set; }
        [Required(ErrorMessage = "Please enter Bank Code")]
        public string BankCode { get; set; }
        [Required(ErrorMessage = "Please enter Bank Address")]
        public string BankAddress { get; set; }
    }
}