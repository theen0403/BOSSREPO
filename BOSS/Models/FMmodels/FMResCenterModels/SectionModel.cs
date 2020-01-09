using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Models.FMmodels.FMResCenterModels
{
    public class SectionModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();

        public SectionModel()
        {
            getSectionList = new List<SectionList>();
            SectionList = new SectionList();

            DeptList = new List<SelectListItem>();
            FunctionList = new List<SelectListItem>();
        }
        public int ActionID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DeptList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> FunctionList { get; set; }
        public List<SectionList> getSectionList { get; set; }
        public SectionList SectionList { get; set; }
    }
    public class SectionList
    {
        public int SectionID { get; set; }
        [Required(ErrorMessage = "Sectiion title is required.")]
        public string SectionTitle { get; set; }
        public int FunctionID { get; set; }
        public int DeptID { get; set; }

        public string DeptTitle { get; set; }
        public string FunctionTitle { get; set; }
    }
}