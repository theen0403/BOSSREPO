using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BOSS.Models.FMmodels.FMResCenterModels
{
    public class DepartmentModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public DepartmentModel()
        {
            getDepartmentList = new List<DepartmentList>();
            DepartmentList = new DepartmentList();

            FundList = new List<SelectListItem>();
            SectorList = new List<SelectListItem>();
            SubSectorList = new List<SelectListItem>();
            OfficeTypeList = new List<SelectListItem>();
        }
        public int ActionID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> FundList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> SectorList { get; set; }
        public List<SelectListItem> SubSectorList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> OfficeTypeList { get; set; }
        public List<DepartmentList> getDepartmentList { get; set; }
        public DepartmentList DepartmentList { get; set; }
    }

    public class DepartmentList
    {
        public int DeptID { get; set; }
        [Required(ErrorMessage = "Department title is required.")]
        public string DeptTitle { get; set; }
        [Required(ErrorMessage = "Department abbreviation is required.")]
        public string DeptAbbrv { get; set; }
        [Required(ErrorMessage = "Department code is required.")]
        public string DeptOfficeCode { get; set; }
        public string SectorTitle { get; set; }
        public string FundTitle { get; set; }
        public string SubSectorTitle { get; set; }
        public int SubSectorID { get; set; }
        [Required(ErrorMessage = "Responsibility code is required.")]
        public string RCcode { get; set; }
        public string OfficeTypeTitle { get; set; }


        public int FundID { get; set; }
        public int SectorID { get; set; }
        public int OfficeTypeID { get; set; }
    }
}