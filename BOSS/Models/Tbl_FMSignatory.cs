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
    
    public partial class Tbl_FMSignatory
    {
        public int SignatoryID { get; set; }
        public string SignatoryName { get; set; }
        public Nullable<int> PositionID { get; set; }
        public Nullable<int> FunctionID { get; set; }
        public Nullable<bool> isHead { get; set; }
    
        public virtual Tbl_FMFunction Tbl_FMFunction { get; set; }
        public virtual Tbl_FMPosition Tbl_FMPosition { get; set; }
    }
}