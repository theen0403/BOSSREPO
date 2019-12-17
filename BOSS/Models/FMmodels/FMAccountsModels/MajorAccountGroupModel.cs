using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace BOSS.Models.FMmodels.FMAccountsModels
{
    public class MajorAccountGroupModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public MajorAccountGroupModel()
        {
            getMajorAccountGroupList = new List<MajorAccountGroupList>();
            MajorAccountGroupList = new MajorAccountGroupList();

            RevYearList = new List<SelectListItem>();
            AllotClassList = new List<SelectListItem>();
            AccntGrpList = new List<SelectListItem>();
        }
        public int ActionID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> RevYearList { get; set; }
        public List<SelectListItem> AllotClassList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> AccntGrpList { get; set; }
        public List<MajorAccountGroupList> getMajorAccountGroupList { get; set; }
        public MajorAccountGroupList MajorAccountGroupList { get; set; }
    }
    public class MajorAccountGroupList
    {
        public int MAGID { get; set; }
        [Required(ErrorMessage = "Please enter Major Account Group Title")]
        public string MAGTitle { get; set; }
        [Required(ErrorMessage = "Please enter Major Account Group Code")]
        public string concatMAGCode { get; set; }
        public string MAGCode { get; set; }
        public string RevYear { get; set; }
        public string AllotmentClassTitle { get; set; }
        public string AGTitle { get; set; }

        public int RevID { get; set; }
        public int AllotmentClassID { get; set; }
        public int AGID { get; set; }
    }
}