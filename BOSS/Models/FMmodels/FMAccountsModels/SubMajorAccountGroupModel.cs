using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace BOSS.Models.FMmodels.FMAccountsModels
{
    public class SubMajorAccountGroupModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public SubMajorAccountGroupModel()
        {
            getSubMajorAccntGrpList = new List<SubMajorAccntGrpList>();
            SubMajorAccntGrpList = new SubMajorAccntGrpList();

            RevYearList = new List<SelectListItem>();
            AllotClassList = new List<SelectListItem>();
            AccntGrpList = new List<SelectListItem>();
            MajAccntGrpList = new List<SelectListItem>();
        }
        public int ActionID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> RevYearList { get; set; }
        public List<SelectListItem> AllotClassList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> AccntGrpList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> MajAccntGrpList { get; set; }
        public List<SubMajorAccntGrpList> getSubMajorAccntGrpList { get; set; }
        public SubMajorAccntGrpList SubMajorAccntGrpList { get; set; }


        //public int SMAGID { get; set; }
        //public int RevID { get; set; }
        //public int AllotmentID { get; set; }
        //public int AGID { get; set; }
        //public int MAGID { get; set; }
        //public string SMAGTitle { get; set;}
        //public string SMAGCode { get; set; }
        //public int RevIDSMAG { get; set; }
        //public int allotclasssTempIDSMAG { get; set; }
        //public int AGIDSMag { get; set; }
        //public int MAGIDSMag { get; set; }
        //public int MAGIDtemp { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> MajorAccountGrpList { get; set; }
    }
    public class SubMajorAccntGrpList
    {
        public int SMAGID { get; set; }
        [Required(ErrorMessage = "Please enter Sub Major Account Group Title")]
        public string SMAGTitle { get; set; }
        [Required(ErrorMessage = "Please select Major Account Group Title")]
        public string SMAGCode { get; set; }
        public string SMAGAccountCode { get; set; }

        public int RevID { get; set; }
        public string RevYear { get; set; }
        public int AllotmentClassID { get; set; }
        public string AllotmentClassTitle { get; set; }
        public string AGTitle { get; set; }
        public int AGID { get; set; }
        public int MAGID { get; set; }
        public string MAGTitle { get; set; }
    }
}