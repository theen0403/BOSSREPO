using BOSS.GlobalFunctions;
using BOSS.Models;
using BOSS.Models.FMmodels.FMAppropriarionSouceModels;
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
            AppropSourceModel model = new AppropSourceModel();
            return View();
        }
        //=============================================================================================================================
                                             //Calling Tabs
        public ActionResult FundSourceTab()
        {
            FundSourceModel model = new FundSourceModel();
            return PartialView("FundSource/FundSourceIndex", model);
        }
        public ActionResult AppSourceTab()
        {
            AppropSourceModel model = new AppropSourceModel();
            return PartialView("AppropriationSource/AppropriationSourceIndex", model);
        }
        //=============================================================================================================================
                                            //Fund Source Tab
        public ActionResult GetFundSourceDT()
        {
            FundSourceModel model = new FundSourceModel();

            List<FundSourceList> getFundSourceList = new List<FundSourceList>();

            var SQLQuery = "SELECT TOP (1000) [FundSourceID],[FundSourceTitle],[AppropSourceTypeTitle] FROM [BOSS].[dbo].[Tbl_FMFundSource], [dbo].[AppropriationSourceType] where [dbo].[Tbl_FMFundSource].AppropSourceTypeID = [dbo].[AppropriationSourceType].AppropSourceTypeID";
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
                        getFundSourceList.Add(new FundSourceList()
                        {
                            FundSourceID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            FundSourceTitle = GlobalFunction.ReturnEmptyString(dr[1]),
                            AppropSourceTypeTitle = GlobalFunction.ReturnEmptyString(dr[2])
                        });
                    }
                }
                Connection.Close();
            }
            model.getFundSourceList = getFundSourceList.ToList();
            return PartialView("FundSource/_TableFundSource", model.getFundSourceList);
        }
        public ActionResult Get_AddFundSource()
        {
            FundSourceModel model = new FundSourceModel();
            return PartialView("FundSource/_AddFundSource", model);
        }
        public JsonResult AddNewFundSource(FundSourceModel model)
        {
            Tbl_FMFundSource FundSourcetbl = new Tbl_FMFundSource();

            FundSourcetbl.FundSourceTitle = GlobalFunction.ReturnEmptyString(model.getFundSourceColumns.FundSourceTitle);
            FundSourcetbl.AppropSourceTypeID = GlobalFunction.ReturnEmptyInt(model.AppropSourceTypeID);
            BOSSDB.Tbl_FMFundSource.Add(FundSourcetbl);

            BOSSDB.SaveChanges();

            return Json(FundSourcetbl);
        }
        public ActionResult Get_UpdateAFundSource(FundSourceModel model, int FundSourceID)
        {
            Tbl_FMFundSource tblFundSource = (from e in BOSSDB.Tbl_FMFundSource where e.FundSourceID == FundSourceID select e).FirstOrDefault();

            model.FundSourceID = FundSourceID;
            model.AppropSourceTypeID = Convert.ToInt32(tblFundSource.AppropSourceTypeID);
            model.getFundSourceColumns.FundSourceTitle = tblFundSource.FundSourceTitle;
            return PartialView("FundSource/_UpdateFundSource", model);
        }
        public ActionResult UpdateFundSource(FundSourceModel model)
        {
            Tbl_FMFundSource tblFundSource = (from e in BOSSDB.Tbl_FMFundSource where e.FundSourceID == model.FundSourceID select e).FirstOrDefault();

            tblFundSource.AppropSourceTypeID = GlobalFunction.ReturnEmptyInt(model.AppropSourceTypeID);
            tblFundSource.FundSourceTitle = GlobalFunction.ReturnEmptyString(model.getFundSourceColumns.FundSourceTitle);
            BOSSDB.Entry(tblFundSource);
            BOSSDB.SaveChanges();
            return PartialView("FundSource/_AddFundSource", model);
        }
        public ActionResult DeleteFundSource(FundSourceModel model, int FundSourceID)
        {
            Tbl_FMFundSource fundSourceTBL = (from e in BOSSDB.Tbl_FMFundSource where e.FundSourceID == FundSourceID select e).FirstOrDefault();
            BOSSDB.Tbl_FMFundSource.Remove(fundSourceTBL);
            BOSSDB.SaveChanges();
            return RedirectToAction("FileAppropFundSource");
        }
        //=============================================================================================================================
                                                    //Apprpriation Source Tab
        //Get Appropriation Source Datatable Partial View
        public ActionResult GetAppropriationSourceDT()
        {
            AppropSourceModel model = new AppropSourceModel();

            List<AppropriationSourceList> getAppropSourceList = new List<AppropriationSourceList>();

            var SQLQuery = "SELECT [AppropriationID], [AppropSourceTypeTitle], [FundSourceTitle], [Description], [BudgetYearTitle] FROM [Tbl_FMAppropriationSource], [Tbl_FMFundSource], [AppropriationSourceType], [BudgetYear] where [BudgetYear].BudgetYearID = [dbo].Tbl_FMAppropriationSource.BudgetYearID and [Tbl_FMAppropriationSource].FundSourceID = [Tbl_FMFundSource].FundSourceID and [Tbl_FMFundSource].AppropSourceTypeID = [AppropriationSourceType].AppropSourceTypeID";
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
                        getAppropSourceList.Add(new AppropriationSourceList()
                        {
                            AppropriationID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            AppropSourceTypeTitle = GlobalFunction.ReturnEmptyString(dr[1]),
                            FundSourceTitle = GlobalFunction.ReturnEmptyString(dr[2]),
                            Description = GlobalFunction.ReturnEmptyString(dr[3]),
                            BudgetYearTitle = GlobalFunction.ReturnEmptyString(dr[4])
                        }); 
                    }
                }
                Connection.Close();
            }
            model.getAppropSourceList = getAppropSourceList.ToList();
            return PartialView("AppropriationSource/_TableAppropriationSource", model.getAppropSourceList);
        }
        public ActionResult GetDynamicFund(int AppropSourceTypeID)
        {
            AppropSourceModel model = new AppropSourceModel();

            model.FundSourceList = new SelectList((from s in BOSSDB.Tbl_FMFundSource.Where(a => a.AppropSourceTypeID == AppropSourceTypeID).ToList() select new { FundSourceID = s.FundSourceID, FundSourceTitle = s.FundSourceTitle }), "FundSourceID", "FundSourceTitle");

            return PartialView("AppropriationSource/_DynamicFundSource", model);
        }
        public ActionResult GetDynamicFund2(AppropSourceModel model, int AppropSourceTypeID, int FundSourceIDHidden)
        {
            model.FundSourceList = new SelectList((from s in BOSSDB.Tbl_FMFundSource.Where(a => a.AppropSourceTypeID == AppropSourceTypeID).ToList() select new { FundSourceID = s.FundSourceID, FundSourceTitle = s.FundSourceTitle }), "FundSourceID", "FundSourceTitle");
            model.FundSourceID = FundSourceIDHidden;

            return PartialView("AppropriationSource/_DynamicFundSource", model);
        }
        //Get AppropriationSource Partial View
        public ActionResult GetAddAppropSource()
        {
            AppropSourceModel model = new AppropSourceModel();
            return PartialView("AppropriationSource/_AddAppropriationSource", model);
        }
        ////Add AppropriationSource
        public JsonResult AddNewAppropSoure(AppropSourceModel model)
        {
            Tbl_FMAppropriationSource tbl_AppropriationSource = new Tbl_FMAppropriationSource();
            
            tbl_AppropriationSource.FundSourceID = GlobalFunction.ReturnEmptyInt(model.FundSourceID);
            tbl_AppropriationSource.Description = GlobalFunction.ReturnEmptyString(model.getAppropriationSourceColumns.Description);
            tbl_AppropriationSource.BudgetYearID = GlobalFunction.ReturnEmptyInt(model.BudgetYearID);
            BOSSDB.Tbl_FMAppropriationSource.Add(tbl_AppropriationSource);

            BOSSDB.SaveChanges();

            return Json(tbl_AppropriationSource);
        }
        //Get AppropriationSource Update Partial View
        public ActionResult Get_UpdateAppropSource(AppropSourceModel model, int AppropriationID)
        {
            Tbl_FMAppropriationSource tblAP = (from e in BOSSDB.Tbl_FMAppropriationSource where e.AppropriationID == AppropriationID select e).FirstOrDefault();

            model.AppropriationID = AppropriationID;
            model.FundSourceIDHidden = Convert.ToInt32(tblAP.FundSourceID);
            model.AppropSourceTypeID = Convert.ToInt32(tblAP.Tbl_FMFundSource.AppropSourceTypeID);
            model.getAppropriationSourceColumns.Description = tblAP.Description;
            model.BudgetYearID = Convert.ToInt32(tblAP.BudgetYearID);
            return PartialView("AppropriationSource/_UpdateAppropriationSource", model);
        }
        //Update AppropriationSource
        public ActionResult UpdateAppropSource(AppropSourceModel model)
        {
            Tbl_FMAppropriationSource tbl_AppropriationSource = (from e in BOSSDB.Tbl_FMAppropriationSource where e.AppropriationID == model.AppropriationID select e).FirstOrDefault();

            tbl_AppropriationSource.FundSourceID = GlobalFunction.ReturnEmptyInt(model.FundSourceID);
            tbl_AppropriationSource.Description = GlobalFunction.ReturnEmptyString(model.getAppropriationSourceColumns.Description);
            tbl_AppropriationSource.BudgetYearID = GlobalFunction.ReturnEmptyInt(model.BudgetYearID);
            BOSSDB.Entry(tbl_AppropriationSource);
            BOSSDB.SaveChanges();
            return PartialView("AppropriationSource/_AddAppropriationSource", model);
        }
        //Delete AppropriationSource
        public ActionResult DeleteAppropriationSource(AppropSourceModel model, int AppropriationID)
        {
            Tbl_FMAppropriationSource tbl_AppropriationSource = (from e in BOSSDB.Tbl_FMAppropriationSource where e.AppropriationID == AppropriationID select e).FirstOrDefault();
            BOSSDB.Tbl_FMAppropriationSource.Remove(tbl_AppropriationSource);
            BOSSDB.SaveChanges();
            return RedirectToAction("FileAppropFundSource");
        }
    }
}