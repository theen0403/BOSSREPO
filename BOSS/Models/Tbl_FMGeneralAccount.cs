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
    
    public partial class Tbl_FMGeneralAccount
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_FMGeneralAccount()
        {
            this.Tbl_FMSubAccount = new HashSet<Tbl_FMSubAccount>();
        }
    
        public int GenAccountID { get; set; }
        public string GenAccountCode { get; set; }
        public string GenAccountTitle { get; set; }
        public Nullable<bool> isReserve { get; set; }
        public Nullable<int> ReservePercent { get; set; }
        public Nullable<bool> isFullRelease { get; set; }
        public Nullable<bool> isContinuing { get; set; }
        public Nullable<bool> isOBRCashAdvance { get; set; }
        public Nullable<int> AllotmentID { get; set; }
    
        public virtual AllotmentClass AllotmentClass { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_FMSubAccount> Tbl_FMSubAccount { get; set; }
    }
}