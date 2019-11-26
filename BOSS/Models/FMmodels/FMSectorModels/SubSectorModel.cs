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
            SubSectorList = new SubSectorList();
            
        }
        public List<SubSectorList> getSubSectorList { get; set; }
        public SubSectorList SubSectorList { get; set; }
        public int ActionID { get; set; }
        public string SectorCode { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> SectorSelectionList
        {
            get
            {
                List<Tbl_FMSector_Sector> SectorSelectionList = BOSSDB.Tbl_FMSector_Sector.ToList();
                return new System.Web.Mvc.SelectList(SectorSelectionList, "SectorID", "SectorTitle");
            }
        }
    }
    public class SubSectorList
    {
        public int SubSectorID { get; set; }
        public int SectorID { get; set; }
        public string SectorTitle { get; set; }
        [Required(ErrorMessage = "Please enter Sub Sector Title")]
        public string SubSectorTitle { get; set; }
        [Required(ErrorMessage = "Please enter Sub Sector Code")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Sub Sector Code should be 3 digits")]
        public string SubSectorCode { get; set; }
        public string CombinedCode { get; set; }
    }
}