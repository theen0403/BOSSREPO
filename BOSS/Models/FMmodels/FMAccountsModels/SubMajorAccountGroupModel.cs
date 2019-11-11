using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace BOSS.Models.FMmodels.FMAccountsModels
{
    public class SubMajorAccountGroupModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public SubMajorAccountGroupModel()
        {
            getSubMajorAccntGrpColumns = new Tbl_FMSubMajorAccountGroup();
            getSubMajorAccntGrpList = new List<SubMajorAccountGroupList>();
        }
        public Tbl_FMSubMajorAccountGroup getSubMajorAccntGrpColumns { get; set; }
        public List<SubMajorAccountGroupList> getSubMajorAccntGrpList { get; set; }
        public int SMAGID { get; set; }
        public int RevID { get; set; }
        public int AllotmentID { get; set; }
        public int AGID { get; set; }
        public int MAGID { get; set; }
        public string SMAGTitle { get; set;}
        public string SMAGCode { get; set; }
        public int RevIDSMAG { get; set; }
        public int allotclasssTempIDSMAG { get; set; }
        public int AGIDSMag { get; set; }
        public int MAGIDSMag { get; set; }
        public int MAGIDtemp { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> MajorAccountGrpList { get; set; }
    }
    public class SubMajorAccountGroupList
    {
        public int SMAGID { get; set; }
        public string RevYear { get; set; }
        public string AllotmentClassTitle { get; set; }
        public string AGTitle { get; set; }
        public string MAGTitle { get; set; }
        public string SMAGTitle { get; set; }
        public string SMAGCode { get; set; }
        public string SMAGAccountCode { get; set; }
    }
}