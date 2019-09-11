using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BOSS.Models.FMSignatoryModels
{
    public class SignatoryModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();

        public SignatoryModel()
        {
            getSignatoryList = new List<SignatoryDTList>();
            getSignatoryColumns = new Tbl_FMSignatory();
            getSignatoryColumns2 = new Tbl_FMSignatory();

        }
        public List<SignatoryDTList> getSignatoryList { get; set; }
        public Tbl_FMSignatory getSignatoryColumns { get; set; }
        public Tbl_FMSignatory getSignatoryColumns2 { get; set; }
        public int SignatoryID { get; set; }
        public string SignatoryName { get; set; }
        public int PositionID { get; set; }
        public int FunctionID { get; set; }
        public bool isHead { get; set; }
        public int DeptID { get; set; }
        public int funcTempID { get; set; }
        public string PreferredName { get; set; }
        public string Division { get; set; }
        public bool isActive { get; set; }
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
                List<Tbl_FMDepartment> tbl_FMDepartments = BOSSDB.Tbl_FMDepartment.ToList();
                return new System.Web.Mvc.SelectList(tbl_FMDepartments, "DeptID", "DeptTitle");
            }
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> FunctionList { get; set; }
    }
    public class SignatoryDTList
    {
        public int SignatoryID { get; set; }
        public string SignatoryName { get; set; }
        public int PositionID { get; set; }
        public string PositionTitle { get; set; }
        public string DeptTitle { get; set; }
        public int FunctionID { get; set; }
        public int isHead { get; set; }
        public int DeptID { get; set; }
        public string PreferredName { get; set; }
        public string Division { get; set; }
        public int isActive { get; set; }
    }
}