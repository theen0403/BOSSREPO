//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BOSS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_FMTax
    {
        public int TaxID { get; set; }
        public string Description { get; set; }
        public string ShortDepscription { get; set; }
        public Nullable<bool> isUsed { get; set; }
        public string Percentage { get; set; }
        public string BaseTax { get; set; }
        public Nullable<int> GAID { get; set; }
    
        public virtual Tbl_FMCOA_GeneralAccount Tbl_FMCOA_GeneralAccount { get; set; }
    }
}
