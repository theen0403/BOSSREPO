using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Models.FMmodels.FMTaxModels
{
    public class TaxModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public TaxModel()
        {
            getTaxList = new List<TaxList>();
            TaxList = new TaxList();
        }
        public int ActionID { get; set; }
        public int GAID { get; set; }
        public List<TaxList> getTaxList { get; set; }
        public TaxList TaxList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> GenAccntList
        {
            get
            {
                List<Tbl_FMCOA_GeneralAccount> tbl_FMCOA_GeneralAccounts = BOSSDB.Tbl_FMCOA_GeneralAccount.ToList();
                return new System.Web.Mvc.SelectList(tbl_FMCOA_GeneralAccounts, "GAID", "GATitle");
            }
        }
    }
    public class TaxList
    {
        public int TaxID { get; set; }
        [Required(ErrorMessage = "Please enter Description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please enter Short Description")]
        public string ShortDescrption { get; set; }
        public bool isUsed { get; set; }
        [Required(ErrorMessage = "Please enter Percentage")]
        public string Percentage {get;set; }
        [Required(ErrorMessage = "Please enter Base")]
        public string BaseTax { get; set; }
        public string GATitle { get; set; }

    }
}