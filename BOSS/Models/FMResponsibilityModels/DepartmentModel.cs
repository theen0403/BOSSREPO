using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BOSS.Models.FMResponsibilityModels
{
    public class DepartmentModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
   
    public DepartmentModel()
        {
            getDeptList = new List<DepartmentList>();
            getDeptColumns = new Tbl_FMDepartment();
            getDeptColumns2 = new Tbl_FMDepartment();
        }
        public List<DepartmentList> getDeptList { get; set; }
        public Tbl_FMDepartment getDeptColumns { get; set; }
        public Tbl_FMDepartment getDeptColumns2 { get; set; }
        public int DeptID { get; set; }
        public string DeptTitle { get; set; }
        public string DeptAbbrv { get; set; }
        public string DeptOfficeCode { get; set; }
        public int SectorID { get; set; }
        public int FundID { get; set; }
        public int SubSectorID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> FundTypeList
        {
            get
            {
                List<FundType> FundTypeLists = BOSSDB.FundTypes.ToList();
                return new System.Web.Mvc.SelectList(FundTypeLists, "FundID", "FundTitle");
            }
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> SectorList
        {
            get
            {
                List<Sector> SectorLists = BOSSDB.Sectors.ToList();
                return new System.Web.Mvc.SelectList(SectorLists, "SectorID", "SectorTitle");
            }
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> SubSectorList { get; set; }
    }

    public class DepartmentList
    {
    public int DeptID { get; set; }
    public string DeptTitle { get; set; }
    public string DeptAbbrv { get; set; }
    public string DeptOfficeCode { get; set; }
    public string SectorTitle { get; set; }
    public string FundTitle {get; set; }
    public string SubSectorTitle { get; set; }
    public int SubSectorID { get; set; }
    }
}