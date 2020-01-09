using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Models.FMmodels.FMAccountsModels
{
    public class SubsidiaryLedgerClassModel
    {
        BOSSEFConnectionString GAASDB = new BOSSEFConnectionString();
        public SubsidiaryLedgerClassModel()
        {
            getSLClassList = new List<SLClassList>();
            SLClassList = new SLClassList();

            FundList = new List<SelectListItem>();
            GenAccntGrpList = new List<SelectListItem>();

        }
        public int ActionID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> SubClassTitleList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> SLDescTitle { get; set; }
        public List<SLClassList> getSLClassList { get; set; }
        public SLClassList SLClassList { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> FundList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> GenAccntGrpList { get; set; }

    }
    public class SLClassList
    {
        public int SLClassID { get; set; }

        [Required(ErrorMessage = "Please enter Subsidiary Ledger Class Title")]
        public string SLClassTitle { get; set; }
        [Required(ErrorMessage = "Please enter Subsidiary Ledger Code")]
        public string SLClassCode { get; set; }

        public int FundID { get; set; }
        public int GAID { get; set; }

        public string FundTitle { get; set; }
        public string GATitle { get; set; }
    }
}