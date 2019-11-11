using BOSS.GlobalFunctions;
using BOSS.Models;
using BOSS.Models.FMmodels.FMSectorModels;
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
    public class FileMaintenanceSectorController : Controller
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        DatatypeValidation GlobalFunction = new DatatypeValidation();
        [Authorize]
        // GET: FileMaintenanceSector
        public ActionResult FileSector()
        {
            SectorModel model = new SectorModel();
            return View();
        }
        public ActionResult GetSectorTab()
        {
            SectorModel model = new SectorModel();
            return PartialView("SectorTab/IndexSectorTab", model);
        }
        public ActionResult GetSubSectorTab()
        {
            SubSectorModel model = new SubSectorModel();
            return PartialView("SubSectorTab/IndexSubSectorTab");
        }
        //---------------------------------------------------------------------------------------------------
        //Sector Tab
        //---------------------------------------------------------------------------------------------------
        //Display Data Table
        public ActionResult GetSectorDTable()
        {
            SectorModel model = new SectorModel();

            List<SectorList> getSectorList = new List<SectorList>();

            var SQLQuery = "SELECT * FROM [Tbl_FMSector]";
            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_Sector]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        getSectorList.Add(new SectorList()
                        {
                            SectorID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            SectorTitle = GlobalFunction.ReturnEmptyString(dr[1]),
                            SectorCode = GlobalFunction.ReturnEmptyString(dr[2])
                        });
                    }
                }
                Connection.Close();
            }
            model.getSectorList = getSectorList.ToList();

            return PartialView("SectorTab/_TableSector", model.getSectorList);
        }
        public ActionResult GetSectorForm(int ActionID, int SectorID)
        {
            SectorModel model = new SectorModel();

            if (ActionID == 2)
            {
                var sector = (from a in BOSSDB.Tbl_FMSector where a.SectorID == SectorID select a).FirstOrDefault();
                model.SectorList.SectorTitle = sector.SectorTitle;
                model.SectorList.SectorCode = sector.SectorCode;
                model.SectorList.SectorID = SectorID;
            }
            model.ActionID = ActionID;
            return PartialView("SectorTab/_SectorForm", model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSector(SectorModel model)
        {
            var isExist = "";
            if (ModelState.IsValid)
            {
                var SectorTitle = model.SectorList.SectorTitle;
                SectorTitle = Regex.Replace(SectorTitle, @"\s\s+", "");
                SectorTitle = Regex.Replace(SectorTitle, @"^\s+", "");
                SectorTitle = Regex.Replace(SectorTitle, @"\s+$", "");
                SectorTitle = new CultureInfo("en-US").TextInfo.ToTitleCase(SectorTitle);

                Tbl_FMSector checkSector = (from a in BOSSDB.Tbl_FMSector where (a.SectorTitle == SectorTitle && a.SectorCode == model.SectorList.SectorCode) select a).FirstOrDefault();

                if (model.ActionID == 1)
                {
                    if (checkSector == null)
                    {
                        Tbl_FMSector Sector = new Tbl_FMSector();
                        Sector.SectorTitle = SectorTitle;
                        Sector.SectorCode = model.SectorList.SectorCode;
                        BOSSDB.Tbl_FMSector.Add(Sector);
                        BOSSDB.SaveChanges();
                        isExist = "false";
                    }
                    else if (checkSector != null)
                    {
                        isExist = "true";
                    }
                }
                else if (model.ActionID == 2)
                {
                    Tbl_FMSector Sector = (from a in BOSSDB.Tbl_FMSector where a.SectorID == model.SectorList.SectorID select a).FirstOrDefault();

                    if (checkSector != null)
                    {
                        if (Sector.SectorTitle == SectorTitle && Sector.SectorCode == model.SectorList.SectorCode) //walang binago 
                        {
                            Sector.SectorTitle = SectorTitle;
                            Sector.SectorCode = model.SectorList.SectorCode;
                            BOSSDB.Entry(Sector);
                            BOSSDB.SaveChanges();
                            isExist = "justUpdate";
                        }
                        else
                        {
                            isExist = "true";
                        }
                    }
                    else if (checkSector == null)
                    {
                        Sector.SectorTitle = SectorTitle;
                        Sector.SectorCode = model.SectorList.SectorCode;
                        BOSSDB.Entry(Sector);
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
        public ActionResult ConfirmDeleteSector(int SectorID)
        {
            Tbl_FMSector Sector = (from a in BOSSDB.Tbl_FMSector where a.SectorID == SectorID select a).FirstOrDefault();
            List<Tbl_FMSubSector> subSector = (from e in BOSSDB.Tbl_FMSubSector where e.SectorID == SectorID select e).ToList();
            if (subSector.Count == 1)
            {
                Tbl_FMSubSector subSectorOne = (from a in BOSSDB.Tbl_FMSubSector where a.SectorID == SectorID select a).FirstOrDefault();
                BOSSDB.Tbl_FMSubSector.Remove(subSectorOne);
                BOSSDB.Tbl_FMSector.Remove(Sector);
                BOSSDB.SaveChanges();

            }
            else if (subSector.Count > 1)
            {
                foreach (var items in subSector)
                {
                    BOSSDB.Tbl_FMSubSector.Remove(items);
                }
                BOSSDB.Tbl_FMSector.Remove(Sector);
                BOSSDB.SaveChanges();
            }

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteSector(int SectorID)
        {
            Tbl_FMSector Sector = (from a in BOSSDB.Tbl_FMSector where a.SectorID == SectorID select a).FirstOrDefault();
            Tbl_FMSubSector subSector = (from a in BOSSDB.Tbl_FMSubSector where a.SectorID == SectorID select a).FirstOrDefault();
            var confirmDeleteSector = "";
            if (Sector != null)
            {
                if (subSector != null)
                {
                    confirmDeleteSector = "confirm";
                }
                else
                {
                    BOSSDB.Tbl_FMSector.Remove(Sector);
                    BOSSDB.SaveChanges();
                    confirmDeleteSector = "delete";
                }
            }
            var result = new { confirmDeleteSector = confirmDeleteSector };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //---------------------------------------------------------------------------------------------------
        //Sub-Sector Tab
        //---------------------------------------------------------------------------------------------------
        //Display Data Table
        public ActionResult GetSubSectorDTable()
        {
            SubSectorModel model = new SubSectorModel();

            List<SubSectorList> getSubSectorList = new List<SubSectorList>();

            var SQLQuery = "SELECT [SubSectorID],[Tbl_FMSector].SectorTitle,[SubSectorTitle],[SectorCode]+ ' - '+[SubSectorCode] as CombinedCode FROM [BOSS].[dbo].[Tbl_FMSubSector],[dbo].[Tbl_FMSector] where [Tbl_FMSubSector].SectorID = [Tbl_FMSector].SectorID";
            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_Sector]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        getSubSectorList.Add(new SubSectorList()
                        {
                            SubSectorID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            SectorTitle = GlobalFunction.ReturnEmptyString(dr[1]),
                            SubSectorTitle = GlobalFunction.ReturnEmptyString(dr[2]),
                            CombinedCode = GlobalFunction.ReturnEmptyString(dr[3])
                        });
                    }
                }
                Connection.Close();
            }
            model.getSubSectorList = getSubSectorList.ToList();

            return PartialView("SubSectorTab/_TableSubSector", model.getSubSectorList);
        }
        public ActionResult GetSubSectorForm(int ActionID, int SubSectorID)
        {
            SubSectorModel model = new SubSectorModel();

            if (ActionID == 2)
            {
                var Subsector = (from a in BOSSDB.Tbl_FMSubSector where a.SubSectorID == SubSectorID select a).FirstOrDefault();
                model.SubSectorList.SectorID = Convert.ToInt32(Subsector.SectorID);
                model.SubSectorList.SubSectorTitle = Subsector.SubSectorTitle;
                model.SubSectorList.SubSectorCode = Subsector.SubSectorCode;
                model.SubSectorList.SubSectorID = SubSectorID;
            }
            model.ActionID = ActionID;
            return PartialView("SubSectorTab/_SubSectorForm", model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSubSector(SubSectorModel model)
        {
            var isExist = "";
            if (ModelState.IsValid)
            {
                var SubSectorTitle = model.SubSectorList.SubSectorTitle;
                var subsecVal = "N/A";
                if (SubSectorTitle != null)
                {
                    subsecVal = Regex.Replace(SubSectorTitle, @"\s\s+", "");
                    subsecVal = Regex.Replace(SubSectorTitle, @"^\s+", "");
                    subsecVal = Regex.Replace(SubSectorTitle, @"\s+$", "");
                    subsecVal = new CultureInfo("en-US").TextInfo.ToTitleCase(SubSectorTitle);
                }
                SubSectorTitle = subsecVal;
                Tbl_FMSubSector checksubSector = (from a in BOSSDB.Tbl_FMSubSector where (a.SubSectorTitle == SubSectorTitle && a.SubSectorCode == model.SubSectorList.SubSectorCode) select a).FirstOrDefault();

                if (model.ActionID == 1)
                {
                    if (checksubSector == null)
                    {
                        Tbl_FMSubSector subSector = new Tbl_FMSubSector();
                        subSector.SectorID = model.SubSectorList.SectorID;
                        subSector.SubSectorTitle = SubSectorTitle;
                        subSector.SubSectorCode = model.SubSectorList.SubSectorCode;
                        BOSSDB.Tbl_FMSubSector.Add(subSector);
                        BOSSDB.SaveChanges();
                        isExist = "false";
                    }
                    else if (checksubSector != null)
                    {
                        isExist = "true";
                    }
                }
                else if (model.ActionID == 2)
                {
                    Tbl_FMSubSector subSector = (from a in BOSSDB.Tbl_FMSubSector where a.SubSectorID == model.SubSectorList.SubSectorID select a).FirstOrDefault();

                    if (checksubSector != null)
                    {
                        if (subSector.SubSectorTitle == SubSectorTitle && subSector.SubSectorCode == model.SubSectorList.SubSectorCode) //walang binago 
                        {
                            subSector.SectorID = model.SubSectorList.SectorID;
                            subSector.SubSectorTitle = SubSectorTitle;
                            subSector.SubSectorCode = model.SubSectorList.SubSectorCode;
                            BOSSDB.Entry(subSector);
                            BOSSDB.SaveChanges();
                            isExist = "justUpdate";
                        }
                        else
                        {
                            isExist = "true";
                        }
                    }
                    else if (checksubSector == null)
                    {
                        subSector.SectorID = model.SubSectorList.SectorID;
                        subSector.SubSectorTitle = SubSectorTitle;
                        subSector.SubSectorCode = model.SubSectorList.SubSectorCode;
                        BOSSDB.Entry(subSector);
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
        public ActionResult DeleteSubSector(int SubSectorID)
        {
            Tbl_FMSubSector subSector = (from a in BOSSDB.Tbl_FMSubSector where a.SubSectorID == SubSectorID select a).FirstOrDefault();
            BOSSDB.Tbl_FMSubSector.Remove(subSector);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSectorCodeField(int SectorID, SubSectorModel model)
        {
            var sector = (from a in BOSSDB.Tbl_FMSector where a.SectorID == SectorID select a).FirstOrDefault();
            var passCon = "";
            model.SectorCode = sector.SectorCode;
            passCon = model.SectorCode;

            var result = new { passCon = passCon }; ;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}