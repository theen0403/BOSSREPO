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
    
    public partial class Tbl_FMAppropriationSource
    {
        public int AppropriationID { get; set; }
        public string Description { get; set; }
        public Nullable<int> FundSourceID { get; set; }
        public Nullable<int> BudgetYearID { get; set; }
    
        public virtual BudgetYear BudgetYear { get; set; }
        public virtual FundSource FundSource { get; set; }
    }
}
