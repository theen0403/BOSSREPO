using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BOSS.Models.FMmodels.FMResCenterModels
{
    public class DepartmentModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public DepartmentModel()
        {
            getDeptList = new List<DepartmentList>();
            getDeptColumns = new Tbl_FMDepartment();
        }
        public List<DepartmentList> getDeptList { get; set; }
        public Tbl_FMDepartment getDeptColumns { get; set; }
        public int DeptID { get; set; }
        public string DeptTitle { get; set; }
        public string DeptAbbrv { get; set; }
        public string DeptOfficeCode { get; set; }
        public int SectorID { get; set; }
        public int OfficeTypeID { get; set; }
        public int FundID { get; set; }
        public int SubSectorID { get; set; }
        public string RCcode { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> FundTypeList
        {
            get
            {
                List<Fund> fundList = BOSSDB.Funds.ToList();
                return new System.Web.Mvc.SelectList(fundList, "FundID", "FundTitle");
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
        public IEnumerable<System.Web.Mvc.SelectListItem> officeTypeList
        {
            get
            {
                List<OfficeType> officeTypeList = BOSSDB.OfficeTypes.ToList();
                return new System.Web.Mvc.SelectList(officeTypeList, "OfficeTypeID", "OfficeTypeTitle");
            }
        }
    }

    public class DepartmentList
    {
        public int DeptID { get; set; }
        public string DeptTitle { get; set; }
        public string DeptAbbrv { get; set; }
        public string DeptOfficeCode { get; set; }
        public string SectorTitle { get; set; }
        public string FundTitle { get; set; }
        public string SubSectorTitle { get; set; }
        public int SubSectorID { get; set; }
        public string RCcode { get; set; }
        public string OfficeTypeTitle { get; set; }

    }
}