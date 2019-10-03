using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BOSS.Models.FMmodels.FMSectorModels
{
    public class SubSectorModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public SubSectorModel()
        {
            getSubSectorList = new List<SubSectorList>();
            getSubSectorColumns = new Tbl_FMSubSector();
        }
        public List<SubSectorList> getSubSectorList { get; set; }
        public Tbl_FMSubSector getSubSectorColumns { get; set; }

        public int SubSectorID { get; set; }
        public int SectorID { get; set; }
        public string SubSectorTitle { get; set; }
        public string SectorCode { get; set; }
        public string SubSectorCode { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> SectorSelectionList
        {
            get
            {
                List<Tbl_FMSector> SectorSelectionList = BOSSDB.Tbl_FMSector.ToList();
                return new System.Web.Mvc.SelectList(SectorSelectionList, "SectorID", "SectorTitle");
            }
        }
    }
    public class SubSectorList
    {
        public int SubSectorID { get; set; }
        public int SectorID { get; set; }
        public string SectorTitle { get; set; }
        public string SubSectorTitle { get; set; }
        public string SubSectorCode { get; set; }
        public string CombinedCode { get; set; }
    }
}