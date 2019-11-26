using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BOSS.Models.FMmodels.FMPositionModels
{
    public class PositionModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public PositionModel()
        {
            getPositionList = new List<PositionList>();
            PositionList = new PositionList();
        }
        public int ActionID { get; set; }
        public List<PositionList> getPositionList { get; set; }
        public PositionList PositionList { get; set; }
    }
    public class PositionList
    {
        public int PositionID { get; set; }
        [Required(ErrorMessage = "Please enter Position Title")]
        public string PositionTitle { get; set; }
        [Required(ErrorMessage = "Please enter Position Code")]
        public string PositionCode { get; set; }
    }
}