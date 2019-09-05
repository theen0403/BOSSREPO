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
            getRevCOAColumns = new Tbl_FMRevisionOfCOA();
            getRevCOAColumns2 = new Tbl_FMRevisionOfCOA();
            getRevCOAList = new List<RevisionCOAList>();
        }
        public Tbl_FMRevisionOfCOA getRevCOAColumns { get; set; }
        public Tbl_FMRevisionOfCOA getRevCOAColumns2 { get; set; }
        public List<RevisionCOAList> getRevCOAList { get; set; }
    }
}