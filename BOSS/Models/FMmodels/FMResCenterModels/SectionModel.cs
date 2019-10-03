using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BOSS.Models.FMmodels.FMResCenterModels
{
    public class SectionModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();

        public SectionModel()
        {
            getSectionList = new List<SectionList>();
            getSectionColumns = new Tbl_FMSection();
        }
        public List<SectionList> getSectionList { get; set; }
        public Tbl_FMSection getSectionColumns { get; set; }
        public int SectionID { get; set; }
        public int FunctionIDHidden { get; set; }
        public string SectionTitle { get; set; }
        public string FunctionTitle { get; set; }
        public string DeptTitle { get; set; }
        public int DeptID { get; set; }
        public int FunctionID { get; set; }
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
    public class SectionList
    {
        public int SectionID { get; set; }
        public string SectionTitle { get; set; }
        public string FunctionTitle { get; set; }
        public string DeptTitle { get; set; }
    }
}