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
    
    public partial class Tbl_FMSubFund
    {
        public int SubFundID { get; set; }
        public string SubFundTitle { get; set; }
        public Nullable<int> FundID { get; set; }
    
        public virtual Tbl_FMFund Tbl_FMFund { get; set; }
    }
}