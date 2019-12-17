using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BOSS.Models.FMmodels.FMBarangayModels
{
    public class BrgyNameModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public BrgyNameModel()
        {
            getBarangayNameList = new List<BarangayNameList>();
            BarangayNameList = new BarangayNameList();
        }
        public int ActionID { get; set; }
        public List<BarangayNameList> getBarangayNameList { get; set; }
        public BarangayNameList BarangayNameList { get; set; }
    }
    public class BarangayNameList
    {
        public int BrgyID { get; set; }
        [Required(ErrorMessage = "Please enter Barangay Name")]
        public string BrgyName { get; set; }
    }
}