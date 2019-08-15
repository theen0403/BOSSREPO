﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class BOSSEFConnectionString : DbContext
    {
        public BOSSEFConnectionString()
            : base("name=BOSSEFConnectionString")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AllotmentClass> AllotmentClasses { get; set; }
        public virtual DbSet<AppropriationSourceType> AppropriationSourceTypes { get; set; }
        public virtual DbSet<BudgetYear> BudgetYears { get; set; }
        public virtual DbSet<FundSource> FundSources { get; set; }
        public virtual DbSet<FundType> FundTypes { get; set; }
        public virtual DbSet<Level1Modules> Level1Modules { get; set; }
        public virtual DbSet<Level2Modules> Level2Modules { get; set; }
        public virtual DbSet<Level3Modules> Level3Modules { get; set; }
        public virtual DbSet<ParentModule> ParentModules { get; set; }
        public virtual DbSet<PersonalInformation> PersonalInformations { get; set; }
        public virtual DbSet<PIN_Accounts> PIN_Accounts { get; set; }
        public virtual DbSet<Sector> Sectors { get; set; }
        public virtual DbSet<SubSector> SubSectors { get; set; }
        public virtual DbSet<Tbl_FMAppropriationSource> Tbl_FMAppropriationSource { get; set; }
        public virtual DbSet<Tbl_FMDepartment> Tbl_FMDepartment { get; set; }
        public virtual DbSet<Tbl_FMFunction> Tbl_FMFunction { get; set; }
        public virtual DbSet<Tbl_FMGeneralAccount> Tbl_FMGeneralAccount { get; set; }
        public virtual DbSet<Tbl_FMOfficeSection> Tbl_FMOfficeSection { get; set; }
        public virtual DbSet<Tbl_FMRevisionOfCOA> Tbl_FMRevisionOfCOA { get; set; }
        public virtual DbSet<Tbl_FMSubAccount> Tbl_FMSubAccount { get; set; }
    
        public virtual int SP_AppropriationSource(string sQLStatement)
        {
            var sQLStatementParameter = sQLStatement != null ?
                new ObjectParameter("SQLStatement", sQLStatement) :
                new ObjectParameter("SQLStatement", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_AppropriationSource", sQLStatementParameter);
        }
    
        public virtual int SP_FMResponsibility(string sQLStatement)
        {
            var sQLStatementParameter = sQLStatement != null ?
                new ObjectParameter("SQLStatement", sQLStatement) :
                new ObjectParameter("SQLStatement", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_FMResponsibility", sQLStatementParameter);
        }
    
        public virtual int SP_FMAccounts(string sQLStatement)
        {
            var sQLStatementParameter = sQLStatement != null ?
                new ObjectParameter("SQLStatement", sQLStatement) :
                new ObjectParameter("SQLStatement", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_FMAccounts", sQLStatementParameter);
        }
    }
}
