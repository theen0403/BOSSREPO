using BOSS.GlobalFunctions;
using BOSS.Models;
using BOSS.Models.FMAppropriationSourceModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace BOSS.Controllers
{
    public class FileMaintenanceAppropriationController : Controller
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        DatatypeValidation GlobalFunction = new DatatypeValidation();

        // GET: FileMaintenanceAppropriation
        [Authorize]
        public ActionResult FileAppropFundSource()
        {
            FMAppropriationSourceModel model = new FMAppropriationSourceModel();
            return View(model);
        }
        //Get Appropriation Source Datatable Partial View
        public ActionResult GetAppropriationSourceDT()
        {
            FMAppropriationSourceModel model = new FMAppropriationSourceModel();

            List<AppropriationSourceList> getAppropriationSourceList = new List<AppropriationSourceList>();

            var SQLQuery = "SELECT [AppropriationID], [AppropriationSourceType].[AppropriationSourceType], [FundSource].[FundSourceTitle], [BudgetYear].[BudgetYearTitle], [Description] FROM[dbo].[Tbl_FMAppropriationSource], [dbo].[FundSource], [dbo].[AppropriationSourceType], [dbo].[BudgetYear] where [dbo].[FundSource].AppropriationSourceID= [dbo].AppropriationSourceType.AppropriationSourceID and[dbo].[FundSource].FundSourceID=[dbo].Tbl_FMAppropriationSource.FundSourceID and[dbo].[BudgetYear].BudgetYearID= [dbo].Tbl_FMAppropriationSource.BudgetYearID";
            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_AppropriationSource]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        getAppropriationSourceList.Add(new AppropriationSourceList()
                        {
                            AppropriationID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            AppropriationSourceType = GlobalFunction.ReturnEmptyString(dr[1]),
                            FundSourceTitle = GlobalFunction.ReturnEmptyString(dr[2]),
                            Description = GlobalFunction.ReturnEmptyString(dr[4]),
                            BudgetYearTitle = GlobalFunction.ReturnEmptyString(dr[3])
                        }); 
                    }
                }
                Connection.Close();
            }
            model.getAppropriationSourceList = getAppropriationSourceList.ToList();
            return PartialView("AppropriationSource/_TableAppropriationSource", model.getAppropriationSourceList);
        }
        public ActionResult GetDynamicFund(int AppropriationSourceID)
        {
            FMAppropriationSourceModel model = new FMAppropriationSourceModel();

            model.FundSourceList = new SelectList((from s in BOSSDB.FundSources.Where(a => a.AppropriationSourceID == AppropriationSourceID).ToList() select new { FundSourceID = s.FundSourceID, FundSourceTitle = s.FundSourceTitle }), "FundSourceID", "FundSourceTitle");
            
            return PartialView("AppropriationSource/_DynamicFundSource", model);
        }
        public ActionResult GetDynamicFund2(FMAppropriationSourceModel model, int FundSourceIDHidden, int ApproIDHidden)
        {
            model.FundSourceList = new SelectList((from s in BOSSDB.FundSources.Where(a => a.AppropriationSourceID == ApproIDHidden).ToList() select new { FundSourceID = s.FundSourceID, FundSourceTitle = s.FundSourceTitle }), "FundSourceID", "FundSourceTitle");
            model.FundSourceID = FundSourceIDHidden;

            return PartialView("AppropriationSource/_DynamicFundSource", model);
        }
        //Get AppropriationSource Partial View
        public ActionResult GetAddAppropSource()
        {
            FMAppropriationSourceModel model = new FMAppropriationSourceModel();
            return PartialView("AppropriationSource/_AddAppropriationSource", model);
        }
        ////Add AppropriationSource
        public JsonResult AddNewAppropSoure(FMAppropriationSourceModel model)
        {
            Tbl_FMAppropriationSource tbl_AppropriationSource = new Tbl_FMAppropriationSource();

            tbl_AppropriationSource.AppropriationID = GlobalFunction.ReturnEmptyInt(model.AppropriationID);
            tbl_AppropriationSource.FundSourceID = GlobalFunction.ReturnEmptyInt(model.FundSourceID);
            tbl_AppropriationSource.Description = GlobalFunction.ReturnEmptyString(model.getAppropriationSourceColumns.Description);
            tbl_AppropriationSource.BudgetYearID = GlobalFunction.ReturnEmptyInt(model.BudgetYearID);
            BOSSDB.Tbl_FMAppropriationSource.Add(tbl_AppropriationSource);

            BOSSDB.SaveChanges();

            return Json(tbl_AppropriationSource);
        }
        //Get AppropriationSource Update Partial View
        public ActionResult Get_UpdateAppropSource(FMAppropriationSourceModel model, int AppropriationID)
        {
            Tbl_FMAppropriationSource tblAP = (from e in BOSSDB.Tbl_FMAppropriationSource where e.AppropriationID == AppropriationID select e).FirstOrDefault();
           
            model.getAppropriationSourceColumns2.Description = tblAP.Description;
            model.BudgetYearID = Convert.ToInt32(tblAP.BudgetYearID);
            model.AppropriationSourceID = Convert.ToInt32(tblAP.FundSource.AppropriationSourceID);
            model.FundSourceIDHidden= Convert.ToInt32(tblAP.FundSourceID);
            model.ApproIDHidden = Convert.ToInt32(tblAP.FundSource.AppropriationSourceID);
            model.getAppropriationSourceColumns2.AppropriationID = tblAP.AppropriationID;
            return PartialView("AppropriationSource/_UpdateAppropriationSource", model);
        }
        //Update AppropriationSource
        public ActionResult UpdateAppropSource(FMAppropriationSourceModel model)
        {
            Tbl_FMAppropriationSource tbl_AppropriationSource = (from e in BOSSDB.Tbl_FMAppropriationSource where e.AppropriationID == model.getAppropriationSourceColumns2.AppropriationID select e).FirstOrDefault();

            tbl_AppropriationSource.FundSourceID = GlobalFunction.ReturnEmptyInt(model.FundSourceID);
            tbl_AppropriationSource.Description = GlobalFunction.ReturnEmptyString(model.getAppropriationSourceColumns2.Description);
            tbl_AppropriationSource.BudgetYearID = GlobalFunction.ReturnEmptyInt(model.BudgetYearID);
            BOSSDB.Entry(tbl_AppropriationSource);
            BOSSDB.SaveChanges();
            return PartialView("AppropriationSource/_AddAppropriationSource", model);
        }
        //Delete AppropriationSource
        public ActionResult DeleteAppropriationSource(FMAppropriationSourceModel model, int AppropriationID)
        {
            Tbl_FMAppropriationSource tbl_AppropriationSource = (from e in BOSSDB.Tbl_FMAppropriationSource where e.AppropriationID == AppropriationID select e).FirstOrDefault();
            BOSSDB.Tbl_FMAppropriationSource.Remove(tbl_AppropriationSource);
            BOSSDB.SaveChanges();
            return RedirectToAction("FileAppropFundSource");
        }


        //=======================================================
        public ActionResult FundSourceTab()
        {
            return PartialView("FundSource/FundSourceIndex");
        }

        public ActionResult AppSourceTab()
        {
            FMAppropriationSourceModel model = new FMAppropriationSourceModel();
            return PartialView("AppropriationSource/AppropriationSourceIndex", model);
        }

    }
}