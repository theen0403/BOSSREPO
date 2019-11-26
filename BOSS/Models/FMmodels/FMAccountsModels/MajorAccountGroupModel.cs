using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace BOSS.Models.FMmodels.FMAccountsModels
{
    public class MajorAccountGroupModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public MajorAccountGroupModel()
        {
            getMajorAccntGrpColumns = new Tbl_FMCOA_MajorAccountGroup();
            getMajorAccntGrpList = new List<MajorAccountGroupList>();
        }
        public Tbl_FMCOA_MajorAccountGroup getMajorAccntGrpColumns { get; set; }
        public List<MajorAccountGroupList> getMajorAccntGrpList { get; set; }
        public int MAGID { get; set; }
        public int AGID { get; set; }
        public string MAGTitle { get; set; }
        public string MAGCode { get; set; }
        public int RevIDMAG { get; set; }
        public int allotclasssTempID { get; set; }
        public int AGIDMag { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> AccountGrpList { get; set; }
    }
    public class MajorAccountGroupList
    {
        public int MAGID { get; set; }
        public string MAGTitle { get; set; }
        public string MAGAccountCode { get; set; }
        public string RevYear { get; set; }
        public string AllotmentClassTitle { get; set; }
        public string AGTitle { get; set; }
    }
}