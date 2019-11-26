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
            PayeeList = new PayeeList();
        }
        public int ActionID { get; set; }
        public List<PayeeList> getPayeeList { get; set; }
        public PayeeList PayeeList { get; set; }
        
        public IEnumerable<System.Web.Mvc.SelectListItem> DeptSelectionListPayee
        {
            get
            {
                List<Tbl_FMRes_Department> departments = BOSSDB.Tbl_FMRes_Department.ToList();
                return new System.Web.Mvc.SelectList(departments, "DeptID", "DeptTitle");
            }
        }
        public int DeptID { get; set; }
    }
    public class PayeeList
    {
        public int PayeeID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        public string DeptTitle { get; set; }
    }
}