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
        public virtual DbSet<FMApprop_AppropriationSourceType> FMApprop_AppropriationSourceType { get; set; }
        public virtual DbSet<FMApprop_BudgetYear> FMApprop_BudgetYear { get; set; }
        public virtual DbSet<Level1Modules> Level1Modules { get; set; }
        public virtual DbSet<Level2Modules> Level2Modules { get; set; }
        public virtual DbSet<Level3Modules> Level3Modules { get; set; }
        public virtual DbSet<ParentModule> ParentModules { get; set; }
        public virtual DbSet<PersonalInformation> PersonalInformations { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Tbl_FMApprop_AppropriationSource> Tbl_FMApprop_AppropriationSource { get; set; }
        public virtual DbSet<Tbl_FMApprop_FundSource> Tbl_FMApprop_FundSource { get; set; }
        public virtual DbSet<Tbl_FMBank_AccountType> Tbl_FMBank_AccountType { get; set; }
        public virtual DbSet<Tbl_FMBank_BankAccounts> Tbl_FMBank_BankAccounts { get; set; }
        public virtual DbSet<Tbl_FMBank_Banks> Tbl_FMBank_Banks { get; set; }
        public virtual DbSet<Tbl_FMBrgy_Barangay> Tbl_FMBrgy_Barangay { get; set; }
        public virtual DbSet<Tbl_FMBrgy_BrgyBankAccount> Tbl_FMBrgy_BrgyBankAccount { get; set; }
        public virtual DbSet<Tbl_FMBrgy_BrgyCollector> Tbl_FMBrgy_BrgyCollector { get; set; }
        public virtual DbSet<Tbl_FMCOA_AccountGroup> Tbl_FMCOA_AccountGroup { get; set; }
        public virtual DbSet<Tbl_FMCOA_AllotmentClass> Tbl_FMCOA_AllotmentClass { get; set; }
        public virtual DbSet<Tbl_FMCOA_GeneralAccount> Tbl_FMCOA_GeneralAccount { get; set; }
        public virtual DbSet<Tbl_FMCOA_MajorAccountGroup> Tbl_FMCOA_MajorAccountGroup { get; set; }
        public virtual DbSet<Tbl_FMCOA_RevisionYear> Tbl_FMCOA_RevisionYear { get; set; }
        public virtual DbSet<TBL_FMCOA_SubLedger_SLAccnt> TBL_FMCOA_SubLedger_SLAccnt { get; set; }
        public virtual DbSet<TBL_FMCOA_SubLedger_SLClass> TBL_FMCOA_SubLedger_SLClass { get; set; }
        public virtual DbSet<Tbl_FMCOA_SubMajorAccountGroup> Tbl_FMCOA_SubMajorAccountGroup { get; set; }
        public virtual DbSet<Tbl_FMFund_Fund> Tbl_FMFund_Fund { get; set; }
        public virtual DbSet<Tbl_FMFund_SubFund> Tbl_FMFund_SubFund { get; set; }
        public virtual DbSet<Tbl_FMOfficeType> Tbl_FMOfficeType { get; set; }
        public virtual DbSet<Tbl_FMPayee> Tbl_FMPayee { get; set; }
        public virtual DbSet<Tbl_FMPosition> Tbl_FMPosition { get; set; }
        public virtual DbSet<Tbl_FMRes_Department> Tbl_FMRes_Department { get; set; }
        public virtual DbSet<Tbl_FMRes_Function> Tbl_FMRes_Function { get; set; }
        public virtual DbSet<Tbl_FMRes_Section> Tbl_FMRes_Section { get; set; }
        public virtual DbSet<Tbl_FMSector_Sector> Tbl_FMSector_Sector { get; set; }
        public virtual DbSet<Tbl_FMSector_SubSector> Tbl_FMSector_SubSector { get; set; }
        public virtual DbSet<Tbl_FMSignatory> Tbl_FMSignatory { get; set; }
        public virtual DbSet<Tbl_FMSupplier> Tbl_FMSupplier { get; set; }
        public virtual DbSet<Tbl_FMTax> Tbl_FMTax { get; set; }
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int SP_AppropriationSource(string sQLStatement)
        {
            var sQLStatementParameter = sQLStatement != null ?
                new ObjectParameter("SQLStatement", sQLStatement) :
                new ObjectParameter("SQLStatement", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_AppropriationSource", sQLStatementParameter);
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int SP_FMAccounts(string sQLStatement)
        {
            var sQLStatementParameter = sQLStatement != null ?
                new ObjectParameter("SQLStatement", sQLStatement) :
                new ObjectParameter("SQLStatement", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_FMAccounts", sQLStatementParameter);
        }
    
        public virtual int SP_FMResponsibility(string sQLStatement)
        {
            var sQLStatementParameter = sQLStatement != null ?
                new ObjectParameter("SQLStatement", sQLStatement) :
                new ObjectParameter("SQLStatement", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_FMResponsibility", sQLStatementParameter);
        }
    
        public virtual int SP_Fund(string sQLStatement)
        {
            var sQLStatementParameter = sQLStatement != null ?
                new ObjectParameter("SQLStatement", sQLStatement) :
                new ObjectParameter("SQLStatement", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_Fund", sQLStatementParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int SP_OfficeType(string sQLStatement)
        {
            var sQLStatementParameter = sQLStatement != null ?
                new ObjectParameter("SQLStatement", sQLStatement) :
                new ObjectParameter("SQLStatement", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_OfficeType", sQLStatementParameter);
        }
    
        public virtual int SP_Payee(string sQLStatement)
        {
            var sQLStatementParameter = sQLStatement != null ?
                new ObjectParameter("SQLStatement", sQLStatement) :
                new ObjectParameter("SQLStatement", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_Payee", sQLStatementParameter);
        }
    
        public virtual int SP_PositionSP(string sQLStatement)
        {
            var sQLStatementParameter = sQLStatement != null ?
                new ObjectParameter("SQLStatement", sQLStatement) :
                new ObjectParameter("SQLStatement", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_PositionSP", sQLStatementParameter);
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    
        public virtual int SP_Sector(string sQLStatement)
        {
            var sQLStatementParameter = sQLStatement != null ?
                new ObjectParameter("SQLStatement", sQLStatement) :
                new ObjectParameter("SQLStatement", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_Sector", sQLStatementParameter);
        }
    
        public virtual int SP_Signatory(string sQLStatement)
        {
            var sQLStatementParameter = sQLStatement != null ?
                new ObjectParameter("SQLStatement", sQLStatement) :
                new ObjectParameter("SQLStatement", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_Signatory", sQLStatementParameter);
        }
    
        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    
        public virtual int SP_Banks(string sQLStatement)
        {
            var sQLStatementParameter = sQLStatement != null ?
                new ObjectParameter("SQLStatement", sQLStatement) :
                new ObjectParameter("SQLStatement", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_Banks", sQLStatementParameter);
        }
    
        public virtual int SP_Tax(string sQLStatement)
        {
            var sQLStatementParameter = sQLStatement != null ?
                new ObjectParameter("SQLStatement", sQLStatement) :
                new ObjectParameter("SQLStatement", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_Tax", sQLStatementParameter);
        }
    
        public virtual int SP_Banks1(string sQLStatement)
        {
            var sQLStatementParameter = sQLStatement != null ?
                new ObjectParameter("SQLStatement", sQLStatement) :
                new ObjectParameter("SQLStatement", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_Banks1", sQLStatementParameter);
        }
    
        public virtual int SP_Tax1(string sQLStatement)
        {
            var sQLStatementParameter = sQLStatement != null ?
                new ObjectParameter("SQLStatement", sQLStatement) :
                new ObjectParameter("SQLStatement", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_Tax1", sQLStatementParameter);
        }
    
        public virtual int SP_Brgy(string sQLStatement)
        {
            var sQLStatementParameter = sQLStatement != null ?
                new ObjectParameter("SQLStatement", sQLStatement) :
                new ObjectParameter("SQLStatement", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_Brgy", sQLStatementParameter);
        }
    
        public virtual int SP_Supplier(string sQLStatement)
        {
            var sQLStatementParameter = sQLStatement != null ?
                new ObjectParameter("SQLStatement", sQLStatement) :
                new ObjectParameter("SQLStatement", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_Supplier", sQLStatementParameter);
        }
    
        public virtual int SP_FMSubsidiaryLedger(string sQLStatement)
        {
            var sQLStatementParameter = sQLStatement != null ?
                new ObjectParameter("SQLStatement", sQLStatement) :
                new ObjectParameter("SQLStatement", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_FMSubsidiaryLedger", sQLStatementParameter);
        }
    }
}
