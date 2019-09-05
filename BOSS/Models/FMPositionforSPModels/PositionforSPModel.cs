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
        public string PositionTitle { get; set; }
        public string PositionCode { get; set; }
        
    }
    public class PositionList
    {
        public int PositionID { get; set; }
        public string PositionTitle { get; set; }
        public string PositionCode { get; set; }
    }
}