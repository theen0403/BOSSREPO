using BOSS.GlobalFunctions;
using BOSS.Models;
using BOSS.Models.FMPositionforSPModels;
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
    public class FileMaintenancePositionController : Controller
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        DatatypeValidation GlobalFunction = new DatatypeValidation();
        // GET: FileMaintenancePosition
        [Authorize]
        public ActionResult FilePosition()
        {
            PositionforSPModel model = new PositionforSPModel();
            return View();
        }
        public ActionResult GetPositionSPDTable()
        {
            PositionforSPModel model = new PositionforSPModel();

            List<PositionList> getPositionList = new List<PositionList>();

            var SQLQuery = "SELECT * FROM [Tbl_FMPosition]";
            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_PositionSP]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        getPositionList.Add(new PositionList()
                        {
                            PositionID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            PositionTitle = GlobalFunction.ReturnEmptyString(dr[1]),
                            PositionCode = GlobalFunction.ReturnEmptyString(dr[2])
                        });
                    }
                }
                Connection.Close();
            }
            model.getPositionList = getPositionList.ToList();

            return PartialView("_TablePositionSP", model.getPositionList);
        }
        public ActionResult GetAddPositionSP()
        {
            PositionforSPModel model = new PositionforSPModel();
            return PartialView("_AddPosition", model);
        }
        //public ActionResult GetAddDynamicClassification(int PSID)
        //{
        //    PositionforSPModel model = new PositionforSPModel();

        //    model.PositionClassList = new SelectList((from s in BOSSDB.PositionClassifications.Where(a => a.PSID == PSID).ToList() select new { PCID = s.PCID, PCTitle = s.PCTitle }), "PCID", "PCTitle");

        //    return PartialView("_AddDynamicClassification", model);
        //}
        //public ActionResult GetUpdateDynamicClassification(int PSID,int PCID)
        //{
        //    PositionforSPModel model = new PositionforSPModel();

        //    model.PositionClassList = new SelectList((from s in BOSSDB.PositionClassifications.Where(a => a.PSID == PSID).ToList() select new { PCID = s.PCID, PCTitle = s.PCTitle }), "PCID", "PCTitle");
        //    model.PCID = PCID;
        //    return PartialView("_UpdateDynamicClassification", model);
        //}
        public JsonResult AddNewPosition(PositionforSPModel model)
        {
            Tbl_FMPosition PositionTBL = new Tbl_FMPosition();

            PositionTBL.PositionTitle = GlobalFunction.ReturnEmptyString(model.getPositionColumns.PositionTitle);
            PositionTBL.PositionCode = GlobalFunction.ReturnEmptyString(model.getPositionColumns.PositionCode);
            BOSSDB.Tbl_FMPosition.Add(PositionTBL);
            //PositionTBL.PCID = GlobalFunction.ReturnEmptyInt(model.PCID);


            BOSSDB.SaveChanges();
            return Json(PositionTBL);
        }
        public ActionResult Get_UpdatePosition(PositionforSPModel model, int PositionID)
        {
            Tbl_FMPosition tblposition = (from e in BOSSDB.Tbl_FMPosition where e.PositionID == PositionID select e).FirstOrDefault();

            model.getPositionColumns2.PositionTitle = tblposition.PositionTitle;
            model.getPositionColumns2.PositionCode = tblposition.PositionCode;

            //model.PSID = Convert.ToInt32(tblposition.PositionClassification.PSID);
            //model.PCIDTemp = Convert.ToInt32(tblposition.PCID);
            model.PositionID = PositionID;
            return PartialView("_UpdatePosition", model);
        }
        public ActionResult UpdatePosition(PositionforSPModel model)
        {
            Tbl_FMPosition positionTBL = (from e in BOSSDB.Tbl_FMPosition where e.PositionID == model.PositionID select e).FirstOrDefault();

            positionTBL.PositionTitle = GlobalFunction.ReturnEmptyString(model.getPositionColumns2.PositionTitle);
            positionTBL.PositionCode = GlobalFunction.ReturnEmptyString(model.getPositionColumns2.PositionCode);

            //positionTBL.PCID = GlobalFunction.ReturnEmptyInt(model.PCID);

            BOSSDB.Entry(positionTBL);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeletePosition(PositionforSPModel model, int PositionID)
        {
            Tbl_FMPosition positionTbl = (from e in BOSSDB.Tbl_FMPosition where e.PositionID == PositionID select e).FirstOrDefault();
            BOSSDB.Tbl_FMPosition.Remove(positionTbl);
            BOSSDB.SaveChanges();
            return RedirectToAction("FilePosition");
        }
    }
}