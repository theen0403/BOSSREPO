using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BOSS.Models.FMmodels.FMResCenterModels
{
    public class FunctionModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();

        public FunctionModel()
        {
            getFuncList = new List<FunctionList>();
            getFunctionColumns = new Tbl_FMRes_Function();
            //deptmodel = new DepartmentModel();
        }
        public List<FunctionList> getFuncList { get; set; }
        public Tbl_FMRes_Function getFunctionColumns { get; set; }
        public int FunctionID { get; set; }
        public int subsectorIDHiddenfunc { get; set; }
        public string FunctionTitle { get; set; }
        public string FunctionAbbrv { get; set; }
        public string FunctionCode { get; set; }
        public string FundTitle { get; set; }
        public int SubSectorID { get; set; }
        public int SectorID { get; set; }
        public string DeptTitle { get; set; }
        public string DeptOfficeCodefunc { get; set; }
        public int OfficeTypeID { get; set; }
        public int DeptID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DeptSelectionListfunc
        {
            get
            {
                List<Tbl_FMRes_Department> departments = BOSSDB.Tbl_FMRes_Department.ToList();
                return new System.Web.Mvc.SelectList(departments, "DeptID", "DeptTitle");
            }
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> SectorListfunc
        {
            get
            {
                List<Tbl_FMSector_Sector> SectorLists = BOSSDB.Tbl_FMSector_Sector.ToList();
                return new System.Web.Mvc.SelectList(SectorLists, "SectorID", "SectorTitle");
            }
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> SubSectorListfunc { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> officeTypeListfunc
        {
            get
            {
                List<Tbl_FMOfficeType> officeTypesfunc = BOSSDB.Tbl_FMOfficeType.ToList();
                return new System.Web.Mvc.SelectList(officeTypesfunc, "OfficeTypeID", "OfficeTypeTitle");
            }
        }
        // public DepartmentModel deptmodel { get; set; }
    }
    public class FunctionList
    {
        public int FunctionID { get; set; }
        public string DeptTitle { get; set; }
        public string FunctionTitle { get; set; }
        public string FunctionAbbrv { get; set; }
        public string FunctionCode { get; set; }
        public string FundTitle { get; set; }
        public string SectorTitle { get; set; }
        public string SubSectorTitle { get; set; }
        public int SubSectorID { get; set; }
        public string OfficeTypeTitle { get; set; }
        public string DeptOfficeCodefunc { get; set; }
    }
}