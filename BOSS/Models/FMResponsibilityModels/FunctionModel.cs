using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BOSS.Models.FMResponsibilityModels
{
    public class FunctionModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();

        public FunctionModel()
        {
            getFuncList = new List<FunctionList>();
            getFunctionColumns = new Tbl_FMFunction(); 
            getFunctionColumns2 = new Tbl_FMFunction();
            //deptmodel = new DepartmentModel();
        }
        public List<FunctionList> getFuncList { get; set; }
        public Tbl_FMFunction getFunctionColumns { get; set; }
        public Tbl_FMFunction getFunctionColumns2 { get; set; }
        public int FunctionID { get; set; }
        public string FunctionTitle { get; set; }
        public string FunctionAbbrv { get; set; }
        public string FunctionCode { get; set; }
        public int FundID { get; set; }

        public string SubSectorTitle { get; set; }
        public string SectorTitle { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> FundTypeList
        {
            get
            {
                List<FundType> FundTypeLists = BOSSDB.FundTypes.ToList();
                return new System.Web.Mvc.SelectList(FundTypeLists, "FundID", "FundTitle");
            }
        }
        public int DeptID2 { get; set; }
        public int DeptID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DeptSelectionList
        {
            get
            {
                List<Tbl_FMDepartment> DeptSelectionLists = BOSSDB.Tbl_FMDepartment.ToList();
                return new System.Web.Mvc.SelectList(DeptSelectionLists, "DeptID", "DeptTitle");
            }
        }
        //public DepartmentModel deptmodel { get; set; }

    }
    public class FunctionList
    {
        public int FunctionID { get; set; }
        public string FunctionTitle { get; set; }
        public string FunctionAbbrv { get; set; }
        public string FunctionCode { get; set; }
        public string FundTitle { get; set; }
        public int DeptID { get; set; }
        public string SectorTitle { get; set; }
        public string SubSectorTitle { get; set; }
        public int SubSectorID { get; set; }
    }
}