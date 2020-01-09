using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Models.FMmodels.FMAccountsModels
{
    public class SubsidiaryLedgerAccountModel
    {
        BOSSEFConnectionString GAASDB = new BOSSEFConnectionString();
        public SubsidiaryLedgerAccountModel()
        {
            getSLAccntList = new List<SLAccntList>();
            SLAccntList = new SLAccntList();

            SLClassTitleList = new List<SelectListItem>();
            SLDescTitle = new List<SelectListItem>();
        }
        public int ActionID2 { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> SLClassTitleList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> SLDescTitle { get; set; }
        public List<SLAccntList> getSLAccntList { get; set; }
        public SLAccntList SLAccntList { get; set; }
    }
    public class SLAccntList
    {
        public int SLAccntID { get; set; }

        [Required(ErrorMessage = "Please enter Subsudiary Ledger Account Title")]
        public string SLAccntTitle { get; set; }

        [Required(ErrorMessage = "Please enter Code")]
        public string SLAccntCode { get; set; }
        
        public int SLClassID { get; set; }
        
        public string SLCategory { get; set; }

        public int SLDescID { get; set; }


        //For Category Dropdown
        //Bank Account table
        public string AccntNo { get; set; }
        //Supplier table
        public string CompanyName { get; set; }
        //Tax table
        public string Description { get; set; }
        //Payee table
        public string Name { get; set; }

        public string SLClassTitle { get; set; }
        public string SLClassCode { get; set; }
        public string SLClassDesc { get; set; }
    }
}