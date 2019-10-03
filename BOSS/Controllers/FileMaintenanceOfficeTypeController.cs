using BOSS.GlobalFunctions;
using BOSS.Models;
using BOSS.Models.FMmodels.FMOfficeTypeModels;
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
        //Display Data Table
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
        //Get Add Partial View
        public ActionResult Get_AddOfficeType()
        {
            OfficeTypeModel model = new OfficeTypeModel();
            return PartialView("_AddOfficeType", model);
        }
        //Add OfficeType function
        public JsonResult AddNewOfficeType(OfficeTypeModel model)
        {
            Tbl_FMOfficeType OfficeTypeTbl = new Tbl_FMOfficeType();
            OfficeTypeTbl.OfficeTypeTitle = GlobalFunction.ReturnEmptyString(model.getOfficeTypeColumns.OfficeTypeTitle);
            OfficeTypeTbl.OfficeTypeCode = GlobalFunction.ReturnEmptyString(model.getOfficeTypeColumns.OfficeTypeCode);

            BOSSDB.Tbl_FMOfficeType.Add(OfficeTypeTbl);

            BOSSDB.SaveChanges();
            return Json(OfficeTypeTbl);
        }
        //Get Update Partial View
        public ActionResult Get_UpdateOfficeType(OfficeTypeModel model, int OfficeTypeID)
        {
            Tbl_FMOfficeType tblOfficeType = (from e in BOSSDB.Tbl_FMOfficeType where e.OfficeTypeID == OfficeTypeID select e).FirstOrDefault();

            model.getOfficeTypeColumns.OfficeTypeTitle = tblOfficeType.OfficeTypeTitle;
            model.getOfficeTypeColumns.OfficeTypeCode = tblOfficeType.OfficeTypeCode;
            model.OfficeTypeID = OfficeTypeID;
            return PartialView("_UpdateOfficeType", model);
        }
        //Update Function
        public ActionResult UpdateOfficeType(OfficeTypeModel model)
        {
            Tbl_FMOfficeType OfficeTypeTBL = (from e in BOSSDB.Tbl_FMOfficeType where e.OfficeTypeID == model.OfficeTypeID select e).FirstOrDefault();

            OfficeTypeTBL.OfficeTypeTitle = GlobalFunction.ReturnEmptyString(model.getOfficeTypeColumns.OfficeTypeTitle);
            OfficeTypeTBL.OfficeTypeCode = GlobalFunction.ReturnEmptyString(model.getOfficeTypeColumns.OfficeTypeCode);
            BOSSDB.Entry(OfficeTypeTBL);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Delete Function
        public ActionResult DeleteOfficeType(OfficeTypeModel model, int OfficeTypeID)
        {
            Tbl_FMOfficeType OfficeTypetbl = (from e in BOSSDB.Tbl_FMOfficeType where e.OfficeTypeID == OfficeTypeID select e).FirstOrDefault();
            BOSSDB.Tbl_FMOfficeType.Remove(OfficeTypetbl);
            BOSSDB.SaveChanges();
            return RedirectToAction("FileOfficeType");
        }
    }
   
}