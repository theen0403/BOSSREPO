using BOSS.GlobalFunctions;
using BOSS.Models;
using BOSS.Models.FMmodels.FMAccountsModels;
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
    public class FileMaintenanceAccountsController : Controller
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        DatatypeValidation GlobalFunction = new DatatypeValidation();
        [Authorize]
        public ActionResult FileAccounts()
        {
            GeneralAccountModel model = new GeneralAccountModel();
            return View(model);
        }
        public ActionResult GetSubsidiaryTab()
        {
            GeneralAccountModel model = new GeneralAccountModel();
            return PartialView("SubsidiaryLedger/_IndexSubsidiaryLedger", model);
        }
        //--------------------------------------------------------------------------------------------------------------------
        //Revision Year
        //--------------------------------------------------------------------------------------------------------------------
        public ActionResult GetRevisionTab()
        {
            RevisionYearModel model = new RevisionYearModel();
            return PartialView("RevisionOfCOA/IndexRCOA", model);
        }
        public ActionResult GetRevisionDTable()
        {
            RevisionYearModel model = new RevisionYearModel();
            List<RevisionList> getRevisionYearList = new List<RevisionList>();
            var SQLQuery = "";
            SQLQuery = "SELECT [RevID], [RevYEar], [isUsed], [Remarks] FROM [BOSS].[dbo].[Tbl_FMCOA_RevisionYear]";

            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_FMAccounts]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        getRevisionYearList.Add(new RevisionList()
                        {
                            RevID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            RevYear = GlobalFunction.ReturnEmptyString(dr[1]),
                            isUsed = GlobalFunction.ReturnEmptyBool(dr[2]),
                            Remarks = GlobalFunction.ReturnEmptyString(dr[3])
                        });
                    }
                }
                Connection.Close();
            }
            model.getRevisionList = getRevisionYearList.ToList();
            return PartialView("RevisionOfCOA/_TableRCOA", getRevisionYearList);
        }
        public ActionResult GetRevisionForm(int ActionID, int PrimaryID)
        {
            RevisionYearModel model = new RevisionYearModel();
            if (ActionID == 2)
            {
                var rev = (from a in BOSSDB.Tbl_FMCOA_RevisionYear where a.RevID == PrimaryID select a).FirstOrDefault();
                model.RevisionList.RevYear = Convert.ToString(rev.RevYear);
                model.RevisionList.Remarks = rev.Remarks;
                model.RevisionList.isUsed = Convert.ToBoolean(rev.isUsed);
                model.RevisionList.RevID = rev.RevID;
            }
            model.ActionID = ActionID;
            return PartialView("RevisionOfCOA/_RCOAForm", model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveRevision(RevisionYearModel model)
        {
            var isExist = "";
            if (ModelState.IsValid)
            {
                var revYear = Convert.ToInt32(model.RevisionList.RevYear);
                Tbl_FMCOA_RevisionYear checkRev = (from a in BOSSDB.Tbl_FMCOA_RevisionYear where (a.RevYear == revYear) select a).FirstOrDefault();
                Tbl_FMCOA_RevisionYear revisionYear = (from a in BOSSDB.Tbl_FMCOA_RevisionYear where a.RevID == model.RevisionList.RevID select a).FirstOrDefault();
                List<Tbl_FMCOA_RevisionYear> revyearTitle = (from e in BOSSDB.Tbl_FMCOA_RevisionYear where e.RevYear == revYear select e).ToList();
                List<Tbl_FMCOA_RevisionYear> isused = (from e in BOSSDB.Tbl_FMCOA_RevisionYear where e.isUsed == true select e).ToList();

                if (model.ActionID == 1)
                {
                    if (isused.Count >= 1 && model.RevisionList.isUsed == true)
                    {
                        isExist = "activeIsUsed";
                    }
                    else
                    {
                        if (checkRev == null)
                        {
                            isExist = "false";
                        }
                        else if (checkRev != null)
                        {
                            isExist = "true";
                        }
                    }
                }
                else if (model.ActionID == 2)
                {
                    if (isused.Count >= 1 && model.RevisionList.isUsed == true)
                    {
                        isExist = "activeIsUsed";
                    }
                    else
                    {
                        if (checkRev != null)
                        {
                            if (revisionYear.RevYear != revYear && revyearTitle.Count >= 1)
                            {
                                isExist = "true";
                            }
                            else
                            {
                                isExist = "justUpdate";
                            }
                        }
                        else if (checkRev == null)
                        {
                            isExist = "justUpdate";
                        }
                    }
                }
                if (isExist == "justUpdate")
                {
                    revisionYear.RevYear = revYear;
                    revisionYear.isUsed = model.RevisionList.isUsed;
                    revisionYear.Remarks = model.RevisionList.Remarks;
                    BOSSDB.Entry(revisionYear);
                    BOSSDB.SaveChanges();
                }
                else if (isExist == "false")
                {
                    Tbl_FMCOA_RevisionYear revyearii = new Tbl_FMCOA_RevisionYear();
                    revyearii.RevYear = revYear;
                    revyearii.isUsed = model.RevisionList.isUsed;
                    revyearii.Remarks = model.RevisionList.Remarks;
                    BOSSDB.Tbl_FMCOA_RevisionYear.Add(revyearii);
                    BOSSDB.SaveChanges();
                }
            }
            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new { isExist = isExist }
            };
        }
        public ActionResult DeleteRevision(int PrimaryID)
        {
            Tbl_FMCOA_RevisionYear rev = (from a in BOSSDB.Tbl_FMCOA_RevisionYear where a.RevID == PrimaryID select a).FirstOrDefault();
            Tbl_FMCOA_AllotmentClass allot = (from a in BOSSDB.Tbl_FMCOA_AllotmentClass where a.RevID == PrimaryID select a).FirstOrDefault();
            var confirmDelete = "";
            if (rev != null)
            {
                if (allot != null)
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
            Tbl_FMCOA_RevisionYear revii = (from a in BOSSDB.Tbl_FMCOA_RevisionYear where a.RevID == PrimaryID select a).FirstOrDefault();
            BOSSDB.Tbl_FMCOA_RevisionYear.Remove(revii);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //--------------------------------------------------------------------------------------------------------------------
        //Allotment Class
        //--------------------------------------------------------------------------------------------------------------------
        public ActionResult AllotmentClassTab()
        {
            FMMainChartofAccountModel model = new FMMainChartofAccountModel();
            return PartialView("AllotmentClass/IndexAllotmentClass", model);
        }
        public ActionResult GetAllotClassForm(int ActionID, int AllotmentClassID)
        {
            AllotmentClassModel model = new AllotmentClassModel();
            if (ActionID == 2)
            {
                var allotClass = (from a in BOSSDB.Tbl_FMCOA_AllotmentClass where a.AllotmentClassID == AllotmentClassID select a).FirstOrDefault();
                model.AllotmentClassList.AllotmentClassTitle = allotClass.AllotmentClassTitle;
                model.AllotmentClassList.AllotmentClassID = allotClass.AllotmentClassID;
                model.AllotmentClassList.RevID = allotClass.Tbl_FMCOA_RevisionYear.RevID;
            }
            //model.RevYearList = new SelectList((from s in BOSSDB.Tbl_FMCOA_RevisionYear.Where(a => a.isUsed == true).ToList() select new { RevID = s.RevID, RevYear = s.RevYear }), "RevID", "RevYear");
            var RevYear = (from a in BOSSDB.Tbl_FMCOA_RevisionYear orderby a.isUsed descending select a).ToList();
            model.RevYearList = new SelectList(RevYear, "RevID", "RevYear");
            model.ActionID = ActionID;
            return PartialView("AllotmentClass/_AllotClassForm", model);
        }
        public ActionResult GetAllotmentClassDT(int RevID)
        {
            AllotmentClassModel model = new AllotmentClassModel();
            List<AllotmentClassList> getAllotmentClassList = new List<AllotmentClassList>();
            var SQLQuery = "SELECT [AllotmentClassID],[AllotmentClassTitle],[RevYear] FROM [BOSS].[dbo].[Tbl_FMCOA_AllotmentClass],[Tbl_FMCOA_RevisionYear] where [Tbl_FMCOA_AllotmentClass].RevID = [Tbl_FMCOA_RevisionYear].RevID and Tbl_FMCOA_RevisionYear.RevID = '"+ RevID + "'";

            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_FMAccounts]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        getAllotmentClassList.Add(new AllotmentClassList()
                        {
                            AllotmentClassID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            AllotmentClassTitle = GlobalFunction.ReturnEmptyString(dr[1]),
                            RevYear = GlobalFunction.ReturnEmptyString(dr[2])
                        });
                    }
                }
                Connection.Close();
            }
            model.getAllotmentClassList = getAllotmentClassList.ToList();
            return PartialView("AllotmentClass/_TableAllotment", getAllotmentClassList);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAllotmentClass(AllotmentClassModel model)
        {
            var isExist = "";

            if (ModelState.IsValid)
            {
                var AllotmentClassTitle = model.AllotmentClassList.AllotmentClassTitle;
                var revID = model.AllotmentClassList.RevID;
               // AllotmentClassTitle = GlobalFunction.AutoCaps_RemoveSpaces(AllotmentClassTitle);

                Tbl_FMCOA_AllotmentClass checkAllotmentClass = (from a in BOSSDB.Tbl_FMCOA_AllotmentClass where (a.AllotmentClassTitle == AllotmentClassTitle && a.RevID == revID) select a).FirstOrDefault();

                if (model.ActionID == 1)
                {
                    if (checkAllotmentClass == null)
                    {
                        Tbl_FMCOA_AllotmentClass allotClass = new Tbl_FMCOA_AllotmentClass();
                        allotClass.AllotmentClassTitle = AllotmentClassTitle;
                        allotClass.RevID = revID;
                        BOSSDB.Tbl_FMCOA_AllotmentClass.Add(allotClass);
                        BOSSDB.SaveChanges();
                        isExist = "false";
                    }
                    else if (checkAllotmentClass != null)
                    {
                        isExist = "true";
                    }
                }
                else if (model.ActionID == 2)
                {
                    Tbl_FMCOA_AllotmentClass allotClass = (from a in BOSSDB.Tbl_FMCOA_AllotmentClass where a.AllotmentClassID == model.AllotmentClassList.AllotmentClassID select a).FirstOrDefault();

                    if (checkAllotmentClass != null)
                    {
                        if (allotClass.AllotmentClassTitle == AllotmentClassTitle && allotClass.RevID == revID) //walang binago 
                        {
                            isExist = "justUpdate";
                        }
                        else
                        {
                            isExist = "true";
                        }
                    }
                    else if (checkAllotmentClass == null)
                    {
                        isExist = "justUpdate";
                    }
                    if(isExist == "justUpdate")
                    {
                        allotClass.AllotmentClassTitle = AllotmentClassTitle;
                        allotClass.RevID = revID;
                        BOSSDB.Entry(allotClass);
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
        public ActionResult DeleteAllotClass(int PrimaryID)
        {
            Tbl_FMCOA_AllotmentClass allotClass = (from a in BOSSDB.Tbl_FMCOA_AllotmentClass where a.AllotmentClassID == PrimaryID select a).FirstOrDefault();
            Tbl_FMCOA_AccountGroup accntGrp = (from a in BOSSDB.Tbl_FMCOA_AccountGroup where a.AllotmentClassID == PrimaryID select a).FirstOrDefault();
            var confirmDelete = "";
            if (allotClass != null)
            {
                if (accntGrp != null)
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
        public ActionResult ConfirmDeleteAllot(int PrimaryID)
        {
            Tbl_FMCOA_AllotmentClass allotClass = (from a in BOSSDB.Tbl_FMCOA_AllotmentClass where a.AllotmentClassID == PrimaryID select a).FirstOrDefault();
            BOSSDB.Tbl_FMCOA_AllotmentClass.Remove(allotClass);
            BOSSDB.SaveChanges();
            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //--------------------------------------------------------------------------------------------------------------------
        //Account Group Tab
        //--------------------------------------------------------------------------------------------------------------------
        public ActionResult AGTab()
        {
            AccountGroupModel model = new AccountGroupModel();
            return PartialView("AccountGroup/IndexAG", model);
        }
        public ActionResult GetAGDTable(int RevID)
        {
            AccountGroupModel model = new AccountGroupModel();
            List<AccountGrpList> getAccountGroupList = new List<AccountGrpList>();
            var SQLQuery = "select Tbl_FMCOA_AccountGroup.*, Tbl_FMCOA_RevisionYear.RevYear, Tbl_FMCOA_AllotmentClass.AllotmentClassTitle from Tbl_FMCOA_AccountGroup inner join Tbl_FMCOA_RevisionYear on Tbl_FMCOA_AccountGroup.RevID = Tbl_FMCOA_RevisionYear.RevID left join Tbl_FMCOA_AllotmentClass on Tbl_FMCOA_AccountGroup.AllotmentClassID = Tbl_FMCOA_AllotmentClass.AllotmentClassID where Tbl_FMCOA_RevisionYear.RevID = '" + RevID + "'";

            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_FMAccounts]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        getAccountGroupList.Add(new AccountGrpList()
                        {
                            AGID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            RevYear = GlobalFunction.ReturnEmptyString(dr[5]),
                            AllotmentClassTitle = GlobalFunction.ReturnEmptyString(dr[6]),
                            AGTitle = GlobalFunction.ReturnEmptyString(dr[1]),
                            AGCode = GlobalFunction.ReturnEmptyString(dr[2])
                        });
                    }
                }
                Connection.Close();
            }
            model.getAccountGroupList = getAccountGroupList.ToList();
            return PartialView("AccountGroup/_TableAG", getAccountGroupList);
        }
        public ActionResult GetAGForm(int ActionID, int AGID)
        {
            AccountGroupModel model = new AccountGroupModel();

            if (ActionID == 2)
            {
                var accntGrp = (from a in BOSSDB.Tbl_FMCOA_AccountGroup where a.AGID == AGID select a).FirstOrDefault();
                model.AccountGrpList.AGTitle = accntGrp.AGTitle;
                model.AccountGrpList.AGCode = accntGrp.AGCode;
                model.AccountGrpList.RevID = accntGrp.Tbl_FMCOA_RevisionYear.RevID;
                model.AccountGrpList.AGID = accntGrp.AGID;

                var allotClass = (from a in BOSSDB.Tbl_FMCOA_AllotmentClass where a.RevID == model.AccountGrpList.RevID orderby a.AllotmentClassTitle select a).ToList();

                foreach (var item in allotClass)
                {
                    model.AllotClassList.Add(new SelectListItem() { Text = item.AllotmentClassTitle, Value = item.AllotmentClassID.ToString() });
                }
                model.AllotClassList.Add(new SelectListItem() { Text = "N/A", Value = "0" });

                if (accntGrp.AllotmentClassID == null)
                {
                    model.AccountGrpList.AllotmentClassID = 0;
                }
                else
                {
                    model.AccountGrpList.AllotmentClassID = accntGrp.Tbl_FMCOA_AllotmentClass.AllotmentClassID;
                }
            }
            else
            {
                var allotClassRevYear = (from a in BOSSDB.Tbl_FMCOA_RevisionYear orderby a.isUsed descending select a.RevID).FirstOrDefault();
                model.AccountGrpList.RevID = allotClassRevYear;

                var allotClass = (from a in BOSSDB.Tbl_FMCOA_AllotmentClass where a.RevID == model.AccountGrpList.RevID orderby a.AllotmentClassTitle select a).ToList();

                foreach (var item in allotClass)
                {
                    model.AllotClassList.Add(new SelectListItem() { Text = item.AllotmentClassTitle, Value = item.AllotmentClassID.ToString() });
                }
                model.AllotClassList.Add(new SelectListItem() { Text = "N/A", Value = "000" });
            }
            var RevYear = (from a in BOSSDB.Tbl_FMCOA_RevisionYear orderby a.isUsed descending select a).ToList();
            model.RevYearList = new SelectList(RevYear, "RevID", "RevYear");
            model.ActionID = ActionID;

            return PartialView("AccountGroup/_AGForm", model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAG(AccountGroupModel model)
        {
            var isExist = "";
            if (ModelState.IsValid)
            {
                var AGTitle = model.AccountGrpList.AGTitle;
                var accntGrpCode = model.AccountGrpList.AGCode;
                var accntGrpRevID = model.AccountGrpList.RevID;

                AGTitle = GlobalFunction.AutoCaps_RemoveSpaces(AGTitle);
                accntGrpCode = GlobalFunction.RemoveSpaces(accntGrpCode);
                int? accntGrpAllotID = model.AccountGrpList.AllotmentClassID;
                if (accntGrpAllotID == 000 || accntGrpAllotID == 0)
                {
                    accntGrpAllotID = null;
                }
                else
                {
                    accntGrpAllotID = model.AccountGrpList.AllotmentClassID;
                }
                Tbl_FMCOA_AccountGroup checkAccntGrp = (from a in BOSSDB.Tbl_FMCOA_AccountGroup where (a.RevID == accntGrpRevID && a.AllotmentClassID == accntGrpAllotID) && (a.AGTitle == AGTitle && a.AGCode == accntGrpCode) || (a.RevID == accntGrpRevID && a.AGCode == accntGrpCode)  select a).FirstOrDefault();
                if (model.ActionID == 1)
                {
                    if (checkAccntGrp == null)
                    {
                        Tbl_FMCOA_AccountGroup accntGrp = new Tbl_FMCOA_AccountGroup();
                        accntGrp.AGTitle = AGTitle;
                        accntGrp.AGCode = accntGrpCode;
                        if (accntGrpAllotID == 000 || accntGrpAllotID == 0)
                        {
                            accntGrp.AllotmentClassID = null;
                        }
                        else
                        {
                            accntGrp.AllotmentClassID = accntGrpAllotID;
                        }
                        accntGrp.RevID = model.AccountGrpList.RevID;
                        BOSSDB.Tbl_FMCOA_AccountGroup.Add(accntGrp);
                        BOSSDB.SaveChanges();
                        isExist = "false";
                    }
                    else if (checkAccntGrp != null)
                    {
                        isExist = "true";
                    }
                }
                else if (model.ActionID == 2)
                {
                    List<Tbl_FMCOA_AccountGroup> selectRevAllot = (from a in BOSSDB.Tbl_FMCOA_AccountGroup where (a.RevID == accntGrpRevID && a.AllotmentClassID == accntGrpAllotID) select a).ToList();
                    Tbl_FMCOA_AccountGroup accntGrp = (from a in BOSSDB.Tbl_FMCOA_AccountGroup where a.AGTitle == model.AccountGrpList.AGTitle select a).FirstOrDefault();
                    var save = false;
                    if (selectRevAllot.Count > 0)
                    {
                        foreach (var item in selectRevAllot)
                        {
                            if (item.AGTitle == AGTitle && item.AGCode == accntGrpCode && item.AGID == accntGrp.AGID)  // walang binago
                            {
                                save = true;
                            }
                            else if (item.AGTitle != AGTitle && item.AGCode != accntGrpCode || item.AGID == accntGrp.AGID) // may binago pero walang kaparehas
                            {
                                save = true;
                            }
                            else if (item.AGTitle == AGTitle || item.AGCode == accntGrpCode) // may binago pero may kaparehas
                            {
                                save = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        save = true;
                    }

                    switch (save)
                    {
                        case true:
                            accntGrp.AGTitle = AGTitle;
                            accntGrp.AGCode = accntGrpCode;
                            if (accntGrpAllotID == 0)
                            {
                                accntGrp.AllotmentClassID = null;
                            }
                            else
                            {
                                accntGrp.AllotmentClassID = accntGrpAllotID;
                            }
                            accntGrp.RevID = model.AccountGrpList.RevID;
                            BOSSDB.Entry(accntGrp);
                            BOSSDB.SaveChanges();
                            isExist = "justUpdate";
                            break;
                        default:
                            isExist = "true";
                            break;
                    }
                }
            }
            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new { isExist = isExist }
            };
        }
        public ActionResult DeleteAG(int PrimaryID)
        {
            Tbl_FMCOA_AccountGroup accntGrp = (from a in BOSSDB.Tbl_FMCOA_AccountGroup where a.AGID == PrimaryID select a).FirstOrDefault();
            Tbl_FMCOA_MajorAccountGroup majaccntGrp = (from a in BOSSDB.Tbl_FMCOA_MajorAccountGroup where a.AGID == PrimaryID select a).FirstOrDefault();
            var confirmDelete = "";
            if (accntGrp != null)
            {
                if (majaccntGrp != null)
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
        public ActionResult ConfirmDeleteAG(int PrimaryID)
        {
            Tbl_FMCOA_AccountGroup accntGrp = (from a in BOSSDB.Tbl_FMCOA_AccountGroup where a.AGID == PrimaryID select a).FirstOrDefault();

            BOSSDB.Tbl_FMCOA_AccountGroup.Remove(accntGrp);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //--------------------------------------------------------------------------------------------------------------------
        //Major Account Group Tab
        //--------------------------------------------------------------------------------------------------------------------
        public ActionResult MAGTab()
        {
            RevisionYearModel model = new RevisionYearModel();
            return PartialView("MajorAccountGroup/IndexMAG", model);
        }
        public ActionResult GetMAGDTable(int RevID)
        {
            MajorAccountGroupModel model = new MajorAccountGroupModel();
            List<MajorAccountGroupList> getMajorAccountGroupList = new List<MajorAccountGroupList>();
            var SQLQuery = "select Tbl_FMCOA_MajorAccountGroup.*, Tbl_FMCOA_RevisionYear.RevYear, Tbl_FMCOA_AllotmentClass.AllotmentClassTitle, Tbl_FMCOA_AccountGroup.*, CONCAT(Tbl_FMCOA_AccountGroup.AGCode, '-', Tbl_FMCOA_MajorAccountGroup.MAGCode) as concatMAGCode from Tbl_FMCOA_MajorAccountGroup inner join Tbl_FMCOA_AccountGroup on Tbl_FMCOA_MajorAccountGroup.MAGID = Tbl_FMCOA_AccountGroup.AGID left join Tbl_FMCOA_AllotmentClass on Tbl_FMCOA_AccountGroup.AllotmentClassID = Tbl_FMCOA_AllotmentClass.AllotmentClassID inner join Tbl_FMCOA_RevisionYear on Tbl_FMCOA_AccountGroup.RevID = Tbl_FMCOA_RevisionYear.RevID where Tbl_FMCOA_RevisionYear.RevID = '" + RevID + "'";

            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_FMAccounts]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        getMajorAccountGroupList.Add(new MajorAccountGroupList()
                        {
                            MAGID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            RevYear = GlobalFunction.ReturnEmptyString(dr[4]),
                            AllotmentClassTitle = GlobalFunction.ReturnEmptyString(dr[5]),
                            AGTitle = GlobalFunction.ReturnEmptyString(dr[7]),
                            MAGTitle = GlobalFunction.ReturnEmptyString(dr[1]),
                            concatMAGCode = GlobalFunction.ReturnEmptyString(dr[11])
                        });
                    }
                }
                Connection.Close();
            }
            model.getMajorAccountGroupList = getMajorAccountGroupList.ToList();
            return PartialView("MajorAccountGroup/_TableMAG", getMajorAccountGroupList);
        }
        public ActionResult GetMAGForm(int ActionID, int MAGID)
        {
            MajorAccountGroupModel model = new MajorAccountGroupModel();

            if (ActionID == 2)
            {
                var majAccntGrp = (from a in BOSSDB.Tbl_FMCOA_MajorAccountGroup where a.MAGID == MAGID select a).FirstOrDefault();

                var GetAGID = (from a in BOSSDB.Tbl_FMCOA_MajorAccountGroup where a.MAGID == MAGID select a.AGID).FirstOrDefault();
                var GetAccntGrpRecord = (from a in BOSSDB.Tbl_FMCOA_AccountGroup where a.AGID == GetAGID select a).FirstOrDefault();
                model.MajorAccountGroupList.RevID = GlobalFunction.ReturnEmptyInt(GetAccntGrpRecord.RevID);
                model.MajorAccountGroupList.AllotmentClassID = GlobalFunction.ReturnEmptyInt(GetAccntGrpRecord.AllotmentClassID);
                model.MajorAccountGroupList.MAGTitle = majAccntGrp.MAGTitle;
                model.MajorAccountGroupList.MAGCode = majAccntGrp.MAGCode;
                model.MajorAccountGroupList.AGID = majAccntGrp.Tbl_FMCOA_AccountGroup.AGID;
                model.MajorAccountGroupList.MAGID = majAccntGrp.MAGID;

                var allotClass = (from a in BOSSDB.Tbl_FMCOA_AllotmentClass where a.RevID == model.MajorAccountGroupList.RevID orderby a.AllotmentClassID select a).ToList();
                var accntGrpCheckAllot = (from a in BOSSDB.Tbl_FMCOA_AccountGroup where a.AGID == model.MajorAccountGroupList.AGID select a).FirstOrDefault();

                foreach (var item in allotClass)
                {
                    model.AllotClassList.Add(new SelectListItem() { Text = item.AllotmentClassTitle, Value = item.AllotmentClassID.ToString() });
                }
                if (accntGrpCheckAllot.AllotmentClassID == null)
                {
                    model.MajorAccountGroupList.AllotmentClassID = 0;
                    var accntClass = (from a in BOSSDB.Tbl_FMCOA_AccountGroup where a.RevID == model.MajorAccountGroupList.RevID && a.AllotmentClassID == null orderby a.AGTitle select a).ToList();
                    model.AccntGrpList = new SelectList(accntClass, "AGID", "AGTitle");
                }
                else
                {
                    model.MajorAccountGroupList.AllotmentClassID = accntGrpCheckAllot.Tbl_FMCOA_AllotmentClass.AllotmentClassID;
                    var accntClass = (from a in BOSSDB.Tbl_FMCOA_AccountGroup where a.RevID == model.MajorAccountGroupList.RevID && a.AllotmentClassID == model.MajorAccountGroupList.AllotmentClassID orderby a.AGTitle select a).ToList();
                    model.AccntGrpList = new SelectList(accntClass, "AGID", "AGTitle");
                }
                model.AccntGrpList = (from li in model.AccntGrpList orderby li.Text select li).ToList();
            }
            else
            {
                var majorRevYear = (from a in BOSSDB.Tbl_FMCOA_RevisionYear orderby a.isUsed descending select a.RevID).FirstOrDefault();
                model.MajorAccountGroupList.RevID = majorRevYear;

                if (majorRevYear != 0)
                {
                    var allotClass = (from a in BOSSDB.Tbl_FMCOA_AllotmentClass where a.RevID == model.MajorAccountGroupList.RevID orderby a.AllotmentClassTitle select a).ToList();
                    if (allotClass.Count > 0)
                    {
                        foreach (var item in allotClass)
                        {
                            model.AllotClassList.Add(new SelectListItem() { Text = item.AllotmentClassTitle, Value = item.AllotmentClassID.ToString() });
                        }
                        var firstAllotClass = (from a in allotClass orderby a.AllotmentClassTitle select a).FirstOrDefault();

                        int? allotID = 0;
                        if (firstAllotClass.AllotmentClassID == 0)
                        {
                            allotID = null;

                        }
                        else
                        {
                            allotID = firstAllotClass.AllotmentClassID;
                        }
                        var accntGrpCheckAllot = (from a in BOSSDB.Tbl_FMCOA_AccountGroup where a.AllotmentClassID == allotID && a.RevID == model.MajorAccountGroupList.RevID select a).ToList();

                        if (accntGrpCheckAllot.Count > 0)
                        {
                            var firstAccntGrp = (from a in accntGrpCheckAllot orderby a.AGTitle select a).FirstOrDefault();

                            model.AccntGrpList = new SelectList(accntGrpCheckAllot, "AGID", "AGTitle");
                            model.AccntGrpList = (from li in model.AccntGrpList orderby li.Text select li).ToList();
                        }
                    }
                }
            }
            var RevYear = (from a in BOSSDB.Tbl_FMCOA_RevisionYear orderby a.isUsed descending select a).ToList();
            model.RevYearList = new SelectList(RevYear, "RevID", "RevYear");
            model.AllotClassList.Add(new SelectListItem() { Text = "N/A", Value = "000" });
            model.ActionID = ActionID;
            return PartialView("MajorAccountGroup/_MAGForm", model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveMAG(MajorAccountGroupModel model)
        {
            var isExist = "";

            if (ModelState.IsValid)
            {
                var majorTitle = model.MajorAccountGroupList.MAGTitle;
                var majorCode = model.MajorAccountGroupList.MAGCode;
                var accntID = model.MajorAccountGroupList.AGID;
                majorTitle = GlobalFunction.AutoCaps_RemoveSpaces(majorTitle);

                Tbl_FMCOA_MajorAccountGroup checkMajAccntClass = (from a in BOSSDB.Tbl_FMCOA_MajorAccountGroup where (a.AGID == accntID) && (a.MAGTitle == majorTitle && a.MAGCode == majorCode) || (a.AGID == accntID && a.MAGCode == majorCode) select a).FirstOrDefault();

                if (model.ActionID == 1)
                {
                    if (checkMajAccntClass == null)
                    {
                        Tbl_FMCOA_MajorAccountGroup majAccntGrp = new Tbl_FMCOA_MajorAccountGroup();
                        majAccntGrp.MAGTitle = majorTitle;
                        majAccntGrp.MAGCode = majorCode;
                        majAccntGrp.AGID = accntID;
                        BOSSDB.Tbl_FMCOA_MajorAccountGroup.Add(majAccntGrp);
                        BOSSDB.SaveChanges();
                        isExist = "false";
                    }
                    else if (checkMajAccntClass != null)
                    {
                        isExist = "true";
                    }
                }
                else if (model.ActionID == 2)
                {
                    Tbl_FMCOA_MajorAccountGroup majAccntGrp = (from a in BOSSDB.Tbl_FMCOA_MajorAccountGroup where a.MAGID == model.MajorAccountGroupList.MAGID select a).FirstOrDefault();
                    List<Tbl_FMCOA_MajorAccountGroup> selectAccntGrp = (from a in BOSSDB.Tbl_FMCOA_MajorAccountGroup where (a.AGID == accntID) select a).ToList();
                    var save = false;
                    if (selectAccntGrp.Count > 0)
                    {
                        foreach (var item in selectAccntGrp)
                        {
                            if (item.MAGTitle == majorTitle && item.MAGCode == majorCode && item.MAGID == majAccntGrp.MAGID)  // walang binago
                            {
                                save = true;
                            }
                            else if (item.MAGTitle != majorTitle && item.MAGCode != majorCode || item.MAGID == majAccntGrp.MAGID) // may binago pero walang kaparehas
                            {
                                save = true;
                            }
                            else if (item.MAGTitle == majorTitle || item.MAGCode == majorCode) // may binago pero may kaparehas
                            {
                                save = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        save = true;
                    }
                    switch (save)
                    {
                        case true:
                            majAccntGrp.MAGTitle = majorTitle;
                            majAccntGrp.MAGCode = majorCode;
                            majAccntGrp.AGID = accntID;
                            BOSSDB.Entry(majAccntGrp);
                            BOSSDB.SaveChanges();
                            isExist = "justUpdate";
                            break;
                        default:
                            isExist = "true";
                            break;
                    }
                }
            }
            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new { isExist = isExist }
            };
        }
        public ActionResult DeleteMAG(int PrimaryID)
        {
            Tbl_FMCOA_MajorAccountGroup majAccntGrp = (from a in BOSSDB.Tbl_FMCOA_MajorAccountGroup where a.MAGID == PrimaryID select a).FirstOrDefault();
            Tbl_FMCOA_SubMajorAccountGroup subMajAccnt = (from a in BOSSDB.Tbl_FMCOA_SubMajorAccountGroup where a.MAGID == PrimaryID select a).FirstOrDefault();
            var confirmDelete = "";
            if (majAccntGrp != null)
            {
                if (subMajAccnt != null)
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
        public ActionResult ConfirmDeleteMAG(int PrimaryID)
        {
            Tbl_FMCOA_MajorAccountGroup majAccntGrp = (from a in BOSSDB.Tbl_FMCOA_MajorAccountGroup where a.MAGID == PrimaryID select a).FirstOrDefault();

            BOSSDB.Tbl_FMCOA_MajorAccountGroup.Remove(majAccntGrp);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //--------------------------------------------------------------------------------------------------------------------
        //Sub Major Account Group Tab
        //--------------------------------------------------------------------------------------------------------------------
        public ActionResult SMAGTab()
        {
            RevisionYearModel model = new RevisionYearModel();
            return PartialView("SubMajorAccountGroup/IndexSMAG", model);
        }
        public ActionResult GetSMAGDTable(int RevID)
        {
            SubMajorAccountGroupModel model = new SubMajorAccountGroupModel();
            List<SubMajorAccntGrpList> getSubMajorAccntGrpList = new List<SubMajorAccntGrpList>();
            var SQLQuery = "";

            SQLQuery = "select Tbl_FMCOA_SubMajorAccountGroup.*, Tbl_FMCOA_MajorAccountGroup.*, Tbl_FMCOA_RevisionYear.RevYear, Tbl_FMCOA_AllotmentClass.AllotmentClassTitle, Tbl_FMCOA_AccountGroup.AGTitle, CONCAT(Tbl_FMCOA_AccountGroup.AGCode, '-', Tbl_FMCOA_MajorAccountGroup.MAGCode, '-', Tbl_FMCOA_SubMajorAccountGroup.SMAGCode) as SMAGAccountCode from Tbl_FMCOA_SubMajorAccountGroup inner join Tbl_FMCOA_MajorAccountGroup on Tbl_FMCOA_SubMajorAccountGroup.MAGID = Tbl_FMCOA_MajorAccountGroup.MAGID inner join Tbl_FMCOA_AccountGroup on Tbl_FMCOA_MajorAccountGroup.AGID = Tbl_FMCOA_AccountGroup.AGID inner join Tbl_FMCOA_RevisionYear on Tbl_FMCOA_AccountGroup.RevID = Tbl_FMCOA_RevisionYear.RevID left join Tbl_FMCOA_AllotmentClass on Tbl_FMCOA_AccountGroup.AllotmentClassID = Tbl_FMCOA_AllotmentClass.AllotmentClassID where Tbl_FMCOA_RevisionYear.RevID = '"+RevID+"' ";

            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_FMAccounts]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        getSubMajorAccntGrpList.Add(new SubMajorAccntGrpList()
                        {
                            SMAGID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            RevYear = GlobalFunction.ReturnEmptyString(dr[8]),
                            AllotmentClassTitle =GlobalFunction.ReturnEmptyString(dr[9]),
                            AGTitle = GlobalFunction.ReturnEmptyString(dr[10]),
                            MAGTitle = GlobalFunction.ReturnEmptyString(dr[5]),
                            SMAGTitle = GlobalFunction.ReturnEmptyString(dr[1]),
                            SMAGAccountCode = GlobalFunction.ReturnEmptyString(dr[11])
                        });
                    }
                }
                Connection.Close();
            }
            model.getSubMajorAccntGrpList = getSubMajorAccntGrpList.ToList();
            return PartialView("SubMajorAccountGroup/_TableSMAG", getSubMajorAccntGrpList);
        }
        public ActionResult GetSMAGForm(int ActionID, int SMAGID)
        {
            SubMajorAccountGroupModel model = new SubMajorAccountGroupModel();

            if (ActionID == 2)
            {
                var subMajAccntGrp = (from a in BOSSDB.Tbl_FMCOA_SubMajorAccountGroup where a.SMAGID == SMAGID select a).FirstOrDefault();

                model.SubMajorAccntGrpList.SMAGID = subMajAccntGrp.SMAGID;
                model.SubMajorAccntGrpList.SMAGTitle = subMajAccntGrp.SMAGTitle;
                model.SubMajorAccntGrpList.SMAGCode = subMajAccntGrp.SMAGCode;
                model.SubMajorAccntGrpList.RevID = GlobalFunction.ReturnEmptyInt(subMajAccntGrp.Tbl_FMCOA_MajorAccountGroup.Tbl_FMCOA_AccountGroup.RevID);
                model.SubMajorAccntGrpList.AllotmentClassID = GlobalFunction.ReturnEmptyInt(subMajAccntGrp.Tbl_FMCOA_MajorAccountGroup.Tbl_FMCOA_AccountGroup.AllotmentClassID);
                model.SubMajorAccntGrpList.AGID = GlobalFunction.ReturnEmptyInt(subMajAccntGrp.Tbl_FMCOA_MajorAccountGroup.Tbl_FMCOA_AccountGroup.AGID);
                model.SubMajorAccntGrpList.MAGID = GlobalFunction.ReturnEmptyInt(subMajAccntGrp.Tbl_FMCOA_MajorAccountGroup.MAGID);

                var allotClass = (from a in BOSSDB.Tbl_FMCOA_AllotmentClass where a.RevID == model.SubMajorAccntGrpList.RevID orderby a.AllotmentClassTitle select a).ToList();

                var accntGrpCheckAllot = (from a in BOSSDB.Tbl_FMCOA_AccountGroup where a.AGID == model.SubMajorAccntGrpList.AGID select a).FirstOrDefault();

                foreach (var item in allotClass)
                {
                    model.AllotClassList.Add(new SelectListItem() { Text = item.AllotmentClassTitle, Value = item.AllotmentClassID.ToString() });
                }

                if (accntGrpCheckAllot.AllotmentClassID == null)
                {
                    model.SubMajorAccntGrpList.AllotmentClassID = 0;
                    var accntClass = (from a in BOSSDB.Tbl_FMCOA_AccountGroup where a.RevID == model.SubMajorAccntGrpList.RevID && a.AllotmentClassID == null orderby a.AGTitle select a).ToList();
                    model.AccntGrpList = new SelectList(accntClass, "AGID", "AGTitle");
                }
                else
                {
                    model.SubMajorAccntGrpList.AllotmentClassID = accntGrpCheckAllot.Tbl_FMCOA_AllotmentClass.AllotmentClassID;
                    var accntClass = (from a in BOSSDB.Tbl_FMCOA_AccountGroup where a.RevID == model.SubMajorAccntGrpList.RevID && a.AllotmentClassID == model.SubMajorAccntGrpList.AllotmentClassID orderby a.AGTitle select a).ToList();
                    model.AccntGrpList = new SelectList(accntClass, "AGID", "AGTitle");
                }

                model.AccntGrpList = (from li in model.AccntGrpList orderby li.Text select li).ToList();

                var majAccntGrp = (from a in BOSSDB.Tbl_FMCOA_MajorAccountGroup where a.AGID == model.SubMajorAccntGrpList.AGID select a).ToList();
                model.MajAccntGrpList = new SelectList(majAccntGrp, "MAGID", "MAGTitle");
                model.MajAccntGrpList = (from li in model.MajAccntGrpList orderby li.Text select li).ToList();
            }
            else
            {
                var allotClassRevYear = (from a in BOSSDB.Tbl_FMCOA_RevisionYear orderby a.isUsed descending select a.RevID).FirstOrDefault();
                model.SubMajorAccntGrpList.RevID = allotClassRevYear;
                if (allotClassRevYear != 0)
                {
                    var allotClass = (from a in BOSSDB.Tbl_FMCOA_AllotmentClass where a.RevID == model.SubMajorAccntGrpList.RevID orderby a.AllotmentClassTitle select a).ToList();
                    if (allotClass.Count > 0)
                    {
                        foreach (var item in allotClass)
                        {
                            model.AllotClassList.Add(new SelectListItem() { Text = item.AllotmentClassTitle, Value = item.AllotmentClassID.ToString() });
                        }

                        var firstAllotClass = (from a in allotClass orderby a.AllotmentClassTitle select a).FirstOrDefault();

                        int? allotID = 0;
                        if (firstAllotClass.AllotmentClassID == 0)
                        {
                            allotID = null;

                        }
                        else
                        {
                            allotID = firstAllotClass.AllotmentClassID;
                        }

                        var accntGrpCheckAllot = (from a in BOSSDB.Tbl_FMCOA_AccountGroup where a.AllotmentClassID == allotID && a.RevID == model.SubMajorAccntGrpList.RevID select a).ToList();

                        if (accntGrpCheckAllot.Count > 0)
                        {
                            var firstAccntGrp = (from a in accntGrpCheckAllot orderby a.AGTitle select a).FirstOrDefault();
                            var majAccntGrp = (from a in BOSSDB.Tbl_FMCOA_MajorAccountGroup where a.AGID == firstAccntGrp.AGID select a).ToList();

                            model.AccntGrpList = new SelectList(accntGrpCheckAllot, "AGID", "AGTitle");
                            model.AccntGrpList = (from li in model.AccntGrpList orderby li.Text select li).ToList();

                            if (majAccntGrp.Count > 0)
                            {
                                model.MajAccntGrpList = new SelectList(majAccntGrp, "MAGID", "MAGTitle");
                                model.MajAccntGrpList = (from li in model.MajAccntGrpList orderby li.Text select li).ToList();
                            }
                        }
                    }
                }
            }
            var RevYear = (from a in BOSSDB.Tbl_FMCOA_RevisionYear orderby a.isUsed descending select a).ToList();
            model.RevYearList = new SelectList(RevYear, "RevID", "RevYear");
            model.AllotClassList.Add(new SelectListItem() { Text = "N/A", Value = "000" });
            model.ActionID = ActionID;
            return PartialView("SubMajorAccountGroup/_SMAGForm", model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSMAG(SubMajorAccountGroupModel model)
        {
            var isExist = "";

            if (ModelState.IsValid)
            {
                var SMAGTitle = model.SubMajorAccntGrpList.SMAGTitle;
                var SMAGCode = model.SubMajorAccntGrpList.SMAGCode;
                var MAGID = model.SubMajorAccntGrpList.MAGID;
                var SMAGID = model.SubMajorAccntGrpList.SMAGID;
                SMAGTitle = GlobalFunction.AutoCaps_RemoveSpaces(SMAGTitle);

                Tbl_FMCOA_SubMajorAccountGroup checkSubMajAccntClass = (from a in BOSSDB.Tbl_FMCOA_SubMajorAccountGroup where (a.MAGID == MAGID) && (a.SMAGTitle == SMAGTitle || a.SMAGCode == SMAGCode) select a).FirstOrDefault();

                if (model.ActionID == 1)
                {
                    if (checkSubMajAccntClass == null)
                    {
                        Tbl_FMCOA_SubMajorAccountGroup subMajAccntGrp = new Tbl_FMCOA_SubMajorAccountGroup();
                        subMajAccntGrp.SMAGTitle = SMAGTitle;
                        subMajAccntGrp.SMAGCode = SMAGCode;
                        subMajAccntGrp.MAGID = MAGID;
                        BOSSDB.Tbl_FMCOA_SubMajorAccountGroup.Add(subMajAccntGrp);
                        BOSSDB.SaveChanges();
                        isExist = "false";
                    }
                    else if (checkSubMajAccntClass != null)
                    {
                        isExist = "true";
                    }
                }
                else if (model.ActionID == 2)
                {
                    Tbl_FMCOA_SubMajorAccountGroup subMajAccntGrp = (from a in BOSSDB.Tbl_FMCOA_SubMajorAccountGroup where a.SMAGID == SMAGID select a).FirstOrDefault();
                    List<Tbl_FMCOA_SubMajorAccountGroup> selectSubMajAccntGrp = (from a in BOSSDB.Tbl_FMCOA_SubMajorAccountGroup where (a.MAGID == MAGID) select a).ToList();
                    var save = false;
                    if (selectSubMajAccntGrp.Count > 0)
                    {
                        foreach (var item in selectSubMajAccntGrp)
                        {
                            if (item.SMAGTitle == SMAGTitle && item.SMAGCode == SMAGCode && item.SMAGID == SMAGID)  // walang binago
                            {
                                save = true;
                            }
                            else if (item.SMAGTitle != SMAGTitle && item.SMAGCode != SMAGCode || item.SMAGID == SMAGID) // may binago pero walang kaparehas
                            {
                                save = true;
                            }
                            else if (item.SMAGTitle == SMAGTitle || item.SMAGCode == SMAGCode) // may binago pero may kaparehas
                            {
                                save = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        save = true;
                    }
                    switch (save)
                    {
                        case true:
                            subMajAccntGrp.SMAGTitle = SMAGTitle;
                            subMajAccntGrp.SMAGCode = SMAGCode;
                            subMajAccntGrp.MAGID = MAGID;
                            BOSSDB.Entry(subMajAccntGrp);
                            BOSSDB.SaveChanges();
                            isExist = "justUpdate";
                            break;
                        default:
                            isExist = "true";
                            break;
                    }
                }

            }
            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new { isExist = isExist }
            };
        }
        public ActionResult DeleteSMAG(int PrimaryID)
        {
            Tbl_FMCOA_SubMajorAccountGroup subMajAccnt = (from a in BOSSDB.Tbl_FMCOA_SubMajorAccountGroup where a.SMAGID == PrimaryID select a).FirstOrDefault();
            Tbl_FMCOA_GeneralAccount genAccnt = (from a in BOSSDB.Tbl_FMCOA_GeneralAccount where a.SMAGID == PrimaryID select a).FirstOrDefault();
            var confirmDelete = "";
            if (subMajAccnt != null)
            {
                if (genAccnt != null)
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
        public ActionResult ConfirmDeleteSMAG(int PrimaryID)
        {
            Tbl_FMCOA_SubMajorAccountGroup subMajAccnt = (from a in BOSSDB.Tbl_FMCOA_SubMajorAccountGroup where a.SMAGID == PrimaryID select a).FirstOrDefault();

            BOSSDB.Tbl_FMCOA_SubMajorAccountGroup.Remove(subMajAccnt);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //--------------------------------------------------------------------------------------------------------------------
        //General Account Tab
        //--------------------------------------------------------------------------------------------------------------------
        public ActionResult GATab()
        {
            GeneralAccountModel model = new GeneralAccountModel();
            return PartialView("GeneralAccount/IndexGA", model);
        }
        public ActionResult GetGADTable(int RevID)
        {
            GeneralAccountModel model = new GeneralAccountModel();
            List<GeneralAccountList> getGeneralAccountList = new List<GeneralAccountList>();
            var SQLQuery = "select Tbl_FMCOA_GeneralAccount.*, Tbl_FMCOA_SubMajorAccountGroup.*, Tbl_FMCOA_RevisionYear.RevYear, Tbl_FMCOA_MajorAccountGroup.MAGTitle, Tbl_FMCOA_AllotmentClass.AllotmentClassTitle, Tbl_FMCOA_AccountGroup.AGTitle, CONCAT(Tbl_FMCOA_AccountGroup.AGCode, '-', Tbl_FMCOA_MajorAccountGroup.MAGCode, '-', Tbl_FMCOA_SubMajorAccountGroup.SMAGCode, '-', Tbl_FMCOA_GeneralAccount.GACode) as GenAccountCode from Tbl_FMCOA_GeneralAccount inner join Tbl_FMCOA_SubMajorAccountGroup on Tbl_FMCOA_GeneralAccount.SMAGID = Tbl_FMCOA_SubMajorAccountGroup.SMAGID inner join Tbl_FMCOA_MajorAccountGroup on Tbl_FMCOA_SubMajorAccountGroup.MAGID = Tbl_FMCOA_MajorAccountGroup.MAGID inner join Tbl_FMCOA_AccountGroup on Tbl_FMCOA_MajorAccountGroup.AGID = Tbl_FMCOA_AccountGroup.AGID inner join Tbl_FMCOA_RevisionYear on Tbl_FMCOA_AccountGroup.RevID = Tbl_FMCOA_RevisionYear.RevID left join Tbl_FMCOA_AllotmentClass on Tbl_FMCOA_AccountGroup.AllotmentClassID = Tbl_FMCOA_AllotmentClass.AllotmentClassID where Tbl_FMCOA_RevisionYear.RevID ='"+RevID+"'";

            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_FMAccounts]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        getGeneralAccountList.Add(new GeneralAccountList()
                        {
                            GAID = GlobalFunction.ReturnEmptyInt(dr[0]), //okie
                            GATitle = GlobalFunction.ReturnEmptyString(dr[2]), //okie
                            SMAGTitle = GlobalFunction.ReturnEmptyString(dr[14]),
                            SMAGID = GlobalFunction.ReturnEmptyInt(dr[13]),
                            RevYear = GlobalFunction.ReturnEmptyString(dr[17]),
                            MAGTitle = GlobalFunction.ReturnEmptyString(dr[18]),
                            AllotmentClassTitle = GlobalFunction.ReturnEmptyString(dr[19]),
                            AGTitle = GlobalFunction.ReturnEmptyString(dr[20]),
                            GenAccountCode = GlobalFunction.ReturnEmptyString(dr[21])
                        });
                    }
                }
                Connection.Close();
            }
            model.getGeneralAccountList = getGeneralAccountList.ToList();
            return PartialView("GeneralAccount/_TableGA", getGeneralAccountList);
        }
        public ActionResult GetGAForm(int ActionID, int GAID)
        {
            GeneralAccountModel model = new GeneralAccountModel();

            if (ActionID == 2)
            {
                var genAccntGrp = (from a in BOSSDB.Tbl_FMCOA_GeneralAccount where a.GAID == GAID select a).FirstOrDefault();
                model.GeneralAccountList.GeneralAccountID = genAccntGrp.GAID;

                model.GeneralAccountList.GATitle = genAccntGrp.GATitle;
                model.GeneralAccountList.GACode = genAccntGrp.GACode;
                model.GeneralAccountList.IsMiscellaneous = GlobalFunction.ReturnEmptyBool(genAccntGrp.isMiscellaneousAccount);
                model.GeneralAccountList.IsReserve = GlobalFunction.ReturnEmptyBool(genAccntGrp.isReserve);

                model.GeneralAccountList.IsRelease = GlobalFunction.ReturnEmptyBool(genAccntGrp.isFullRelease);
                model.GeneralAccountList.IsContinuing = GlobalFunction.ReturnEmptyBool(genAccntGrp.isContinuing);
                model.GeneralAccountList.IsOBRCash = GlobalFunction.ReturnEmptyBool(genAccntGrp.isOBRCashAdvance);
                model.GeneralAccountList.NormalBal = GlobalFunction.ReturnEmptyString(genAccntGrp.NormalBalance);

                if (genAccntGrp.isReserve == false)
                {
                    model.GeneralAccountList.ReservePercent = "0";
                }
                else
                {
                    model.GeneralAccountList.ReservePercent = genAccntGrp.ReservePercent.ToString();
                }

                model.GeneralAccountList.SMAGID = GlobalFunction.ReturnEmptyInt(genAccntGrp.SMAGID);
                model.GeneralAccountList.MAGID = GlobalFunction.ReturnEmptyInt(genAccntGrp.Tbl_FMCOA_SubMajorAccountGroup.Tbl_FMCOA_MajorAccountGroup.MAGID);
                model.GeneralAccountList.AGID = GlobalFunction.ReturnEmptyInt(genAccntGrp.Tbl_FMCOA_SubMajorAccountGroup.Tbl_FMCOA_MajorAccountGroup.Tbl_FMCOA_AccountGroup.AGID);
                model.GeneralAccountList.AllotmentClassID = GlobalFunction.ReturnEmptyInt(genAccntGrp.Tbl_FMCOA_SubMajorAccountGroup.Tbl_FMCOA_MajorAccountGroup.Tbl_FMCOA_AccountGroup.AllotmentClassID);
                model.GeneralAccountList.RevID = GlobalFunction.ReturnEmptyInt(genAccntGrp.Tbl_FMCOA_SubMajorAccountGroup.Tbl_FMCOA_MajorAccountGroup.Tbl_FMCOA_AccountGroup.RevID);

                var allotClass = (from a in BOSSDB.Tbl_FMCOA_AllotmentClass where a.RevID == model.GeneralAccountList.RevID orderby a.AllotmentClassTitle select a).ToList();

                var accntGrpCheckAllot = (from a in BOSSDB.Tbl_FMCOA_AccountGroup where a.AGID == model.GeneralAccountList.AGID select a).FirstOrDefault();

                foreach (var item in allotClass)
                {
                    model.AllotClassList.Add(new SelectListItem() { Text = item.AllotmentClassTitle, Value = item.AllotmentClassID.ToString() });
                }

                if (accntGrpCheckAllot.AllotmentClassID == null)
                {
                    model.GeneralAccountList.AllotmentClassID = 0;
                    var accntClass = (from a in BOSSDB.Tbl_FMCOA_AccountGroup where a.RevID == model.GeneralAccountList.RevID && a.AllotmentClassID == null orderby a.AGTitle select a).ToList();
                    model.AccntGrpList = new SelectList(accntClass, "AGID", "AGTitle");
                }
                else
                {
                    model.GeneralAccountList.AllotmentClassID = accntGrpCheckAllot.Tbl_FMCOA_AllotmentClass.AllotmentClassID;
                    var accntClass = (from a in BOSSDB.Tbl_FMCOA_AccountGroup where a.RevID == model.GeneralAccountList.RevID && a.AllotmentClassID == model.GeneralAccountList.AllotmentClassID orderby a.AGTitle select a).ToList();
                    model.AccntGrpList = new SelectList(accntClass, "AGID", "AGTitle");
                }

                model.AccntGrpList = (from li in model.AccntGrpList orderby li.Text select li).ToList();

                var majAccntGrp = (from a in BOSSDB.Tbl_FMCOA_MajorAccountGroup where a.AGID == model.GeneralAccountList.AGID select a).ToList();
                model.MajAccntGrpList = new SelectList(majAccntGrp, "MAGID", "MAGTitle");
                model.MajAccntGrpList = (from li in model.MajAccntGrpList orderby li.Text select li).ToList();

                var subMajAccntGrp = (from a in BOSSDB.Tbl_FMCOA_SubMajorAccountGroup where a.MAGID == model.GeneralAccountList.MAGID select a).ToList();
                model.SubMajAccntGrpList = new SelectList(subMajAccntGrp, "SMAGID", "SMAGTitle");
                model.SubMajAccntGrpList = (from li in model.SubMajAccntGrpList orderby li.Text select li).ToList();



                if (genAccntGrp.isSubAccount != 0)
                {
                    var genAccntGrpList = (from a in BOSSDB.Tbl_FMCOA_GeneralAccount where a.SMAGID == model.GeneralAccountList.SMAGID && a.isSubAccount == 0 orderby a.GATitle select a).ToList();
                    model.GenAccntGrpList = new SelectList(genAccntGrpList, "GAID", "GATitle");
                    model.GenAccntGrpList = (from li in model.GenAccntGrpList orderby li.Text select li).ToList();

                    model.GeneralAccountList.GAID = genAccntGrp.isSubAccount;
                    model.GeneralAccountList.isSubAccountCheckBox = true;
                    model.GeneralAccountList.isContraAccountCheckBox = false;
                }
                else if (genAccntGrp.isContraAccount != 0)
                {
                    var genAccntGrpList = (from a in BOSSDB.Tbl_FMCOA_GeneralAccount where a.SMAGID == model.GeneralAccountList.SMAGID && a.isContraAccount == 0 && a.isSubAccount == 0 orderby a.GATitle select a).ToList();
                    model.GenAccntGrpList = new SelectList(genAccntGrpList, "GAID", "GATitle");
                    model.GenAccntGrpList = (from li in model.GenAccntGrpList orderby li.Text select li).ToList();

                    model.GeneralAccountList.GAID = genAccntGrp.isContraAccount;
                    model.GeneralAccountList.isContraAccountCheckBox = true;
                    model.GeneralAccountList.isSubAccountCheckBox = false;
                }
                else
                {
                    var genAccntGrpList = (from a in BOSSDB.Tbl_FMCOA_GeneralAccount where a.SMAGID == model.GeneralAccountList.SMAGID && a.isSubAccount == 0 orderby a.GATitle select a).ToList();
                    model.GenAccntGrpList = new SelectList(genAccntGrpList, "GAID", "GATitle");
                    model.GenAccntGrpList = (from li in model.GenAccntGrpList orderby li.Text select li).ToList();

                    model.GeneralAccountList.GAID = 0;
                    model.GeneralAccountList.isContraAccountCheckBox = false;
                    model.GeneralAccountList.isSubAccountCheckBox = false;
                }

            }
            else
            {
                var allotClassRevYear = (from a in BOSSDB.Tbl_FMCOA_RevisionYear orderby a.isUsed descending select a.RevID).FirstOrDefault();
                if (allotClassRevYear != 0)
                {
                    model.GeneralAccountList.RevID = allotClassRevYear;

                    var allotClass = (from a in BOSSDB.Tbl_FMCOA_AllotmentClass where a.RevID == model.GeneralAccountList.RevID orderby a.AllotmentClassTitle select a).ToList();
                    if (allotClass.Count > 0)
                    {

                        foreach (var item in allotClass)
                        {
                            model.AllotClassList.Add(new SelectListItem() { Text = item.AllotmentClassTitle, Value = item.AllotmentClassID.ToString() });
                        }

                        var firstAllotClass = (from a in allotClass orderby a.AllotmentClassTitle select a).FirstOrDefault();

                        int? allotID = 0;
                        if (firstAllotClass.AllotmentClassID == 0)
                        {
                            allotID = null;

                        }
                        else
                        {
                            allotID = firstAllotClass.AllotmentClassID;
                        }

                        var accntGrpCheckAllot = (from a in BOSSDB.Tbl_FMCOA_AccountGroup where a.AllotmentClassID == allotID && a.RevID == model.GeneralAccountList.RevID select a).ToList();

                        if (accntGrpCheckAllot.Count > 0)
                        {
                            model.AccntGrpList = new SelectList(accntGrpCheckAllot, "AGID", "AGTitle");
                            model.AccntGrpList = (from li in model.AccntGrpList orderby li.Text select li).ToList();

                            var firstAccntGrp = (from a in accntGrpCheckAllot orderby a.AGTitle select a).FirstOrDefault();
                            var majAccntGrp = (from a in BOSSDB.Tbl_FMCOA_MajorAccountGroup where a.AGID == firstAccntGrp.AGID select a).ToList();


                            if (majAccntGrp.Count > 0)
                            {
                                model.MajAccntGrpList = new SelectList(majAccntGrp, "MAGID", "MAGTitle");
                                model.MajAccntGrpList = (from li in model.MajAccntGrpList orderby li.Text select li).ToList();

                                var firstMajAccnt = (from a in majAccntGrp orderby a.MAGTitle select a).FirstOrDefault();
                                var subMajAccntGrp = (from a in BOSSDB.Tbl_FMCOA_SubMajorAccountGroup where a.MAGID == firstMajAccnt.MAGID select a).ToList();

                                if (subMajAccntGrp.Count > 0)
                                {
                                    model.SubMajAccntGrpList = new SelectList(subMajAccntGrp, "SMAGID", "SMAGTitle");
                                    model.SubMajAccntGrpList = (from li in model.SubMajAccntGrpList orderby li.Text select li).ToList();

                                    var firstSubMajAccnt = (from a in subMajAccntGrp orderby a.SMAGTitle select a).FirstOrDefault();
                                    var genAccntGrp = (from a in BOSSDB.Tbl_FMCOA_GeneralAccount where a.SMAGID == firstSubMajAccnt.SMAGID select a).ToList();

                                    if (genAccntGrp.Count > 0)
                                    {
                                        model.GenAccntGrpList = new SelectList(genAccntGrp, "GAID", "GATitle");
                                        model.GenAccntGrpList = (from li in model.GenAccntGrpList orderby li.Text select li).ToList();
                                    }
                                }
                            }

                        }
                    }
                }
            }

            var RevYear = (from a in BOSSDB.Tbl_FMCOA_RevisionYear orderby a.isUsed descending select a).ToList();
            model.RevYearList = new SelectList(RevYear, "RevID", "RevYear");

            model.AllotClassList.Add(new SelectListItem() { Text = "N/A", Value = "000" });

            model.ActionID = ActionID;
            return PartialView("GeneralAccount/_GAForm", model);
        }






















        //-------------------------------------------------------------------
        // Onchange 
        //-------------------------------------------------------------------
        public ActionResult ChangeRevisionYear_AllotClass(int RevID, AccountGroupModel model)
        {
            var allotClass = (from a in BOSSDB.Tbl_FMCOA_AllotmentClass where a.RevID == RevID orderby a.AllotmentClassTitle select a).ToList();
            foreach (var item in allotClass)
            {
                model.AllotClassList.Add(new SelectListItem() { Text = item.AllotmentClassTitle, Value = item.AllotmentClassID.ToString() });

            }
            return Json(new SelectList(allotClass, "AllotmentClassID", "AllotmentClassTitle"), JsonRequestBehavior.AllowGet);

        }
        public ActionResult ChangeAllotClass_AccntGrp(int? AllotmentClassID, int RevID, MajorAccountGroupModel model)
        {
            if (AllotmentClassID == 0 || AllotmentClassID == null)
            {
                AllotmentClassID = null;
            }

            var accntClass = (from a in BOSSDB.Tbl_FMCOA_AccountGroup where a.AllotmentClassID == AllotmentClassID && a.RevID == RevID orderby a.AGTitle select a).ToList();
            return Json(new SelectList(accntClass, "AGID", "AGTitle"), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeAccntGrp_MajAccntGrp(int AGID, MajorAccountGroupModel model)
        {
            var accntClass = (from a in BOSSDB.Tbl_FMCOA_MajorAccountGroup where a.AGID == AGID orderby a.MAGTitle select a).ToList();
            return Json(new SelectList(accntClass, "MAGID", "MAGTitle"), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeMajAccntGrp_SubMajAccntGrp(int MAGID, GeneralAccountModel model)
        {
            var SubmajAccntClass = (from a in BOSSDB.Tbl_FMCOA_SubMajorAccountGroup where a.MAGID == MAGID orderby a.SMAGTitle select a).ToList();
            return Json(new SelectList(SubmajAccntClass, "SMAGID", "SMAGTitle"), JsonRequestBehavior.AllowGet);
        }
        //public ActionResult ChangeSubMajAccntGrp_GenAccntGrp(int SMAGID, bool chckBoxContra, bool chckBoxSub, GeneralAccountModel model)
        //{
        //    List<Tbl_FMCOA_GeneralAccount> GenAccntClass = new List<Tbl_FMCOA_GeneralAccount>();
        //    if (chckBoxContra == true)
        //    {
        //        GenAccntClass = (from a in BOSSDB.Tbl_FMCOA_GeneralAccount where a.SMAGID == SMAGID && a.isContraAccount == 0 && a.isSubAccount == 0 orderby a.GATitle select a).ToList();

        //    }
        //    else if (chckBoxSub == true)
        //    {
        //        GenAccntClass = (from a in BOSSDB.Tbl_FMCOA_GeneralAccount where a.SMAGID == SMAGID && a.isSubAccount == 0 orderby a.GATitle select a).ToList();

        //    }
        //    return Json(new SelectList(GenAccntClass, "GAID", "GATitle"), JsonRequestBehavior.AllowGet);
        //}
        //================================
        // OM CHANGE CODES
        //================================
        public ActionResult ChangeAccountCode(int AGID, MajorAccountGroupModel model)
        {
            var accntGrp = (from a in BOSSDB.Tbl_FMCOA_AccountGroup where a.AGID == AGID select a).FirstOrDefault();
            var passCon = "";

            if (accntGrp != null)
            {
                passCon = accntGrp.AGCode;
            }
            var result = new { passCon = passCon };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeMajAccountCode(int MAGID, MajorAccountGroupModel model)
        {
            var majAccntGrp = (from a in BOSSDB.Tbl_FMCOA_MajorAccountGroup where a.MAGID == MAGID select a).FirstOrDefault();
            var passCon = "";
            if (majAccntGrp != null)
            {
                var accntGrp = (from a in BOSSDB.Tbl_FMCOA_AccountGroup where a.AGID == majAccntGrp.AGID select a).FirstOrDefault();
                if (accntGrp != null)
                {
                    passCon = (accntGrp.AGCode + "-" + majAccntGrp.MAGCode + "-");
                }
            }
            var result = new { passCon = passCon };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeSubMajorAccountCode(int SMAGID, MajorAccountGroupModel model)
        {
            var subMajAccntGrp = (from a in BOSSDB.Tbl_FMCOA_SubMajorAccountGroup where a.SMAGID == SMAGID select a).FirstOrDefault();
            var majAccntGrp = (from a in BOSSDB.Tbl_FMCOA_MajorAccountGroup where a.MAGID == subMajAccntGrp.MAGID select a).FirstOrDefault();
            var accntGrp = (from a in BOSSDB.Tbl_FMCOA_AccountGroup where a.AGID == majAccntGrp.AGID select a).FirstOrDefault();
            var passCon = "";

            if (accntGrp != null)
            {
                passCon = (accntGrp.AGCode + "-" + majAccntGrp.MAGCode + "-" + subMajAccntGrp.SMAGCode + "-");
            }
            var result = new { passCon = passCon };
            return Json(result, JsonRequestBehavior.AllowGet);
        }




       
    }
}