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
            getSectorColumns = new Tbl_FMSector();
        }
        public List<SectorList> getSectorList { get; set; }
        public Tbl_FMSector getSectorColumns { get; set; }
        public int SectorID { get; set; }
        public string SectorTitle { get; set; }
        public string SectorCode { get; set; }
    }
    public class SectorList
    {
        public int SectorID { get; set; }
        public string SectorTitle { get; set; }
        public string SectorCode { get; set; }
    }
}