using BOSS.GlobalFunctions;
using BOSS.Models;
using BOSS.Models.FMmodels.FMAppropriarionSouceModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
        //=============================================================================================================================
        public ActionResult GetFundSourceTab()
        {
            FundSourceModel model = new FundSourceModel();
            return PartialView("FundSource/FundSourceIndex", model);
        }
        public ActionResult GetAppSourceTab()
        {
            AppropSourceModel model = new AppropSourceModel();
            
            return PartialView("AppropriationSource/AppropriationSourceIndex", model);
        }
        //=============================================================================================================================
        //Fund Source Tab
        //=============================================================================================================================
        public ActionResult GetFundSourceDTable()
        {
            FundSourceModel model = new FundSourceModel();

            List<FundSourceList> getFundSourceList = new List<FundSourceList>();

            var SQLQuery = "SELECT [FundSourceID],[FundSourceTitle],[AppropSourceTypeTitle] FROM [BOSS].[dbo].[Tbl_FMApprop_FundSource], [dbo].[FMApprop_AppropriationSourceType] where [dbo].[Tbl_FMApprop_FundSource].AppropSourceTypeID = [dbo].[FMApprop_AppropriationSourceType].AppropSourceTypeID";
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
        public ActionResult GetFundSourceForm(int ActionID, int PrimaryID)
        {
            FundSourceModel model = new FundSourceModel();

            if (ActionID == 2)
            {
                var fundsource = (from a in BOSSDB.Tbl_FMApprop_FundSource where a.FundSourceID == PrimaryID select a).FirstOrDefault();
             
                model.AppropSourceTypeID = Convert.ToInt32(fundsource.AppropSourceTypeID);
                model.FundSourceList.FundSourceTitle = fundsource.FundSourceTitle;
                model.FundSourceList.FundSourceID = fundsource.FundSourceID;
            }
            model.ActionID = ActionID;
            return PartialView("FundSource/_FundSourceForm", model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveFundSource(FundSourceModel model)
        {
            var isExist = "";
            if (ModelState.IsValid)
            {
                var fundsourceTitle = model.FundSourceList.FundSourceTitle;
                fundsourceTitle = Regex.Replace(fundsourceTitle, @"\s\s+", "");
                fundsourceTitle = Regex.Replace(fundsourceTitle, @"^\s+", "");
                fundsourceTitle = Regex.Replace(fundsourceTitle, @"\s+$", "");
                fundsourceTitle = new CultureInfo("en-US").TextInfo.ToTitleCase(fundsourceTitle);
                Tbl_FMApprop_FundSource checkFundsource = (from a in BOSSDB.Tbl_FMApprop_FundSource where (a.FundSourceTitle == fundsourceTitle) select a).FirstOrDefault();

                if (model.ActionID == 1)
                {
                    if (checkFundsource == null)
                    {
                        Tbl_FMApprop_FundSource fundsource = new Tbl_FMApprop_FundSource();
                        fundsource.FundSourceTitle = fundsourceTitle;
                        fundsource.AppropSourceTypeID = model.AppropSourceTypeID;
                        BOSSDB.Tbl_FMApprop_FundSource.Add(fundsource);
                        BOSSDB.SaveChanges();
                        isExist = "false";
                    }
                    else if (checkFundsource != null)
                    {
                        isExist = "true";
                    }
                }
                else if (model.ActionID == 2)
                {
                    Tbl_FMApprop_FundSource fundS = (from a in BOSSDB.Tbl_FMApprop_FundSource where a.FundSourceID == model.FundSourceList.FundSourceID select a).FirstOrDefault();
                    List<Tbl_FMApprop_FundSource> fundsourcetitle = (from e in BOSSDB.Tbl_FMApprop_FundSource where e.FundSourceTitle == fundsourceTitle select e).ToList();
                    if (checkFundsource != null)
                    {
                        if (fundS.FundSourceTitle != fundsourceTitle && fundsourcetitle.Count >= 1)
                        {
                            isExist = "true";
                        }
                        else
                        {
                            fundS.FundSourceTitle = fundsourceTitle;
                            fundS.AppropSourceTypeID = model.AppropSourceTypeID;
                            BOSSDB.Entry(fundS);
                            BOSSDB.SaveChanges();
                            isExist = "justUpdate";
                        }
                    }
                    else if (checkFundsource == null)
                    {
                        fundS.FundSourceTitle = fundsourceTitle;
                        fundS.AppropSourceTypeID = model.AppropSourceTypeID;
                        BOSSDB.Entry(fundS);
                        BOSSDB.SaveChanges();
                        isExist = "justUpdate";
                    }
                }
            }
            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new { isExist = isExist }
            };
        }
        public ActionResult DeleteFundSource(int PrimaryID)
        {
            Tbl_FMApprop_FundSource fSource = (from a in BOSSDB.Tbl_FMApprop_FundSource where a.FundSourceID == PrimaryID select a).FirstOrDefault();
         
            var confirmDelete = "";
            if (fSource != null)
            {
                confirmDelete = "false";
            }
            var result = new { confirmDelete = confirmDelete };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConfirmDelete(int PrimaryID)
        {
            Tbl_FMApprop_FundSource fundSource = (from a in BOSSDB.Tbl_FMApprop_FundSource where a.FundSourceID == PrimaryID select a).FirstOrDefault();
            BOSSDB.Tbl_FMApprop_FundSource.Remove(fundSource);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //=============================================================================================================================
        //Apprpriation Source Tab
        //=============================================================================================================================
        //Get Appropriation Source Datatable Partial View
        public ActionResult GetAppropSourceDTable()
        {
            AppropSourceModel model = new AppropSourceModel();

            List<AppropSourceList> getAppropSourceList = new List<AppropSourceList>();

            var SQLQuery = "SELECT [AppropriationID], [AppropSourceTypeTitle], [FundSourceTitle], [Description], [BudgetYearTitle] FROM [Tbl_FMApprop_AppropriationSource], [Tbl_FMApprop_FundSource], [FMApprop_AppropriationSourceType], [FMApprop_BudgetYear] where [FMApprop_BudgetYear].BudgetYearID = [dbo].Tbl_FMApprop_AppropriationSource.BudgetYearID and [Tbl_FMApprop_AppropriationSource].FundSourceID = [Tbl_FMApprop_FundSource].FundSourceID and [Tbl_FMApprop_FundSource].AppropSourceTypeID = [FMApprop_AppropriationSourceType].AppropSourceTypeID";
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
                        getAppropSourceList.Add(new AppropSourceList()
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

        public ActionResult onChangeFundSource(AppropSourceModel model , int AppropSourceTypeID)
        {
            var FoundSourceClass = (from a in BOSSDB.Tbl_FMApprop_FundSource where a.AppropSourceTypeID == AppropSourceTypeID select a).ToList();
            return Json(new SelectList(FoundSourceClass, "FundSourceID", "FundSourceTitle"), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAppropSourceForm(int ActionID, int PrimaryID)
        {
            AppropSourceModel model = new AppropSourceModel();

            if (ActionID == 2)
            {
                var appropSource = (from a in BOSSDB.Tbl_FMApprop_AppropriationSource where a.AppropriationID == PrimaryID select a).FirstOrDefault();

                model.FundSourceID = Convert.ToInt32(appropSource.FundSourceID);
                model.AppropSourceTypeID = Convert.ToInt32(appropSource.Tbl_FMApprop_FundSource.AppropSourceTypeID);
                model.AppropSourceList.Description = appropSource.Description;
                model.BudgetYearID = Convert.ToInt32(appropSource.BudgetYearID);
                model.AppropSourceList.AppropriationID = appropSource.AppropriationID;

                var fundsourceTBL = (from a in BOSSDB.Tbl_FMApprop_FundSource orderby a.FundSourceTitle where a.AppropSourceTypeID == model.AppropSourceTypeID select a).ToList();

                if (fundsourceTBL.Count > 0)
                {
                    model.FundSourceList = new SelectList(fundsourceTBL, "FundSourceID", "FundSourceTitle");
                }
            }
            else
            {
                var appropSourceTypeTBL = (from a in BOSSDB.FMApprop_AppropriationSourceType orderby a.AppropSourceTypeTitle select a.AppropSourceTypeID).FirstOrDefault();
                var fundsourceTBL = (from a in BOSSDB.Tbl_FMApprop_FundSource orderby a.FundSourceTitle where a.AppropSourceTypeID == appropSourceTypeTBL select a).ToList();
                if (fundsourceTBL.Count > 0)
                {
                    model.FundSourceList = new SelectList(fundsourceTBL, "FundSourceID", "FundSourceTitle");
                }
            }
            model.FundSourceList = (from li in model.FundSourceList orderby li.Text select li).ToList();

            model.ActionID = ActionID;
            return PartialView("AppropriationSource/_AppropSourceForm", model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAppropSource(AppropSourceModel model)
        {
            var isExist = "";
            if (ModelState.IsValid)
            {
                var appropDesc = model.AppropSourceList.Description;
                appropDesc = Regex.Replace(appropDesc, @"\s\s+", "");
                appropDesc = Regex.Replace(appropDesc, @"^\s+", "");
                appropDesc = Regex.Replace(appropDesc, @"\s+$", "");
                appropDesc = new CultureInfo("en-US").TextInfo.ToTitleCase(appropDesc);
                Tbl_FMApprop_AppropriationSource checkApprop = (from a in BOSSDB.Tbl_FMApprop_AppropriationSource where (a.Description == model.AppropSourceList.Description) select a).FirstOrDefault();

                if (model.ActionID == 1)
                {
                    if (checkApprop == null)
                    {
                        Tbl_FMApprop_AppropriationSource appropS = new Tbl_FMApprop_AppropriationSource();
                        appropS.Description = appropDesc;
                        appropS.FundSourceID = model.FundSourceID;
                        appropS.BudgetYearID = model.BudgetYearID;
                        BOSSDB.Tbl_FMApprop_AppropriationSource.Add(appropS);
                        BOSSDB.SaveChanges();
                        isExist = "false";
                    }
                    else if (checkApprop != null)
                    {
                        isExist = "true";
                    }
                }
                else if (model.ActionID == 2)
                {
                    Tbl_FMApprop_AppropriationSource approp = (from a in BOSSDB.Tbl_FMApprop_AppropriationSource where a.AppropriationID == model.AppropSourceList.AppropriationID select a).FirstOrDefault();
                    List<Tbl_FMApprop_AppropriationSource> appropDescription = (from e in BOSSDB.Tbl_FMApprop_AppropriationSource where e.Description == appropDesc select e).ToList();
                    if (checkApprop != null)
                    {
                        if (approp.Description == appropDesc)
                        {
                            approp.Description = appropDesc;
                            approp.FundSourceID = model.FundSourceID;
                            approp.BudgetYearID = model.BudgetYearID;
                            BOSSDB.Entry(approp);
                            BOSSDB.SaveChanges();
                            isExist = "justUpdate";
                        }
                        else
                        {
                            if (approp.Description != appropDesc && appropDescription.Count >= 1 )
                            {
                                isExist = "true";
                            }
                            else
                            {
                                approp.Description = appropDesc;
                                approp.FundSourceID = model.FundSourceID;
                                approp.BudgetYearID = model.BudgetYearID;
                                BOSSDB.Entry(approp);
                                BOSSDB.SaveChanges();
                                isExist = "justUpdate";
                            }
                        }
                    }
                    else if (checkApprop == null)
                    {
                        approp.Description = appropDesc;
                        approp.FundSourceID = model.FundSourceID;
                        approp.BudgetYearID = model.BudgetYearID;
                        BOSSDB.Entry(approp);
                        BOSSDB.SaveChanges();
                        isExist = "justUpdate";
                    }
                }
            }
            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new { isExist = isExist }
            };
        }
        public ActionResult DeleteAppropSource(int AppropriationID)
        {
            Tbl_FMApprop_AppropriationSource ASource = (from a in BOSSDB.Tbl_FMApprop_AppropriationSource where a.AppropriationID == AppropriationID select a).FirstOrDefault();
            BOSSDB.Tbl_FMApprop_AppropriationSource.Remove(ASource);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}