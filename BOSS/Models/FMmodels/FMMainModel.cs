using BOSS.Models.FMmodels.FMAccountsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BOSS.Models.FMmodels
{
    public class FMMainModel
    {
        public FMMainModel()
        {
            AccountGrpModel = new AccountGroupModel();
            AllotClassModel = new AllotmentClassModel();
            RevYearModel = new RevisionYearModel();
        }
        public  AccountGroupModel AccountGrpModel { get; set; }
        public AllotmentClassModel AllotClassModel { get; set; }
        public RevisionYearModel RevYearModel { get; set; }


    }

}