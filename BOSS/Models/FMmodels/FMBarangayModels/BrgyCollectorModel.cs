using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace BOSS.Models.FMmodels.FMBarangayModels
{
    public class BrgyCollectorModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public BrgyCollectorModel()
        {
            getBarangayCollectorList = new List<BarangayCollectorList>();
            BrangayCollectorList = new BarangayCollectorList();
        }
        public int ActionID { get; set; }
        public List<BarangayCollectorList> getBarangayCollectorList { get; set; }
        public BarangayCollectorList BrangayCollectorList { get; set; }
    }
    public class BarangayCollectorList
    {
        public int BrgyCollectorID { get; set; }
        [Required(ErrorMessage = "Please enter First Name")]
        public string Fname { get; set; }
        public string Mname { get; set; }
        [Required(ErrorMessage = "Please enter Last Name")]
        public string Lname { get; set; }

        public string Fullname { get; set; }
    }
}