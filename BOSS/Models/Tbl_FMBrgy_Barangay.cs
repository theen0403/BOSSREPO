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
    
    public partial class Tbl_FMBrgy_Barangay
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_FMBrgy_Barangay()
        {
            this.Tbl_FMBrgy_BrgyBankAccount = new HashSet<Tbl_FMBrgy_BrgyBankAccount>();
        }
    
        public int BrgyID { get; set; }
        public string BrgyName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_FMBrgy_BrgyBankAccount> Tbl_FMBrgy_BrgyBankAccount { get; set; }
    }
}
