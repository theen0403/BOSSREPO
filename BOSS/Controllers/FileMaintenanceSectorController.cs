using BOSS.GlobalFunctions;
using BOSS.Models;
using BOSS.Models.FMmodels.FMSectorModels;
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
        public ActionResult SectorTab()
        {
            SectorModel model = new SectorModel();
            return PartialView("SectorTab/IndexSectorTab", model);
        }
        public ActionResult SubSectorTab()
        {
            SubSectorModel model = new SubSectorModel();
            return PartialView("SubSectorTab/IndexSubSectorTab");
        }
        //========================================================
                                                        //Sector Tab
        //Display Data Table
        public ActionResult GetSectorDTable()
        {
            SectorModel model = new SectorModel();

            List<SectorList> getSectorList = new List<SectorList>();

            var SQLQuery = "SELECT * FROM [Sector]";
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
        //Get Add Partial View
        public ActionResult Get_AddSector()
        {
            SectorModel model = new SectorModel();
            return PartialView("SectorTab/_AddSector", model);
        }
        //Add Sector function
        public JsonResult AddNewSector(SectorModel model)
        {
            Sector SectorTbl = new Sector();

            SectorTbl.SectorTitle = GlobalFunction.ReturnEmptyString(model.getSectorColumns.SectorTitle);
            SectorTbl.SectorCode = GlobalFunction.ReturnEmptyString(model.getSectorColumns.SectorCode);
            BOSSDB.Sectors.Add(SectorTbl);

            BOSSDB.SaveChanges();
            return Json(SectorTbl);
        }
        //Get Update Partial View
        public ActionResult Get_UpdateSector(SectorModel model, int SectorID)
        {
            Sector tblSector = (from e in BOSSDB.Sectors where e.SectorID == SectorID select e).FirstOrDefault();

            model.getSectorColumns.SectorTitle = tblSector.SectorTitle;
            model.getSectorColumns.SectorCode = tblSector.SectorCode;
            model.SectorID = SectorID;
            return PartialView("SectorTab/_UpdateSector", model);
        }
        //Update Function
        public ActionResult UpdateSector(SectorModel model)
        {
            Sector SectorTBL = (from e in BOSSDB.Sectors where e.SectorID == model.SectorID select e).FirstOrDefault();

            SectorTBL.SectorTitle = GlobalFunction.ReturnEmptyString(model.getSectorColumns.SectorTitle);
            SectorTBL.SectorCode = GlobalFunction.ReturnEmptyString(model.getSectorColumns.SectorCode);
            BOSSDB.Entry(SectorTBL);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Delete Function
        public ActionResult DeleteSector(SectorModel model, int SectorID)
        {
            List<SubSector> subSector = (from e in BOSSDB.SubSectors where e.SectorID == SectorID select e).ToList();
            if (subSector != null)
            {
                foreach (var items in subSector)
                {
                    BOSSDB.SubSectors.Remove(items);
                    BOSSDB.SaveChanges();
                }
            }

            Sector Sectortbl = (from e in BOSSDB.Sectors where e.SectorID == SectorID select e).FirstOrDefault();
            BOSSDB.Sectors.Remove(Sectortbl);
            BOSSDB.SaveChanges();
            return RedirectToAction("FileSector");
        }
        //========================================================
                                                            //Sub-Sector Tab
        //Display Data Table
        public ActionResult GetSubSectorDTable()
        {
            SubSectorModel model = new SubSectorModel();

            List<SubSectorList> getSubSectorList = new List<SubSectorList>();

            var SQLQuery = "SELECT [SubSectorID],[Sector].SectorTitle,[SubSectorTitle],[SectorCode]+ ' - '+[SubSectorCode] as SubSectorCode FROM[BOSS].[dbo].[SubSector],[dbo].[Sector] where [SubSector].SectorID = [Sector].SectorID";
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
                            SubSectorCode = GlobalFunction.ReturnEmptyString(dr[3])
                        });
                    }
                }
                Connection.Close();
            }
            model.getSubSectorList = getSubSectorList.ToList();

            return PartialView("SubSectorTab/_TableSubSector", model.getSubSectorList);
        }
        //Get Add Partial View
        public ActionResult Get_AddSubSector()
        {
            SubSectorModel model = new SubSectorModel();
            return PartialView("SubSectorTab/_AddSubSector", model);
        }
        //Get field for Sector code
        public ActionResult GetSectorCodeField(int SectorID)
        {
            SubSectorModel model = new SubSectorModel();
            var SubSectorTbl = (from e in BOSSDB.Sectors where e.SectorID == SectorID select e).FirstOrDefault();
            model.SectorCode = SubSectorTbl.SectorCode;

            return PartialView("SubSectorTab/_DynamicSectorCode", model);
        }
        //Add Function
        public JsonResult AddNewSubSector(SubSectorModel model)
        {
            SubSector subSectorTbl = new SubSector();

            subSectorTbl.SectorID = GlobalFunction.ReturnEmptyInt(model.SectorID);
            subSectorTbl.SubSectorTitle = GlobalFunction.ReturnEmptyString(model.getSubSectorColumns.SubSectorTitle);
            subSectorTbl.SubSectorCode = GlobalFunction.ReturnEmptyString(model.getSubSectorColumns.SubSectorCode);
            BOSSDB.SubSectors.Add(subSectorTbl);

            BOSSDB.SaveChanges();
            return Json(subSectorTbl);
        }
        //Get Update Partial View
        public ActionResult Get_UpdateSubSector(SubSectorModel model, int SubSectorID)
        {
            SubSector tblsubSector = (from e in BOSSDB.SubSectors where e.SubSectorID == SubSectorID select e).FirstOrDefault();

            model.SectorID = Convert.ToInt32(tblsubSector.SectorID);
            model.getSubSectorColumns.SubSectorTitle = tblsubSector.SubSectorTitle;
            model.getSubSectorColumns.SubSectorCode = tblsubSector.SubSectorCode;
            model.SubSectorID = SubSectorID;
            return PartialView("SubSectorTab/_UpdateSubSector", model);
        }
        //Update Function
        public ActionResult UpdateSubSector(SubSectorModel model)
        {
            SubSector subSectorTBL = (from e in BOSSDB.SubSectors where e.SubSectorID == model.SubSectorID select e).FirstOrDefault();

            subSectorTBL.SectorID = GlobalFunction.ReturnEmptyInt(model.SectorID);
            subSectorTBL.SubSectorTitle = GlobalFunction.ReturnEmptyString(model.getSubSectorColumns.SubSectorTitle);
            subSectorTBL.SubSectorCode = GlobalFunction.ReturnEmptyString(model.getSubSectorColumns.SubSectorCode);
            BOSSDB.Entry(subSectorTBL);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        ////Delete Function
        public ActionResult DeleteSubSector(SubSectorModel model, int SubSectorID)
        {
            SubSector subSectortbl = (from e in BOSSDB.SubSectors where e.SubSectorID == SubSectorID select e).FirstOrDefault();
            BOSSDB.SubSectors.Remove(subSectortbl);
            BOSSDB.SaveChanges();
            return RedirectToAction("FileSector");
        }

    }
}