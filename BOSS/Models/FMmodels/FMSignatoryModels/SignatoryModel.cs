using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BOSS.Models.FMmodels.FMSignatoryModels
{
    public class SignatoryModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();

        public SignatoryModel()
        {
            getSignatoryList = new List<SignatoryList>();
            SignatoryList = new SignatoryList();
            FunctionList = new List<SelectListItem>();
        }
        public List<SignatoryList> getSignatoryList { get; set; }
        public SignatoryList SignatoryList { get; set; }

        public int ActionID { get; set; }
        public int PositionID { get; set; }
        public int DeptID { get; set; }
        public int FunctionID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> PositionList
        {
            get
            {
                List<Tbl_FMPosition> tbl_FMPositions = BOSSDB.Tbl_FMPosition.ToList();
                return new System.Web.Mvc.SelectList(tbl_FMPositions, "PositionID", "PositionTitle");
            }
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> DepartmentList
        {
            get
            {
                List<Tbl_FMRes_Department> Tbl_FMRes_Departments = BOSSDB.Tbl_FMRes_Department.ToList();
                return new System.Web.Mvc.SelectList(Tbl_FMRes_Departments, "DeptID", "DeptTitle");
            }
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> FunctionList { get; set; }
    }
    public class SignatoryList
    {
        public int SignatoryID { get; set; }
        [Required]
        public string SignatoryName { get; set; }
        [Required]
        public string PreferredName { get; set; }
        public string PositionTitle { get; set; }
        public string DeptTitle { get; set; }
        [Required]
        public string Division { get; set; }
        public string FunctionTitle { get; set; }
        public bool isHead { get; set; }
        public bool isActive { get; set; }
    }
}