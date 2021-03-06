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
    
    public partial class Tbl_FMRes_Department
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_FMRes_Department()
        {
            this.BPAIP_PPAperOffice = new HashSet<BPAIP_PPAperOffice>();
            this.Tbl_FMPayee = new HashSet<Tbl_FMPayee>();
            this.Tbl_FMRes_Function = new HashSet<Tbl_FMRes_Function>();
            this.Tbl_FMRes_Section = new HashSet<Tbl_FMRes_Section>();
        }
    
        public int DeptID { get; set; }
        public string DeptTitle { get; set; }
        public string DeptAbbrv { get; set; }
        public string RCcode { get; set; }
        public Nullable<int> FundID { get; set; }
        public Nullable<int> SectorID { get; set; }
        public Nullable<int> SubSectorID { get; set; }
        public Nullable<int> OfficeTypeID { get; set; }
        public string DeptOfficeCode { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BPAIP_PPAperOffice> BPAIP_PPAperOffice { get; set; }
        public virtual Tbl_FMFund_Fund Tbl_FMFund_Fund { get; set; }
        public virtual Tbl_FMOfficeType Tbl_FMOfficeType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_FMPayee> Tbl_FMPayee { get; set; }
        public virtual Tbl_FMSector_Sector Tbl_FMSector_Sector { get; set; }
        public virtual Tbl_FMSector_SubSector Tbl_FMSector_SubSector { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_FMRes_Function> Tbl_FMRes_Function { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_FMRes_Section> Tbl_FMRes_Section { get; set; }
    }
}
