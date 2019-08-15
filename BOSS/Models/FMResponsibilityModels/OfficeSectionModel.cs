using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BOSS.Models.FMResponsibilityModels
{
    public class OfficeSectionModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();

        public OfficeSectionModel()
        {
            getOfficeSecList = new List<OfficeSectionList>();
            getOfficeSecColumns = new Tbl_FMOfficeSection();
            getOfficeSecColumns2 = new Tbl_FMOfficeSection();
            //funcmodel = new FunctionModel();
        }
        public List<OfficeSectionList> getOfficeSecList { get; set; }
        //public FunctionModel funcmodel { get; set; }
        public Tbl_FMOfficeSection getOfficeSecColumns { get; set; }
        public Tbl_FMOfficeSection getOfficeSecColumns2 { get; set; }
        public int OfficeSecID { get; set; }
        public string OfficeSecTitle { get; set; }
        public int DeptID { get; set; }
        public int FuncID { get; set; }
        public string DeptTitle { get; set; }
        public string FunctionTitle { get; set; }
        public int DeptID2 { get; set; }
        public int FuncID2 { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DeptSelectionList2
        {
            get
            {
                List<Tbl_FMDepartment> DeptSelectionLists = BOSSDB.Tbl_FMDepartment.ToList();
                return new System.Web.Mvc.SelectList(DeptSelectionLists, "DeptID", "DeptTitle");
            }
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> FunctionList { get; set; }
    }
    public class OfficeSectionList
    {
        public int OfficeSecID { get; set; }
        public string OfficeSecTitle { get; set; }
        public int DeptID { get; set; }
        public int FuncID { get; set; }
        public string FunctionTitle { get; set; }
        public string DeptTitle { get; set; }
    }
}