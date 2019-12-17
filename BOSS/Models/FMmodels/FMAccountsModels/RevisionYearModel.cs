using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace BOSS.Models.FMmodels.FMAccountsModels
{
    public class RevisionYearModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public RevisionYearModel()
        {
            getRevisionList = new List<RevisionList>();
            RevisionList = new RevisionList();
        }
        public List<RevisionList> getRevisionList { get; set; }
        public RevisionList RevisionList { get; set; }
        public int ActionID { get; set; }
    }
    public class RevisionList
    {
        public int RevID { get; set; }
        [Required(ErrorMessage = "Please enter Revision Year")]
        public string RevYear { get; set; }
        public bool isUsed { get; set; }
        public string Remarks { get; set; }
    }
}