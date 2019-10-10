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
            getMajorAccntGrpColumns = new Tbl_FMMajorAccountGroup();
            getMajorAccntGrpList = new List<MajorAccountGroupList>();
        }
        public Tbl_FMMajorAccountGroup getMajorAccntGrpColumns { get; set; }
        public List<MajorAccountGroupList> getMajorAccntGrpList { get; set; }
        public int MAGID { get; set; }
        public string MAGTitle { get; set; }
        public int RevIDMAG { get; set; }
        public int AllotmentIDMAG { get; set; }
        public int AGIDMag { get; set; }
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