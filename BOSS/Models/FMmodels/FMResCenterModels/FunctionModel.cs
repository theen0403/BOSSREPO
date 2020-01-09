using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Models.FMmodels.FMResCenterModels
{
    public class FunctionModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();

        public FunctionModel()
        {
            getFunctionList = new List<FunctionList>();
            FunctionList = new FunctionList();

            DeptList = new List<SelectListItem>();
            FundList = new List<SelectListItem>();
            SectorList = new List<SelectListItem>();
            SubSectorList = new List<SelectListItem>();
            OfficeTypeList = new List<SelectListItem>();
        }
        public int ActionID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DeptList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> FundList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> SectorList { get; set; }
        public List<SelectListItem> SubSectorList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> OfficeTypeList { get; set; }
        public List<FunctionList> getFunctionList { get; set; }
        public FunctionList FunctionList { get; set; }
    }
    public class FunctionList
    {
        public int FunctionID { get; set; }
        public string DeptTitle { get; set; }
        [Required(ErrorMessage = "Function Title is required.")]
        public string FunctionTitle { get; set; }
        [Required(ErrorMessage = "Function abbreviation is required.")]
        public string FunctionAbbrv { get; set; }
        [Required(ErrorMessage = "Function code is required.")]
        public string FunctionCode { get; set; }
        public string FundTitle { get; set; }
        public string SectorTitle { get; set; }
        public string SubSectorTitle { get; set; }
        public string OfficeTypeTitle { get; set; }
        public string DeptOfficeCodefunc { get; set; }


        public int DeptID { get; set; }
        public int SectorID { get; set; }
        public int SubSectorID { get; set; }
        public int OfficeTypeID { get; set; }
    }
}