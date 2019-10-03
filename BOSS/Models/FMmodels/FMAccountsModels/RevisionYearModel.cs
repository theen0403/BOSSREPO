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
            getRevYearColumns = new Tbl_FMRevisionYear();
            getRevisionYearList = new List<RevisionList>();
        }
        public Tbl_FMRevisionYear getRevYearColumns { get; set; }
        public List<RevisionList> getRevisionYearList { get; set; }
        public int RevID { get; set; }
        public int RevYEar { get; set; }
        public bool isUsed { get; set; }
        public string Remarks { get; set; }
    }
    public class RevisionList
    {
        public int RevID { get; set; }
        public int RevYEar { get; set; }
        public int isUsed { get; set; }
        public string Remarks { get; set; }
    }
}