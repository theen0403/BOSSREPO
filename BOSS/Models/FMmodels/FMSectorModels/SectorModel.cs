using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BOSS.Models.FMmodels.FMSectorModels
{
    public class SectorModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public SectorModel()
        {
            getSectorList = new List<SectorList>();
            SectorList = new SectorList();
        }
        public List<SectorList> getSectorList { get; set; }
        public SectorList SectorList { get; set; }
        public int ActionID { get; set; }
    }
    public class SectorList
    {
        public int SectorID { get; set; }
        [Required]
        public string SectorTitle { get; set; }
        [Required]
        public string SectorCode { get; set; }
    }
}