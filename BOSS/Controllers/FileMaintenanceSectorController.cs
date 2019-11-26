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

            var SQLQuery = "SELECT * FROM [Tbl_FMSector_Sector]";
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
        public ActionResult GetSectorForm(int ActionID, int PrimaryID)
        {
            SectorModel model = new SectorModel();

            if (ActionID == 2)
            {
                var sector = (from a in BOSSDB.Tbl_FMSector_Sector where a.SectorID == PrimaryID select a).FirstOrDefault();
                model.SectorList.SectorTitle = sector.SectorTitle;
                model.SectorList.SectorCode = sector.SectorCode;
                model.SectorList.SectorID = sector.SectorID;
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

                Tbl_FMSector_Sector checkSector = (from a in BOSSDB.Tbl_FMSector_Sector where (a.SectorTitle == SectorTitle || a.SectorCode == model.SectorList.SectorCode) select a).FirstOrDefault();

                if (model.ActionID == 1)
                {
                    if (checkSector == null)
                    {
                        Tbl_FMSector_Sector Sector = new Tbl_FMSector_Sector();
                        Sector.SectorTitle = SectorTitle;
                        Sector.SectorCode = model.SectorList.SectorCode;
                        BOSSDB.Tbl_FMSector_Sector.Add(Sector);
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
                    Tbl_FMSector_Sector Sector = (from a in BOSSDB.Tbl_FMSector_Sector where a.SectorID == model.SectorList.SectorID select a).FirstOrDefault();
                    List<Tbl_FMSector_Sector> sectorTitlelist = (from e in BOSSDB.Tbl_FMSector_Sector where e.SectorTitle == SectorTitle select e).ToList();
                    List<Tbl_FMSector_Sector> sectorCode = (from e in BOSSDB.Tbl_FMSector_Sector where e.SectorCode == model.SectorList.SectorCode select e).ToList();
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
                            if (Sector.SectorTitle != SectorTitle && sectorTitlelist.Count >= 1 || Sector.SectorCode != model.SectorList.SectorCode && sectorCode.Count >= 1)
                            {
                                isExist = "true";
                            }
                            else
                            {
                                Sector.SectorTitle = SectorTitle;
                                Sector.SectorCode = model.SectorList.SectorCode;
                                BOSSDB.Entry(Sector);
                                BOSSDB.SaveChanges();
                                isExist = "justUpdate";
                            }
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
        public ActionResult DeleteSector(int PrimaryID)
        {
            Tbl_FMSector_Sector sector = (from a in BOSSDB.Tbl_FMSector_Sector where a.SectorID == PrimaryID select a).FirstOrDefault();
            Tbl_FMSector_SubSector subSector = (from a in BOSSDB.Tbl_FMSector_SubSector where a.SectorID == PrimaryID select a).FirstOrDefault();

            Tbl_FMRes_Department dept = (from e in BOSSDB.Tbl_FMRes_Department where e.SectorID == PrimaryID select e).FirstOrDefault();
            Tbl_FMRes_Function func = (from e in BOSSDB.Tbl_FMRes_Function where e.SectorID == PrimaryID select e).FirstOrDefault();
            var confirmDelete = "";
            if (sector != null)
            {
                if (dept != null || func != null)
                {
                    confirmDelete = "restricted";
                }
                else if (subSector != null)
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
        public ActionResult ConfirmDelete(int PrimaryID)
        {
            Tbl_FMSector_Sector sector = (from a in BOSSDB.Tbl_FMSector_Sector where a.SectorID == PrimaryID select a).FirstOrDefault();
            BOSSDB.Tbl_FMSector_Sector.Remove(sector);
            BOSSDB.SaveChanges();

            var result = "";
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

            var SQLQuery = "SELECT [SubSectorID],[Tbl_FMSector_Sector].SectorTitle,[SubSectorTitle],[SectorCode]+ ' - '+[SubSectorCode] as CombinedCode FROM [BOSS].[dbo].[Tbl_FMSector_SubSector],[dbo].[Tbl_FMSector_Sector] where [Tbl_FMSector_SubSector].SectorID = [Tbl_FMSector_Sector].SectorID";
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
        public ActionResult GetSubSectorForm(int ActionID, int PrimaryID)
        {
            SubSectorModel model = new SubSectorModel();

            if (ActionID == 2)
            {
                var Subsector = (from a in BOSSDB.Tbl_FMSector_SubSector where a.SubSectorID == PrimaryID select a).FirstOrDefault();
                model.SubSectorList.SectorID = Convert.ToInt32(Subsector.SectorID);
                model.SubSectorList.SubSectorTitle = Subsector.SubSectorTitle;
                model.SubSectorList.SubSectorCode = Subsector.SubSectorCode;
                model.SubSectorList.SubSectorID = Subsector.SubSectorID;
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
                SubSectorTitle = Regex.Replace(SubSectorTitle, @"\s\s+", "");
                SubSectorTitle = Regex.Replace(SubSectorTitle, @"^\s+", "");
                SubSectorTitle = Regex.Replace(SubSectorTitle, @"\s+$", "");
                SubSectorTitle = new CultureInfo("en-US").TextInfo.ToTitleCase(SubSectorTitle);
                Tbl_FMSector_SubSector checksubSector = (from a in BOSSDB.Tbl_FMSector_SubSector where (a.SubSectorTitle == SubSectorTitle || a.SubSectorCode == model.SubSectorList.SubSectorCode) select a).FirstOrDefault();

                if (model.ActionID == 1)
                {
                    if (checksubSector == null)
                    {
                        Tbl_FMSector_SubSector subSector = new Tbl_FMSector_SubSector();
                        subSector.SectorID = model.SubSectorList.SectorID;
                        subSector.SubSectorTitle = SubSectorTitle;
                        subSector.SubSectorCode = model.SubSectorList.SubSectorCode;
                        BOSSDB.Tbl_FMSector_SubSector.Add(subSector);
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
                    Tbl_FMSector_SubSector subSector = (from a in BOSSDB.Tbl_FMSector_SubSector where a.SubSectorID == model.SubSectorList.SubSectorID select a).FirstOrDefault();

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
        public ActionResult DeleteSubSector(int PrimaryID)
        {
            Tbl_FMSector_SubSector subSector = (from a in BOSSDB.Tbl_FMSector_SubSector where a.SubSectorID == PrimaryID select a).FirstOrDefault();

            Tbl_FMRes_Department dept = (from e in BOSSDB.Tbl_FMRes_Department where e.SubSectorID == PrimaryID select e).FirstOrDefault();
            Tbl_FMRes_Function func = (from e in BOSSDB.Tbl_FMRes_Function where e.SubSectorID == PrimaryID select e).FirstOrDefault();
            var confirmDelete = "";
            if (subSector != null)
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
        public ActionResult ConfirmDeleteSubFund(int PrimaryID)
        {
            Tbl_FMSector_SubSector subsector = (from a in BOSSDB.Tbl_FMSector_SubSector where a.SubSectorID == PrimaryID select a).FirstOrDefault();
            BOSSDB.Tbl_FMSector_SubSector.Remove(subsector);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSectorCodeField(int SectorID, SubSectorModel model)
        {
            var sector = (from a in BOSSDB.Tbl_FMSector_Sector where a.SectorID == SectorID select a).FirstOrDefault();
            var passCon = "";
            model.SectorCode = sector.SectorCode;
            passCon = model.SectorCode;

            var result = new { passCon = passCon }; ;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}