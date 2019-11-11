using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BOSS.Models.FMmodels.FMOfficeTypeModels
{
    public class OfficeTypeModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public OfficeTypeModel()
        {
            getOfficeTypeList = new List<OfficeTypeList>();

            OfficeTypeList = new OfficeTypeList();
        }
        public List<OfficeTypeList> getOfficeTypeList { get; set; }
        public OfficeTypeList OfficeTypeList { get; set; }
        public int ActionID { get; set; }

    }
    public class OfficeTypeList
    {
        public int OfficeTypeID { get; set; }
        [Required]
        public string OfficeTypeTitle { get; set; }
        [Required]
        public string OfficeTypeCode { get; set; }
    }
}