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
    
    public partial class PositionClassification
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PositionClassification()
        {
            this.Tbl_FMPosition = new HashSet<Tbl_FMPosition>();
        }
    
        public int PCID { get; set; }
        public string PCTitle { get; set; }
        public Nullable<int> PSID { get; set; }
    
        public virtual PositionStatu PositionStatu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_FMPosition> Tbl_FMPosition { get; set; }
    }
}