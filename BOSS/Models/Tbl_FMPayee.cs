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
    
    public partial class Tbl_FMPayee
    {
        public int PayeeID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public Nullable<int> DeptID { get; set; }
    
        public virtual Tbl_FMRes_Department Tbl_FMRes_Department { get; set; }
    }
}
