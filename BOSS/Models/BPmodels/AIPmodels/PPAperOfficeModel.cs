using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace BOSS.Models.BPmodels.AIPmodels
{
    public class PPAperOfficeModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public PPAperOfficeModel()
        {
            getPPAperOfficeList = new List<PPAperOfficeList>();
            PPAperOfficeList = new PPAperOfficeList();
            DeptList = new List<SelectListItem>();
            AvailableYear = new List<SelectListItem>();
        }
        public int ActionID { get; set; }
        public List<PPAperOfficeList> getPPAperOfficeList { get; set; }
        public PPAperOfficeList PPAperOfficeList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DeptList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> AvailableYear { get; set; }
    }
    public class PPAperOfficeList
    {
        public int DeptID { get; set; }
        public int Year { get; set; }
    }
}