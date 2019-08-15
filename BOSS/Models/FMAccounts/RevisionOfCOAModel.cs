using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BOSS.Models.FMAccounts
{
    public class RevisionOfCOAModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public RevisionOfCOAModel()
        {
            getRevCOAColumns = new Tbl_FMRevisionOfCOA();
            getRevCOAColumns2 = new Tbl_FMRevisionOfCOA();
            getRevCOAList = new List<RevisionCOAList>();
        }
        public Tbl_FMRevisionOfCOA getRevCOAColumns { get; set; }
        public Tbl_FMRevisionOfCOA getRevCOAColumns2 { get; set; }
        public List<RevisionCOAList> getRevCOAList { get; set; }
        public int RevID { get; set; }
        public int RevYEar { get; set; }
        public bool isUsed { get; set; }
        public string Remarks { get; set; }

    }
    public class RevisionCOAList
    {
        public int RevID { get; set; }
        public int RevYEar { get; set; }
        public int isUsed { get; set; }
        public string Remarks { get; set; }
    }



}