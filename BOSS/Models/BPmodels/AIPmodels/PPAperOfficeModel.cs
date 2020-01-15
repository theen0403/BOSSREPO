using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace BOSS.Models.BPmodels.AIPmodels
{
    public class PPAperOfficeModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public PPAperOfficeModel()
        {
            getPPAperOfficeList = new List<PPAperOfficeList>();
            PPAperOfficeList = new PPAperOfficeList();
            DeptList = new List<SelectListItem>();

            FundList = new List<SelectListItem>();
            SectorList = new List<SelectListItem>();
            SubSectorList = new List<SelectListItem>();
        }
        public int ActionID { get; set; }
        public List<PPAperOfficeList> getPPAperOfficeList { get; set; }
        public PPAperOfficeList PPAperOfficeList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DeptList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> FundList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> SectorList { get; set; }
        public List<SelectListItem> SubSectorList { get; set; }
    }
    public class PPAperOfficeList
    {
        public int ProgramID { get; set; }
        public int DeptID { get; set; }
        public string Year { get; set; }
        public string BudgetYear { get; set; }
        public int SectorID { get; set; }
        public int SubSectorID { get; set; }
        public bool isInfrastructure { get; set; }
        public string AIPCode { get; set; }
        public string ProgDescription { get; set; }
        public string ProgType { get; set; }
        public int FundID { get; set; }
        public string ProgStartDate { get; set; }
        public string ProgCompletionDate { get; set; }
        public string ProgPolicyObjective { get; set; }
        public string ExpectedOutput { get; set; }
        public string ProgOImplementingOffice { get; set; }
        
        public int PPACCCostID { get; set; }
        public string PS { get; set; }
        public string CO { get; set; }
        public string MOOE { get; set; }
        public string OFExpense { get; set; }
        public decimal PPATotal { get; set; }
        public string CCAdoptation { get; set; }
        public string CCMitigation { get; set; }
        public string CCTypologyCode { get; set; }
    }
}