using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BOSS.Models.FMPositionforSPModels
{
    public class PositionforSPModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public PositionforSPModel()
        {
            getPositionList = new List<PositionList>();
            getPositionColumns = new Tbl_FMPosition();
            getPositionColumns2 = new Tbl_FMPosition();
        }
        public List<PositionList> getPositionList { get; set; }
        public Tbl_FMPosition getPositionColumns { get; set; }
        public Tbl_FMPosition getPositionColumns2 { get; set; }

        public int PositionID { get; set; }
        public int PCHiddenID { get; set; }
        public string PositionTitle { get; set; }
        public int PSID { get; set; }
        public int PCID { get; set; }
        public int PCIDTemp { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> PositionStatusList
        {
            get
            {
                List<PositionStatu> positionStatus = BOSSDB.PositionStatus.ToList();
                return new System.Web.Mvc.SelectList(positionStatus, "PSID", "PSTitle");
            }
        }
        public IEnumerable<System.Web.Mvc.SelectListItem> PositionClassList { get; set; }

    }
    public class PositionList
    {
        public int PositionID { get; set; }
        public string PositionTitle { get; set; }
        public int PSID { get; set; }
        public string PSTitle { get; set; }
        public int PCID { get; set; }
        public string PCTitle { get; set; }
    }
}