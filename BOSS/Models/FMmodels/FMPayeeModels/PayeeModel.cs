using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BOSS.Models.FMmodels.FMPayeeModels
{
    public class PayeeModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public PayeeModel()
        {
            getPayeeList = new List<PayeeList>();
            getPayeeColumns = new Tbl_FMPayee();
        }
        public List<PayeeList> getPayeeList { get; set; }
        public Tbl_FMPayee getPayeeColumns { get; set; }
        public int PayeeID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int DeptID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DeptSelectionListPayee
        {
            get
            {
                List<Tbl_FMDepartment> departments = BOSSDB.Tbl_FMDepartment.ToList();
                return new System.Web.Mvc.SelectList(departments, "DeptID", "DeptTitle");
            }
        }
    }
    public class PayeeList
    {
        public int PayeeID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string DeptTitle { get; set; }

    }
}