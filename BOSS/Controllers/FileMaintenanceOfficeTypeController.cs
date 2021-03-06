﻿using BOSS.GlobalFunctions;
using BOSS.Models;
using BOSS.Models.FMmodels.FMOfficeTypeModels;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class FileMaintenanceOfficeTypeController : Controller
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        DatatypeValidation GlobalFunction = new DatatypeValidation();
        [Authorize]
        // GET: FileMaintenanceOfficeType
        public ActionResult FileOfficeType()
        {
            OfficeTypeModel model = new OfficeTypeModel();
            return View();
        }
        public ActionResult GetOfficeTypeForm(int ActionID, int PrimaryID)
        {
            OfficeTypeModel model = new OfficeTypeModel();

            if (ActionID == 2)
            {
                var officeType = (from e in BOSSDB.Tbl_FMOfficeType where e.OfficeTypeID == PrimaryID select e).FirstOrDefault();
                model.OfficeTypeList.OfficeTypeTitle = officeType.OfficeTypeTitle;
                model.OfficeTypeList.OfficeTypeCode = officeType.OfficeTypeCode;
                model.OfficeTypeList.OfficeTypeID = officeType.OfficeTypeID;
            }
            model.ActionID = ActionID;
            return PartialView("_OfficeTypeForm", model);
        }
        public ActionResult GetOfficeTypeDTable()
        {
            OfficeTypeModel model = new OfficeTypeModel();

            List<OfficeTypeList> getOfficeTypeList = new List<OfficeTypeList>();

            var SQLQuery = "SELECT * FROM [Tbl_FMOfficeType]";
            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_OfficeType]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        getOfficeTypeList.Add(new OfficeTypeList()
                        {
                            OfficeTypeID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            OfficeTypeTitle = GlobalFunction.ReturnEmptyString(dr[1]),
                            OfficeTypeCode = GlobalFunction.ReturnEmptyString(dr[2])
                        });
                    }
                }
                Connection.Close();
            }
            model.getOfficeTypeList = getOfficeTypeList.ToList();

            return PartialView("_TableOfficeType", model.getOfficeTypeList);
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOfficeType(OfficeTypeModel model)
        {
            var isExist = "";
            if (ModelState.IsValid)
            {
                var offtypetitle = model.OfficeTypeList.OfficeTypeTitle;
                offtypetitle = Regex.Replace(offtypetitle, @"\s\s+", "");
                offtypetitle = Regex.Replace(offtypetitle, @"^\s+", "");
                offtypetitle = Regex.Replace(offtypetitle, @"\s+$", "");
                offtypetitle = new CultureInfo("en-US").TextInfo.ToTitleCase(offtypetitle);
                Tbl_FMOfficeType checkofficetype = (from a in BOSSDB.Tbl_FMOfficeType where (a.OfficeTypeTitle == offtypetitle || a.OfficeTypeCode == model.OfficeTypeList.OfficeTypeCode) select a).FirstOrDefault();

                if (model.ActionID == 1)
                {
                    if (checkofficetype == null)
                    {
                        Tbl_FMOfficeType OfficeTypeTbl = new Tbl_FMOfficeType();
                        OfficeTypeTbl.OfficeTypeTitle = GlobalFunction.ReturnEmptyString(model.OfficeTypeList.OfficeTypeTitle);
                        OfficeTypeTbl.OfficeTypeCode = GlobalFunction.ReturnEmptyString(model.OfficeTypeList.OfficeTypeCode);

                        BOSSDB.Tbl_FMOfficeType.Add(OfficeTypeTbl);
                        BOSSDB.SaveChanges();
                        isExist = "false";
                    }
                    else if (checkofficetype != null)
                    {
                        isExist = "true";
                    }
                }
                else if (model.ActionID == 2)
                {
                    Tbl_FMOfficeType officetype = (from e in BOSSDB.Tbl_FMOfficeType where e.OfficeTypeID == model.OfficeTypeList.OfficeTypeID select e).FirstOrDefault();
                    List<Tbl_FMOfficeType> officeTitle = (from e in BOSSDB.Tbl_FMOfficeType where e.OfficeTypeTitle == offtypetitle select e).ToList();
                    List<Tbl_FMOfficeType> officeCode = (from e in BOSSDB.Tbl_FMOfficeType where e.OfficeTypeCode == model.OfficeTypeList.OfficeTypeCode select e).ToList();

                    if (checkofficetype != null)
                    {
                        if (officetype.OfficeTypeTitle == offtypetitle && officetype.OfficeTypeCode == model.OfficeTypeList.OfficeTypeCode) //walang binago 
                        {
                            officetype.OfficeTypeTitle = offtypetitle;
                            officetype.OfficeTypeCode = model.OfficeTypeList.OfficeTypeCode;
                            BOSSDB.Entry(officetype);
                            BOSSDB.SaveChanges();
                            isExist = "justUpdate";
                        }
                        else
                        {
                            if (officetype.OfficeTypeTitle != offtypetitle && officeTitle.Count >= 1 || officetype.OfficeTypeCode != model.OfficeTypeList.OfficeTypeCode && officeCode.Count >= 1)
                            {
                                isExist = "true";
                            }
                            else
                            {
                                officetype.OfficeTypeTitle = offtypetitle;
                                officetype.OfficeTypeCode = model.OfficeTypeList.OfficeTypeCode;
                                BOSSDB.Entry(officetype);
                                BOSSDB.SaveChanges();
                                isExist = "justUpdate";
                            }
                        }
                    }
                    else if (checkofficetype == null)
                    {
                        officetype.OfficeTypeTitle = offtypetitle;
                        officetype.OfficeTypeCode = model.OfficeTypeList.OfficeTypeCode;
                        BOSSDB.Entry(officetype);
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

        public ActionResult DeleteOfficeType(int PrimaryID)
        {
            Tbl_FMOfficeType officetype = (from a in BOSSDB.Tbl_FMOfficeType where a.OfficeTypeID == PrimaryID select a).FirstOrDefault();
            Tbl_FMRes_Department dept = (from e in BOSSDB.Tbl_FMRes_Department where e.OfficeTypeID == PrimaryID select e).FirstOrDefault();
            Tbl_FMRes_Function func = (from e in BOSSDB.Tbl_FMRes_Function where e.OfficeTypeID == PrimaryID select e).FirstOrDefault();
            var confirmDelete = "";
            if (officetype != null)
            {
                if (dept != null || func != null)
                {
                    confirmDelete = "restricted";
                }
                else
                {
                    confirmDelete = "false";
                }
            }
            var result = new { confirmDelete = confirmDelete };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConfirmDelete(int PrimaryID)
        {
           Tbl_FMOfficeType officetype = (from a in BOSSDB.Tbl_FMOfficeType where a.OfficeTypeID == PrimaryID select a).FirstOrDefault();
            BOSSDB.Tbl_FMOfficeType.Remove(officetype);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        ////Display Data Table

        ////Get Add Partial View
        //public ActionResult Get_AddOfficeType()
        //{
        //    OfficeTypeModel model = new OfficeTypeModel();
        //    return PartialView("_AddOfficeType", model);
        //}
        ////Add OfficeType function
        //public JsonResult AddNewOfficeType(OfficeTypeModel model)
        //{
        //    Tbl_FMOfficeType OfficeTypeTbl = new Tbl_FMOfficeType();
        //    OfficeTypeTbl.OfficeTypeTitle = GlobalFunction.ReturnEmptyString(model.getOfficeTypeColumns.OfficeTypeTitle);
        //    OfficeTypeTbl.OfficeTypeCode = GlobalFunction.ReturnEmptyString(model.getOfficeTypeColumns.OfficeTypeCode);

        //    BOSSDB.Tbl_FMOfficeType.Add(OfficeTypeTbl);

        //    BOSSDB.SaveChanges();
        //    return Json(OfficeTypeTbl);
        //}
        ////Get Update Partial View
        //public ActionResult Get_UpdateOfficeType(OfficeTypeModel model, int OfficeTypeID)
        //{
        //    Tbl_FMOfficeType tblOfficeType = (from e in BOSSDB.Tbl_FMOfficeType where e.OfficeTypeID == OfficeTypeID select e).FirstOrDefault();

        //    model.getOfficeTypeColumns.OfficeTypeTitle = tblOfficeType.OfficeTypeTitle;
        //    model.getOfficeTypeColumns.OfficeTypeCode = tblOfficeType.OfficeTypeCode;
        //    model.OfficeTypeID = OfficeTypeID;
        //    return PartialView("_UpdateOfficeType", model);
        //}
        ////Update Function
        //public ActionResult UpdateOfficeType(OfficeTypeModel model)
        //{
        //    Tbl_FMOfficeType OfficeTypeTBL = (from e in BOSSDB.Tbl_FMOfficeType where e.OfficeTypeID == model.OfficeTypeID select e).FirstOrDefault();

        //    OfficeTypeTBL.OfficeTypeTitle = GlobalFunction.ReturnEmptyString(model.getOfficeTypeColumns.OfficeTypeTitle);
        //    OfficeTypeTBL.OfficeTypeCode = GlobalFunction.ReturnEmptyString(model.getOfficeTypeColumns.OfficeTypeCode);
        //    BOSSDB.Entry(OfficeTypeTBL);
        //    BOSSDB.SaveChanges();

        //    var result = "";
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        ////Delete Function
        //public ActionResult DeleteOfficeType(OfficeTypeModel model, int OfficeTypeID)
        //{
        //    Tbl_FMOfficeType OfficeTypetbl = (from e in BOSSDB.Tbl_FMOfficeType where e.OfficeTypeID == OfficeTypeID select e).FirstOrDefault();
        //    BOSSDB.Tbl_FMOfficeType.Remove(OfficeTypetbl);
        //    BOSSDB.SaveChanges();
        //    return RedirectToAction("FileOfficeType");
        //}
    }

}