using BOSS.GlobalFunctions;
using BOSS.Models;
using BOSS.Models.FMmodels.FMSignatoryModels;
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
    public class FileMaintenanceSignatoryController : Controller
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        DatatypeValidation GlobalFunction = new DatatypeValidation();
        // GET: FileMaintenanceSignatory
        [Authorize]
        public ActionResult FileSignatory()
        {
            SignatoryModel model = new SignatoryModel();
            return View(model);
        }
        //View Table For Signatory
        public ActionResult GetSignatoryDTable()
        {
            SignatoryModel model = new SignatoryModel();

            List<SignatoryDTList> getSignatoryList = new List<SignatoryDTList>();

            var SQLQuery = "SELECT [SignatoryID], [SignatoryName], [PreferredName], [Division], Tbl_FMPosition.PositionTitle, [Tbl_FMRes_Department].DeptTitle, [Tbl_FMSignatory].[FunctionID], [isHead], [isActive]  FROM [Tbl_FMSignatory], [Tbl_FMRes_Function], [Tbl_FMPosition], [Tbl_FMRes_Department] where[Tbl_FMRes_Department].DeptID = [Tbl_FMRes_Function].DeptID and[Tbl_FMSignatory].FunctionID = [Tbl_FMRes_Function].FunctionID and [Tbl_FMSignatory].PositionID = [Tbl_FMPosition].PositionID";
            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_Signatory]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        getSignatoryList.Add(new SignatoryDTList()
                        {
                            SignatoryID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            SignatoryName = GlobalFunction.ReturnEmptyString(dr[1]),
                            PreferredName = GlobalFunction.ReturnEmptyString(dr[2]),
                            PositionTitle = GlobalFunction.ReturnEmptyString(dr[4]),
                            DeptTitle = GlobalFunction.ReturnEmptyString(dr[5]),
                            isHead = GlobalFunction.ReturnEmptyInt(dr[7]),
                            FunctionID = GlobalFunction.ReturnEmptyInt(dr[6]),
                            Division = GlobalFunction.ReturnEmptyString(dr[3]),
                            isActive = GlobalFunction.ReturnEmptyInt(dr[8])
                        });
                    }
                }
                Connection.Close();
            }
            model.getSignatoryList = getSignatoryList.ToList();
            return PartialView("_TableFileMaintenanceSignatory", model.getSignatoryList);
        }
        public ActionResult GetAddSignatory()
        {
            SignatoryModel model = new SignatoryModel();
            return PartialView("_AddSignatory", model);
        }
        public ActionResult GetAddDynamicFunction(int DeptID)
        {
            SignatoryModel model = new SignatoryModel();

            model.FunctionList = new SelectList((from s in BOSSDB.Tbl_FMRes_Function.Where(a => a.DeptID == DeptID).ToList() select new { FunctionID = s.FunctionID, FunctionTitle = s.FunctionTitle }), "FunctionID", "FunctionTitle");

            return PartialView("_AddDynamicFunctionList", model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public JsonResult AddNewSignatory(SignatoryModel model)
        {
            Tbl_FMSignatory signatorytbl = new Tbl_FMSignatory();

            signatorytbl.SignatoryName = GlobalFunction.ReturnEmptyString(model.getSignatoryColumns.SignatoryName);
            signatorytbl.PreferredName = GlobalFunction.ReturnEmptyString(model.getSignatoryColumns.PreferredName);
            signatorytbl.isHead = model.isHead;
            signatorytbl.PositionID = GlobalFunction.ReturnEmptyInt(model.PositionID);
            signatorytbl.FunctionID = GlobalFunction.ReturnEmptyInt(model.FunctionID);
            signatorytbl.Division = GlobalFunction.ReturnEmptyString(model.getSignatoryColumns.Division);
            signatorytbl.isActive = model.isActive;
            BOSSDB.Tbl_FMSignatory.Add(signatorytbl);

            BOSSDB.SaveChanges();

            return Json(signatorytbl);
        }
        public ActionResult GetUpdateDynamicFunction(SignatoryModel model, int DeptID, int funcTempID)
        {
            model.FunctionList = new SelectList((from s in BOSSDB.Tbl_FMRes_Function.Where(a => a.DeptID == DeptID).ToList() select new { FunctionID = s.FunctionID, FunctionTitle = s.FunctionTitle }), "FunctionID", "FunctionTitle");
            model.FunctionID = funcTempID;
            return PartialView("_AddDynamicFunctionList", model);
        }
        public ActionResult Get_UpdateSignatory(SignatoryModel model, int SignatoryID)
        {
            Tbl_FMSignatory signatoryTable = (from e in BOSSDB.Tbl_FMSignatory where e.SignatoryID == SignatoryID select e).FirstOrDefault();

            model.getSignatoryColumns.SignatoryName = signatoryTable.SignatoryName;
            model.getSignatoryColumns.PreferredName = signatoryTable.PreferredName;
            model.isHead = Convert.ToBoolean(signatoryTable.isHead);
            model.PositionID = Convert.ToInt32(signatoryTable.PositionID);
            model.DeptID = Convert.ToInt32(signatoryTable.Tbl_FMRes_Function.DeptID);
            model.getSignatoryColumns.Division = signatoryTable.Division;
            model.funcTempID = Convert.ToInt32(signatoryTable.FunctionID);
            model.isActive = Convert.ToBoolean(signatoryTable.isActive);
            return PartialView("_UpdateSignatory", model);
        }
        public ActionResult UpdateSignatory(SignatoryModel model)
        {
            Tbl_FMSignatory signatorytbl = (from e in BOSSDB.Tbl_FMSignatory where e.SignatoryID == model.SignatoryID select e).FirstOrDefault();

            signatorytbl.SignatoryName = GlobalFunction.ReturnEmptyString(model.getSignatoryColumns.SignatoryName);
            signatorytbl.PositionID = GlobalFunction.ReturnEmptyInt(model.PositionID);
            signatorytbl.isHead = model.isHead;
            signatorytbl.PreferredName = GlobalFunction.ReturnEmptyString(model.getSignatoryColumns.PreferredName);
            signatorytbl.FunctionID = GlobalFunction.ReturnEmptyInt(model.FunctionID);
            signatorytbl.Division = GlobalFunction.ReturnEmptyString(model.getSignatoryColumns.Division);
            signatorytbl.isActive = model.isActive;
            BOSSDB.Entry(signatorytbl);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteSignatory(SignatoryModel model, int SignatoryID)
        {
            Tbl_FMSignatory signatorytbl = (from e in BOSSDB.Tbl_FMSignatory where e.SignatoryID == SignatoryID select e).FirstOrDefault();
            BOSSDB.Tbl_FMSignatory.Remove(signatorytbl);
            BOSSDB.SaveChanges();
            return RedirectToAction("FileSignatory");
        }
    }
}