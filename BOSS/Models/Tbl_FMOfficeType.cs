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
    
    public partial class Tbl_FMOfficeType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_FMOfficeType()
        {
            this.Tbl_FMRes_Department = new HashSet<Tbl_FMRes_Department>();
            this.Tbl_FMRes_Function = new HashSet<Tbl_FMRes_Function>();
        }
    
        public int OfficeTypeID { get; set; }
        public string OfficeTypeTitle { get; set; }
        public string OfficeTypeCode { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_FMRes_Department> Tbl_FMRes_Department { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_FMRes_Function> Tbl_FMRes_Function { get; set; }
    }
}
