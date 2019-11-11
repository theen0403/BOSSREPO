using BOSS.GlobalFunctions;
using BOSS.Models;
using BOSS.Models.FMmodels.FMFundModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class FileMaintenanceFundController : Controller
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        DatatypeValidation GlobalFunction = new DatatypeValidation();
        [Authorize]
        // GET: FileMaintenanceFund
        public ActionResult FileFund()
        {
            FundModel model = new FundModel();
            return View();
        }
        //Fund Tab
        public ActionResult GetFundTab()
        {
            FundModel model = new FundModel();
            return PartialView("FundTab/IndexFundTab", model);
        }
        //SubFund Tab
        public ActionResult GetSubFundTab()
        {
            SubFundModel model = new SubFundModel();
            return PartialView("SubFundTab/IndexSubFundTab", model);
        }
      
        //------------------------------------------------------------------------------------------------
        //Fund Tab
        //------------------------------------------------------------------------------------------------
        //Display Data Table
        public ActionResult GetFundDTable()
        {
            FundModel model = new FundModel();

            List<FundList> getFundList = new List<FundList>();

            var SQLQuery = "SELECT * FROM [Tbl_FMFund]";
            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_Fund]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        getFundList.Add(new FundList()
                        {
                            FundID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            FundTitle = GlobalFunction.ReturnEmptyString(dr[1]),
                            FundCode = GlobalFunction.ReturnEmptyString(dr[2])
                        });
                    }
                }
                Connection.Close();
            }
            model.getFundList = getFundList.ToList();

            return PartialView("FundTab/_TableFund", model.getFundList);
        }
        public ActionResult GetFundForm(int ActionID, int FundID)
        {
            FundModel model = new FundModel();

            if (ActionID == 2)
            {
                var fund = (from a in BOSSDB.Tbl_FMFund where a.FundID == FundID select a).FirstOrDefault();
                model.FundList.FundTitle = fund.FundTitle;
                model.FundList.FundCode = fund.FundCode;
                model.FundList.FundID = fund.FundID;
            }
            model.ActionID = ActionID;
            return PartialView("FundTab/_FundForm", model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveFund(FundModel model)
        {
            var isExist = "";
            if (ModelState.IsValid)
            {
                var fundTitle = model.FundList.FundTitle;
                fundTitle = Regex.Replace(fundTitle, @"\s\s+", "");
                fundTitle = Regex.Replace(fundTitle, @"^\s+", "");
                fundTitle = Regex.Replace(fundTitle, @"\s+$", "");
                fundTitle = new CultureInfo("en-US").TextInfo.ToTitleCase(fundTitle);
                Tbl_FMFund checkFund = (from a in BOSSDB.Tbl_FMFund where (a.FundTitle == fundTitle || a.FundCode == model.FundList.FundCode) select a).FirstOrDefault();

                if (model.ActionID == 1)
                {
                    if (checkFund == null)
                    {
                        Tbl_FMFund fund = new Tbl_FMFund();
                        fund.FundTitle = fundTitle;
                        fund.FundCode = model.FundList.FundCode;
                        BOSSDB.Tbl_FMFund.Add(fund);
                        BOSSDB.SaveChanges();
                        isExist = "false";
                    }
                    else if (checkFund != null)
                    {
                        isExist = "true";
                    }
                }
                else if (model.ActionID == 2)
                {
                    Tbl_FMFund fund = (from a in BOSSDB.Tbl_FMFund where a.FundID == model.FundList.FundID select a).FirstOrDefault();
                    List<Tbl_FMFund> fundTitlelist = (from e in BOSSDB.Tbl_FMFund where e.FundTitle == fundTitle select e).ToList();
                    List<Tbl_FMFund> fundCode = (from e in BOSSDB.Tbl_FMFund where e.FundCode == model.FundList.FundCode select e).ToList();
                    if (checkFund != null)
                    {
                        if(fundTitlelist == 1 || fundCode == 1) { }

                        if (fund.FundTitle == fundTitle && fund.FundCode == model.FundList.FundCode)
                        {
                            fund.FundTitle = fundTitle;
                            fund.FundCode = model.FundList.FundCode;
                            BOSSDB.Entry(fund);
                            BOSSDB.SaveChanges();
                            isExist = "justUpdate";
                            isExist = "true";
                        }
                        else
                        {
                            isExist = "true";
                        }
                    }
                    else if (checkFund == null)
                    {
                        fund.FundTitle = fundTitle;
                        fund.FundCode = model.FundList.FundCode;
                        BOSSDB.Entry(fund);
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
        public ActionResult ConfirmDeleteFund(int PrimaryID)
        {
            Tbl_FMFund fund = (from a in BOSSDB.Tbl_FMFund where a.FundID == PrimaryID select a).FirstOrDefault();
            BOSSDB.Tbl_FMFund.Remove(fund);
            BOSSDB.SaveChanges();
            //var sql = "SELECT DISTINCT TABLE_NAME FROM Information_Schema.Columns WHERE COLUMN_NAME = 'FundID' AND TABLE_NAME<> 'Tbl_FMFund'";

            //if (sql != null)
            //{
            //    TempData["Message"] = "With Data.";
            //}
            //else
            //{
            //    TempData["Message"] = "Without Data.";
            //}
            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteFund(int PrimaryID)
        {
            Tbl_FMFund fund = (from a in BOSSDB.Tbl_FMFund where a.FundID == PrimaryID select a).FirstOrDefault();
            Tbl_FMSubFund subFund = (from a in BOSSDB.Tbl_FMSubFund where a.FundID == PrimaryID select a).FirstOrDefault();
            Tbl_FMDepartment dept = (from e in BOSSDB.Tbl_FMDepartment where e.FundID == PrimaryID select e).FirstOrDefault();
            var confirmDelete = "";
            if (fund != null)
            {
                if (dept != null)
                {
                    confirmDelete = "restricted";
                }
                else if (subFund != null)
                {
                    confirmDelete = "true";
                }
                else
                {
                    confirmDelete = "false";
                }
            }
            var result = new { confirmDelete = confirmDelete };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //------------------------------------------------------------------------------------------------
        //Sub Fund Tab
        //------------------------------------------------------------------------------------------------
        //Display Data Table
        public ActionResult GetSubFundDTable()
        {
            SubFundModel model = new SubFundModel(); 
            List<SubFundList> getSubFundList = new List<SubFundList>();

            var SQLQuery = "SELECT [SubFundID], [SubFundTitle], [Tbl_FMFund].[FundTitle] FROM [BOSS].[dbo].[Tbl_FMSubFund],[dbo].[Tbl_FMFund] where [dbo].[Tbl_FMSubFund].FundID =[dbo].[Tbl_FMFund].FundID";
            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_Fund]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        getSubFundList.Add(new SubFundList()
                        {
                            SubFundID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            FundTitle = GlobalFunction.ReturnEmptyString(dr[2]),
                            SubFundTitle = GlobalFunction.ReturnEmptyString(dr[1])
                        });
                    }
                }
                Connection.Close();
            }
            model.getSubFundList = getSubFundList.ToList();

            return PartialView("SubFundTab/_TableSubFund", model.getSubFundList);
        }
        public ActionResult GetSubFundForm(int ActionID, int SubFundID)
        {
            SubFundModel model = new SubFundModel();

            if (ActionID == 2)
            {
                var sfund = (from a in BOSSDB.Tbl_FMSubFund where a.SubFundID == SubFundID select a).FirstOrDefault();
                model.SubFundList.SubFundTitle = sfund.SubFundTitle;
                model.SubFundList.FundID = Convert.ToInt32(sfund.FundID);
                model.SubFundList.SubFundID = sfund.SubFundID;
            }
            model.ActionID = ActionID;
            return PartialView("SubFundTab/_SubFundForm", model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSubFund(SubFundModel model)
        {
            var isExist = "";
            if (ModelState.IsValid)
            {
                var subfundTitle = model.SubFundList.SubFundTitle;
                subfundTitle = Regex.Replace(subfundTitle, @"\s\s+", " ");
                subfundTitle = Regex.Replace(subfundTitle, @"^\s+", "");
                subfundTitle = Regex.Replace(subfundTitle, @"\s+$", "");
                subfundTitle = new CultureInfo("en-US").TextInfo.ToTitleCase(subfundTitle);

                Tbl_FMSubFund checksubFund = (from a in BOSSDB.Tbl_FMSubFund where (a.FundID == model.SubFundList.FundID || a.SubFundTitle == model.SubFundList.SubFundTitle) select a).FirstOrDefault();

                if (model.ActionID == 1)
                {
                    if (checksubFund == null)
                    {
                        Tbl_FMSubFund sfund = new Tbl_FMSubFund();
                        sfund.SubFundTitle = subfundTitle;
                        sfund.FundID = model.SubFundList.FundID;
                        BOSSDB.Tbl_FMSubFund.Add(sfund);
                        BOSSDB.SaveChanges();
                        isExist = "false";
                    }
                    else if (checksubFund != null)
                    {
                        isExist = "true";
                    }
                }
                else if (model.ActionID == 2)
                {
                    Tbl_FMSubFund subfund = (from a in BOSSDB.Tbl_FMSubFund where a.SubFundID == model.SubFundList.SubFundID select a).FirstOrDefault();
                    if (checksubFund != null)
                    {
                        if (subfund.SubFundTitle == subfundTitle || subfund.FundID == model.SubFundList.FundID) //walang binago 
                        {
                            subfund.SubFundTitle = subfundTitle;
                            subfund.FundID = GlobalFunction.ReturnEmptyInt(model.SubFundList.FundID);
                            BOSSDB.Entry(subfund);
                            BOSSDB.SaveChanges();
                            isExist = "justUpdate";
                        }
                        else
                        {
                            isExist = "true";
                        }
                    }
                    else if (checksubFund == null)
                    {
                        subfund.SubFundTitle = subfundTitle;
                        subfund.FundID = model.SubFundList.FundID;
                        BOSSDB.Entry(subfund);
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
        public ActionResult DeleteSubFund(int SubFundID)
        {
            Tbl_FMSubFund subFundOne = (from a in BOSSDB.Tbl_FMSubFund where a.SubFundID == SubFundID select a).FirstOrDefault();
            BOSSDB.Tbl_FMSubFund.Remove(subFundOne);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}