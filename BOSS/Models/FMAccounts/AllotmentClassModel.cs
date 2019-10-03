using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BOSS.Models.FMAccounts
{
    public class AllotmentClassModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public AllotmentClassModel()
        {
            getRevCOAColumns = new Tbl_FMRevisionYear();
            getRevCOAColumns2 = new Tbl_FMRevisionYear();
           // getRevCOAList = new List<RevisionCOAList>();
        }
        public Tbl_FMRevisionYear getRevCOAColumns { get; set; }
        public Tbl_FMRevisionYear getRevCOAColumns2 { get; set; }
       // public List<RevisionCOAList> getRevCOAList { get; set; }
    }
}