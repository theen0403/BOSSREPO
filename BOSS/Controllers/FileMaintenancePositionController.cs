using BOSS.GlobalFunctions;
using BOSS.Models;
using BOSS.Models.FMmodels.FMPositionModels;
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
    public class FileMaintenancePositionController : Controller
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        DatatypeValidation GlobalFunction = new DatatypeValidation();
        // GET: FileMaintenancePosition
        [Authorize]
        public ActionResult FilePosition()
        {
            PositionModel model = new PositionModel();
            return View();
        }
        public ActionResult GetPositionSPDTable()
        {
            PositionModel model = new PositionModel();

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
        public ActionResult GetPositionForm(int ActionID, int PrimaryID)
        {
            PositionModel model = new PositionModel();

            if (ActionID == 2)
            {
                var position = (from a in BOSSDB.Tbl_FMPosition where a.PositionID == PrimaryID select a).FirstOrDefault();

                model.PositionList.PositionTitle = position.PositionTitle;
                model.PositionList.PositionCode = position.PositionCode;
                model.PositionList.PositionID = position.PositionID;
            }
            model.ActionID = ActionID;
            return PartialView("_PositionForm", model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SavePosition(PositionModel model)
        {
            var isExist = "";
            if (ModelState.IsValid)
            {
                var postitle = model.PositionList.PositionTitle;
                postitle = new CultureInfo("en-US").TextInfo.ToTitleCase(postitle);
                model.PositionList.PositionCode = new CultureInfo("en-US").TextInfo.ToUpper(model.PositionList.PositionCode);
                Tbl_FMPosition checkpos = (from a in BOSSDB.Tbl_FMPosition where (a.PositionTitle == postitle || a.PositionCode == model.PositionList.PositionCode) select a).FirstOrDefault();

                if (model.ActionID == 1)
                {
                    if (checkpos == null)
                    {
                        Tbl_FMPosition position = new Tbl_FMPosition();
                        position.PositionTitle = postitle;
                        position.PositionCode = model.PositionList.PositionCode;
                        BOSSDB.Tbl_FMPosition.Add(position);
                        BOSSDB.SaveChanges();
                        isExist = "false";
                    }
                    else if (checkpos != null)
                    {
                        isExist = "true";
                    }
                }
                else if (model.ActionID == 2)
                {
                    Tbl_FMPosition position = (from a in BOSSDB.Tbl_FMPosition where a.PositionID == model.PositionList.PositionID select a).FirstOrDefault();
                    List<Tbl_FMPosition> positiontitle = (from e in BOSSDB.Tbl_FMPosition where e.PositionTitle == postitle select e).ToList();
                    List<Tbl_FMPosition> positioncode = (from e in BOSSDB.Tbl_FMPosition where e.PositionCode == model.PositionList.PositionCode select e).ToList();
                    if (checkpos != null)
                    {
                        if (position.PositionTitle == postitle && position.PositionCode == model.PositionList.PositionCode)
                        {
                            position.PositionTitle = postitle;
                            position.PositionCode = model.PositionList.PositionCode;
                            BOSSDB.Entry(position);
                            BOSSDB.SaveChanges();
                            isExist = "justUpdate";
                        }
                        else
                        {
                            if (position.PositionTitle != postitle && positiontitle.Count >= 1 || position.PositionCode != model.PositionList.PositionCode && positioncode.Count >= 1)
                            {
                                isExist = "true";
                            }
                            else
                            {
                                position.PositionTitle = postitle;
                                position.PositionCode = model.PositionList.PositionCode;
                                BOSSDB.Entry(position);
                                BOSSDB.SaveChanges();
                                isExist = "justUpdate";
                            }
                        }
                    }
                    else if (checkpos == null)
                    {
                        position.PositionTitle = postitle;
                        position.PositionCode = model.PositionList.PositionCode;
                        BOSSDB.Entry(position);
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

        public ActionResult DeletePosition(int PrimaryID)
        {
            Tbl_FMPosition poss = (from a in BOSSDB.Tbl_FMPosition where a.PositionID == PrimaryID select a).FirstOrDefault();
            Tbl_FMSignatory sign = (from e in BOSSDB.Tbl_FMSignatory where e.PositionID == PrimaryID select e).FirstOrDefault();
            var confirmDelete = "";
            if (poss != null)
            {
                if (sign != null)
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
            Tbl_FMPosition positionTbl = (from e in BOSSDB.Tbl_FMPosition where e.PositionID == PrimaryID select e).FirstOrDefault();
            BOSSDB.Tbl_FMPosition.Remove(positionTbl);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}