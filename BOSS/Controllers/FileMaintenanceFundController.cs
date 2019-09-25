using BOSS.GlobalFunctions;
using BOSS.Models;
using BOSS.Models.FMmodels.FMFundModels;
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
        //Calling per TAB
        public ActionResult FundTab()
        {
            FundModel model = new FundModel();
            return PartialView("FundTab/IndexFundTab", model);
        }
        public ActionResult SubFundTab()
        {
            SubFundModel model = new SubFundModel();
            return PartialView("SubFundTab/IndexSubFundTab", model);
        }
        //========================================================
                                                        //Fund Tab
        //Display Data Table
        public ActionResult GetFundDTable()
        {
            FundModel model = new FundModel();

            List<FundList> getFundList = new List<FundList>();

            var SQLQuery = "SELECT * FROM [Fund]";
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
        //Get Add Partial View
        public ActionResult Get_AddFund()
        {
            FundModel model = new FundModel();
            return PartialView("FundTab/_AddFund", model);
        }
        //Add Fund function
        public JsonResult AddNewFund(FundModel model)
        {
            Fund FundTbl = new Fund();

            FundTbl.FundTitle = GlobalFunction.ReturnEmptyString(model.getFundColumns.FundTitle);
            FundTbl.FundCode = GlobalFunction.ReturnEmptyString(model.getFundColumns.FundCode);
            BOSSDB.Funds.Add(FundTbl);
            
            BOSSDB.SaveChanges();
            return Json(FundTbl);
        }
        //Get Update Partial View
        public ActionResult Get_UpdateFund(FundModel model, int FundID)
        {
            Fund tblFund = (from e in BOSSDB.Funds where e.FundID == FundID select e).FirstOrDefault();
            
            model.getFundColumns.FundTitle = tblFund.FundTitle;
            model.getFundColumns.FundCode = tblFund.FundCode;
            model.FundID = FundID;
            return PartialView("FundTab/_UpdateFund", model);
        }
        //Update Function
        public ActionResult UpdateFund(FundModel model)
        {
            Fund fundTBL = (from e in BOSSDB.Funds where e.FundID == model.FundID select e).FirstOrDefault();

            fundTBL.FundTitle = GlobalFunction.ReturnEmptyString(model.getFundColumns.FundTitle);
            fundTBL.FundCode = GlobalFunction.ReturnEmptyString(model.getFundColumns.FundCode);
            BOSSDB.Entry(fundTBL);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Delete Function
        public ActionResult DeleteFund(FundModel model, int FundID)
        {
            List<SubFund> subfund = (from e in BOSSDB.SubFunds where e.FundID == FundID select e).ToList();
            if (subfund != null)
            {
                foreach (var items in subfund)
                {
                    BOSSDB.SubFunds.Remove(items);
                    BOSSDB.SaveChanges();
                }
            }
            
            Fund fundtbl = (from e in BOSSDB.Funds where e.FundID == FundID select e).FirstOrDefault();
            BOSSDB.Funds.Remove(fundtbl);
            BOSSDB.SaveChanges();
            return RedirectToAction("FileFund");
        }
        //========================================================
                                                    //Sub-Fund Tab
        //Display Data Table
        public ActionResult GetSubFundDTable()
        {
            SubFundModel model = new SubFundModel();
            List<SubFundList> getSubFundList = new List<SubFundList>();

            var SQLQuery = "SELECT [SubFundID], [SubFundTitle], [Fund].[FundTitle] FROM [BOSS].[dbo].[SubFund],[dbo].[Fund] where [dbo].[SubFund].FundID =[dbo].[Fund].FundID";
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
        //Get Add Partial View
        public ActionResult Get_AddSubFund()
        {
            SubFundModel model = new SubFundModel();
            return PartialView("SubFundTab/_AddSubFund", model);
        }
        //Add Sub-Fund function
        public JsonResult AddSubNewSubFund(SubFundModel model)
        {
            SubFund SubFundTbl = new SubFund();

            SubFundTbl.SubFundTitle = GlobalFunction.ReturnEmptyString(model.getSubFundColumns.SubFundTitle);
            SubFundTbl.FundID = GlobalFunction.ReturnEmptyInt(model.FundID);
            BOSSDB.SubFunds.Add(SubFundTbl);

            BOSSDB.SaveChanges();
            return Json(SubFundTbl);
        }
        //Get Update Partial View
        public ActionResult Get_UpdateSubFund(SubFundModel model, int SubFundID)
        {
            SubFund tblsubFund = (from e in BOSSDB.SubFunds where e.SubFundID == SubFundID select e).FirstOrDefault();

            model.getSubFundColumns.SubFundTitle = tblsubFund.SubFundTitle;
            model.FundID = Convert.ToInt32(tblsubFund.FundID);
            model.SubFundID = SubFundID;
            return PartialView("SubFundTab/_UpdateSubFund", model);
        }
        //Update Function
        public ActionResult UpdateSubFund(SubFundModel model)
        {
            SubFund subfundTBL = (from e in BOSSDB.SubFunds where e.SubFundID == model.SubFundID select e).FirstOrDefault();

            subfundTBL.SubFundTitle = GlobalFunction.ReturnEmptyString(model.getSubFundColumns.SubFundTitle);
            subfundTBL.FundID = GlobalFunction.ReturnEmptyInt(model.FundID);
            BOSSDB.Entry(subfundTBL);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Delete Function
        public ActionResult DeleteSubFund(SubFundModel model, int SubFundID)
        {
            SubFund subfundtbl = (from e in BOSSDB.SubFunds where e.SubFundID == SubFundID select e).FirstOrDefault();
            BOSSDB.SubFunds.Remove(subfundtbl);
            BOSSDB.SaveChanges();
            return RedirectToAction("FileFund");
        }
        
        //Get Add Fund Modal
        public ActionResult GetAddFundModal()
        {
            return PartialView("FundTab/IndexFundTab");
        }
    }
}