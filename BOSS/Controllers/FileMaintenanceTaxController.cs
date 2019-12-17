using BOSS.GlobalFunctions;
using BOSS.Models;
using BOSS.Models.FMmodels.FMTaxModels;
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
    public class FileMaintenanceTaxController : Controller
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        DatatypeValidation GlobalFunction = new DatatypeValidation();
        [Authorize]
        // GET: FileMaintenanceTax
        public ActionResult FileMaintenanceTax()
        {
            TaxModel model = new TaxModel();
            return View(model);
        }
        public ActionResult GetTaxForm(int ActionID, int PrimaryID)
        {
            TaxModel model = new TaxModel();
            if (ActionID == 2)
            {
                var taxeslist = (from a in BOSSDB.Tbl_FMTax where a.TaxID == PrimaryID select a).FirstOrDefault();
                model.TaxList.Description = taxeslist.Description;
                model.TaxList.ShortDescrption = taxeslist.ShortDepscription;
                model.TaxList.isUsed = Convert.ToBoolean(taxeslist.isUsed);
                model.TaxList.Percentage = taxeslist.Percentage;
                model.TaxList.BaseTax = taxeslist.BaseTax;
                model.GAID = Convert.ToInt32(taxeslist.GAID);
                model.TaxList.TaxID = taxeslist.TaxID;
            }
            model.ActionID = ActionID;
            return PartialView("_TaxForm", model);
        }
        public ActionResult GetTaxDTableForm()
        {
            TaxModel model = new TaxModel();

            List<TaxList> getTaxList = new List<TaxList>();

            var SQLQuery = "SELECT [TaxID], [Description], [Percentage], [BaseTax], Tbl_FMCOA_GeneralAccount.GATitle,[isUsed], [ShortDepscription] from [Tbl_FMTax], [Tbl_FMCOA_GeneralAccount] where[Tbl_FMCOA_GeneralAccount].GAID = [Tbl_FMTax].GAID";
            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_Tax]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        getTaxList.Add(new TaxList()
                        {
                            TaxID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            Description = GlobalFunction.ReturnEmptyString(dr[1]),
                            Percentage = GlobalFunction.ReturnEmptyString(dr[2]),
                            BaseTax = GlobalFunction.ReturnEmptyString(dr[3]),
                            GATitle = GlobalFunction.ReturnEmptyString(dr[4]),
                            isUsed = GlobalFunction.ReturnEmptyBool(dr[5]),
                            ShortDescrption = GlobalFunction.ReturnEmptyString(dr[6])
                        });
                    }
                }
                Connection.Close();
            }
            model.getTaxList = getTaxList.ToList();
            return PartialView("_TableTax", model.getTaxList);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveTax(TaxModel model)
        {
            var isExist = "";
            if (ModelState.IsValid)
            {
                var desc = model.TaxList.Description;
                var shortdesc = model.TaxList.ShortDescrption;

                Tbl_FMTax checkTax = (from a in BOSSDB.Tbl_FMTax where (a.Description == desc) select a).FirstOrDefault();

                if (model.ActionID == 1)
                {
                    if (checkTax == null)
                    {
                        Tbl_FMTax taxii = new Tbl_FMTax();
                        taxii.Description = desc;
                        taxii.ShortDepscription = model.TaxList.ShortDescrption;
                        taxii.isUsed = model.TaxList.isUsed;
                        taxii.Percentage = model.TaxList.Percentage;
                        taxii.BaseTax = model.TaxList.BaseTax;
                        taxii.GAID = model.GAID;
                        BOSSDB.Tbl_FMTax.Add(taxii);
                        BOSSDB.SaveChanges();
                        isExist = "false";
                    }
                    else if (checkTax != null)
                    {
                        isExist = "true";
                    }
                }
                else if (model.ActionID == 2)
                {
                    Tbl_FMTax tax = (from a in BOSSDB.Tbl_FMTax where a.TaxID == model.TaxList.TaxID select a).FirstOrDefault();
                    List<Tbl_FMTax> descCount = (from e in BOSSDB.Tbl_FMTax where e.Description == desc select e).ToList();
                    List<Tbl_FMTax> shortdescCount = (from e in BOSSDB.Tbl_FMTax where e.ShortDepscription == shortdesc select e).ToList();
                    if (checkTax != null)
                    {
                        if (tax.Description == desc && tax.ShortDepscription == model.TaxList.ShortDescrption && tax.TaxID == model.TaxList.TaxID)
                        {
                            isExist = "justUpdate";
                        }
                        else
                        {
                            if (tax.Description != desc && descCount.Count >= 1 || tax.ShortDepscription != shortdesc && shortdescCount.Count >= 1)
                            {
                                isExist = "true";
                            }
                            else
                            {
                                isExist = "justUpdate";
                            }
                        }
                    }
                    else if (checkTax == null)
                    {
                        isExist = "justUpdate";
                    }
                    if (isExist == "justUpdate")
                    {
                        tax.Description = desc;
                        tax.ShortDepscription = model.TaxList.ShortDescrption;
                        tax.isUsed = model.TaxList.isUsed;
                        tax.Percentage = model.TaxList.Percentage;
                        tax.BaseTax = model.TaxList.BaseTax;
                        tax.GAID = model.GAID;
                        BOSSDB.Entry(tax);
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
        public ActionResult DeleteTax(int PrimaryID)
        {
            Tbl_FMTax taxTbl = (from a in BOSSDB.Tbl_FMTax where a.TaxID == PrimaryID select a).FirstOrDefault();
            var confirmDelete = "";
            if (taxTbl != null)
            {
                confirmDelete = "false";
            }
            var result = new { confirmDelete = confirmDelete };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConfirmDeleteTax(int PrimaryID)
        {
            Tbl_FMTax taxiii = (from a in BOSSDB.Tbl_FMTax where a.TaxID == PrimaryID select a).FirstOrDefault();
            BOSSDB.Tbl_FMTax.Remove(taxiii);
            BOSSDB.SaveChanges();
            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}