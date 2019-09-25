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
            getOfficeTypeColumns = new OfficeType();
        }
        public List<OfficeTypeList> getOfficeTypeList { get; set; }
        public OfficeType getOfficeTypeColumns { get; set; }
        public int OfficeTypeID { get; set; }
        public string OfficeTypeTitle { get; set; }
        public string OfficeTypeCode { get; set; }

    }
    public class OfficeTypeList
    {
        public int OfficeTypeID { get; set; }
        public string OfficeTypeTitle { get; set; }
        public string OfficeTypeCode { get; set; }
    }
}