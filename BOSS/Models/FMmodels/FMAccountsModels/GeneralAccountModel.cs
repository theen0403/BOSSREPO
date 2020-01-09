//using ExpressiveAnnotations.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Models.FMmodels.FMAccountsModels
{
    public class GeneralAccountModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public GeneralAccountModel()
        {
            getGeneralAccountList = new List<GeneralAccountList>();
            GeneralAccountList = new GeneralAccountList();

            RevYearList = new List<SelectListItem>();
            AllotClassList = new List<SelectListItem>();
            AccntGrpList = new List<SelectListItem>();
            MajAccntGrpList = new List<SelectListItem>();
            SubMajAccntGrpList = new List<SelectListItem>();
            GenAccntGrpList = new List<SelectListItem>();
        }
        public int ActionID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> RevYearList { get; set; }
        public List<SelectListItem> AllotClassList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> AccntGrpList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> MajAccntGrpList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> SubMajAccntGrpList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> GenAccntGrpList { get; set; }
        public List<GeneralAccountList> getGeneralAccountList { get; set; }
        public GeneralAccountList GeneralAccountList { get; set; }

    }
    public class GeneralAccountList
    {
        //Required Fields
        [Required(ErrorMessage = "Please enter Sub Major Account Group Title")]
        public string GATitle { get; set; }

        [Required(ErrorMessage = "Please enter Sub Major Account Group Code")]
        public string GACode { get; set; }

        [Required(ErrorMessage = "Please select IsReserve")]
        public bool IsReserve { get; set; }

        [Required(ErrorMessage = "Please select IsRelease")]
        public bool IsRelease { get; set; }

        [Required(ErrorMessage = "Please select IsContinuing")]
        public bool IsContinuing { get; set; }

        [Required(ErrorMessage = "Please select IsOBRCash")]
        public bool IsOBRCash { get; set; }

        [Required(ErrorMessage = "Please select NormalBal")]
        public string NormalBal { get; set; }

        [Required(ErrorMessage = "Please select Major Account Group Title")]
        public int SMAGID { get; set; }

        //Optional Fields
        public bool IsMiscellaneous { get; set; }

        public bool isContraAccountCheckBox { get; set; }
        public bool isSubAccountCheckBox { get; set; }

        //Conditional Validation
        //[AssertThat("IsContra == true || IsSubAccnt == true", ErrorMessage = "Please select General AccntID")]
        //[RequiredIf("IsContraCheckBox == true || IsSubAccntCheckBox == true", ErrorMessage = "Please enter Reserved Pecent")]
        public int? GAID { get; set; }

        public int GAID2 { get; set; }

      //  [RequiredIf("IsReserve == true", ErrorMessage = "Please enter Reserved Pecent")]
        public string ReservePercent { get; set; }



        public string RevYear { get; set; }
        public int RevID { get; set; }

        public string AllotmentClassTitle { get; set; }
        public int AllotmentClassID { get; set; }

        public string AGTitle { get; set; }
        public int AGID { get; set; }

        public string MAGTitle { get; set; }
        public int MAGID { get; set; }

        public string SMAGTitle { get; set; }
        public string SubGenCode { get; set; }
        public string GenAccountCode { get; set; }
    }
}