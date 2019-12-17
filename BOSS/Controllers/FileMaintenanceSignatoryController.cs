using BOSS.GlobalFunctions;
using BOSS.Models;
using BOSS.Models.FMmodels.FMSignatoryModels;
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

            List<SignatoryList> getSignatoryList = new List<SignatoryList>();

            var SQLQuery = "SELECT [SignatoryID], [SignatoryName], [PreferredName], [Division], Tbl_FMPosition.PositionTitle, [Tbl_FMRes_Department].DeptTitle, [Tbl_FMRes_Function].[FunctionTitle], [isHead], [isActive]  FROM [Tbl_FMSignatory], [Tbl_FMRes_Function], [Tbl_FMPosition], [Tbl_FMRes_Department] where[Tbl_FMRes_Department].DeptID = [Tbl_FMRes_Function].DeptID and[Tbl_FMSignatory].FunctionID = [Tbl_FMRes_Function].FunctionID and [Tbl_FMSignatory].PositionID = [Tbl_FMPosition].PositionID";
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
                        getSignatoryList.Add(new SignatoryList()
                        {
                            SignatoryID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            SignatoryName = GlobalFunction.ReturnEmptyString(dr[1]),
                            PreferredName = GlobalFunction.ReturnEmptyString(dr[2]),
                            PositionTitle = GlobalFunction.ReturnEmptyString(dr[4]),
                            DeptTitle = GlobalFunction.ReturnEmptyString(dr[5]),
                            isHead = GlobalFunction.ReturnEmptyBool(dr[7]),
                            FunctionTitle = GlobalFunction.ReturnEmptyString(dr[6]),
                            Division = GlobalFunction.ReturnEmptyString(dr[3]),
                            isActive = GlobalFunction.ReturnEmptyBool(dr[8])
                        });
                    }
                }
                Connection.Close();
            }
            model.getSignatoryList = getSignatoryList.ToList();
            return PartialView("_TableFileMaintenanceSignatory", model.getSignatoryList);
        }
        public ActionResult GetSignatoryForm(int ActionID, int PrimaryID)
        {
            SignatoryModel model = new SignatoryModel();
            if (ActionID == 2)
            {
                var signatory = (from a in BOSSDB.Tbl_FMSignatory where a.SignatoryID == PrimaryID select a).FirstOrDefault();
                model.SignatoryList.SignatoryName = signatory.SignatoryName;
                model.SignatoryList.PreferredName = signatory.PreferredName;
                model.SignatoryList.isHead = Convert.ToBoolean(signatory.isHead);
                
                model.PositionID = Convert.ToInt32(signatory.PositionID);
                model.DeptID = Convert.ToInt32(signatory.Tbl_FMRes_Function.DeptID);
                model.SignatoryList.Division = signatory.Division;
                model.FunctionID = Convert.ToInt32(signatory.FunctionID);
                model.SignatoryList.isActive = Convert.ToBoolean(signatory.isActive);
                model.SignatoryList.SignatoryID = signatory.SignatoryID;

                var functionCount = (from a in BOSSDB.Tbl_FMRes_Function orderby a.FunctionTitle where a.DeptID == model.DeptID select a).ToList();
                if (functionCount.Count > 0)
                {
                    model.FunctionList = new SelectList(functionCount, "FunctionID", "FunctionTitle");
                }
            }
            else
            {
                var deptTbl = (from a in BOSSDB.Tbl_FMRes_Department orderby a.DeptTitle select a.DeptID).FirstOrDefault();
                var functionCount = (from a in BOSSDB.Tbl_FMRes_Function orderby a.FunctionTitle where a.DeptID == deptTbl select a).ToList();
                if (functionCount.Count > 0)
                {
                    model.FunctionList = new SelectList(functionCount, "FunctionID", "FunctionTitle");
                }
            }
            model.FunctionList = (from li in model.FunctionList orderby li.Text select li).ToList();
            model.ActionID = ActionID;
            return PartialView("_SignatoryForm", model);
        }
        public ActionResult onChangeSignatory(SignatoryModel model, int DeptID)
        {
            var FunctionClass = (from a in BOSSDB.Tbl_FMRes_Function where a.DeptID == DeptID select a).ToList();
            return Json(new SelectList(FunctionClass, "FunctionID", "FunctionTitle"), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSignatory(SignatoryModel model)
        {
            var isExist = "";
            if (ModelState.IsValid)
            {
                var SigName = model.SignatoryList.SignatoryName;
                SigName = Regex.Replace(SigName, @"\s\s+", "");
                SigName = Regex.Replace(SigName, @"^\s+", "");
                SigName = Regex.Replace(SigName, @"\s+$", "");
                SigName = new CultureInfo("en-US").TextInfo.ToTitleCase(SigName);

                Tbl_FMSignatory checkSig = (from a in BOSSDB.Tbl_FMSignatory where (a.SignatoryName == SigName) select a).FirstOrDefault();

                if (model.ActionID == 1)
                {
                    if (checkSig == null)
                    {
                        Tbl_FMSignatory signatori = new Tbl_FMSignatory();
                        signatori.SignatoryName = SigName;
                        signatori.PreferredName = model.SignatoryList.PreferredName;
                        signatori.PositionID = model.PositionID;
                        signatori.FunctionID = model.FunctionID;
                        signatori.Division = model.SignatoryList.Division;
                        signatori.isHead = model.SignatoryList.isHead;
                        signatori.isActive = model.SignatoryList.isActive;
                        BOSSDB.Tbl_FMSignatory.Add(signatori);
                        BOSSDB.SaveChanges();
                        isExist = "false";
                    }
                    else if (checkSig != null)
                    {
                        isExist = "true";
                    }
                }
                else if (model.ActionID == 2)
                {
                    Tbl_FMSignatory signa = (from a in BOSSDB.Tbl_FMSignatory where a.SignatoryID == model.SignatoryList.SignatoryID select a).FirstOrDefault();
                    List<Tbl_FMSignatory> signatoriiName = (from e in BOSSDB.Tbl_FMSignatory where e.SignatoryName == SigName select e).ToList();
                    List<Tbl_FMSignatory> signatoriiPref = (from e in BOSSDB.Tbl_FMSignatory where e.PreferredName == model.SignatoryList.PreferredName select e).ToList();
                    if (checkSig != null)
                    {
                        if (signa.SignatoryName == SigName && signa.PreferredName == model.SignatoryList.PreferredName && signa.PositionID == model.PositionID)
                        {
                            isExist = "justUpdate";
                        }
                        else
                        {
                            if (signa.SignatoryName != SigName && signatoriiName.Count >= 1 || signa.PreferredName != SigName && signatoriiPref.Count >= 1)
                            {
                                isExist = "true";
                            }
                            else
                            {
                                isExist = "justUpdate";
                            }
                        }
                    }
                    else if (checkSig == null)
                    {
                        isExist = "justUpdate";
                    }
                    if (isExist == "justUpdate")
                    {
                        signa.SignatoryName = SigName;
                        signa.PreferredName = model.SignatoryList.PreferredName;
                        signa.PositionID = model.PositionID;
                        signa.FunctionID = model.FunctionID;
                        signa.Division = model.SignatoryList.Division;
                        signa.isHead = model.SignatoryList.isHead;
                        signa.isActive = model.SignatoryList.isActive;
                        BOSSDB.Entry(signa);
                        BOSSDB.SaveChanges();
                    }
                }
            }
            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new { isExist = isExist }
            };
        }
        public ActionResult DeleteSignatory(int PrimaryID)
        {
            Tbl_FMSignatory signatoryTbl = (from a in BOSSDB.Tbl_FMSignatory where a.SignatoryID == PrimaryID select a).FirstOrDefault();

            var confirmDelete = "";
            if (signatoryTbl != null)
            {
                confirmDelete = "false";
            }
            var result = new { confirmDelete = confirmDelete };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConfirmDelete(int PrimaryID)
        {
            Tbl_FMSignatory signatorii = (from a in BOSSDB.Tbl_FMSignatory where a.SignatoryID == PrimaryID select a).FirstOrDefault();
            BOSSDB.Tbl_FMSignatory.Remove(signatorii);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}