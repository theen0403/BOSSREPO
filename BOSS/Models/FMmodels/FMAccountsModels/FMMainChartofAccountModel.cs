using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BOSS.Models.FMmodels.FMAccountsModels
{
    public class FMMainChartofAccountModel
    {
        public FMMainChartofAccountModel()
        {
            AccountGrpModel = new AccountGroupModel();
            AllotClassModel = new AllotmentClassModel();
            RevYearModel = new RevisionYearModel();
            MajorAccountGrpModel = new MajorAccountGroupModel();
            SubMajorAccountGrpModel = new SubMajorAccountGroupModel();
            GeneralAccntModel = new GeneralAccountModel();
        }
        public AccountGroupModel AccountGrpModel { get; set; }
        public AllotmentClassModel AllotClassModel { get; set; }
        public RevisionYearModel RevYearModel { get; set; }
        public MajorAccountGroupModel MajorAccountGrpModel { get; set; }
        public SubMajorAccountGroupModel SubMajorAccountGrpModel { get; set; }
        public GeneralAccountModel GeneralAccntModel { get; set; }
    }
}