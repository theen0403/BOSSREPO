using BOSS.GlobalFunctions;
using BOSS.Models;
using BOSS.Models.FMmodels.FMBarangayModels;
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
    public class FileMaintenanceBarangayController : Controller
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        DatatypeValidation GlobalFunction = new DatatypeValidation();
        [Authorize]
        // GET: FileMaintenanceBarangay
        public ActionResult FileBarangay()
        {
            BrgyNameModel model = new BrgyNameModel();
            return View(model);
        }
        //---------------------------------------------------------------------------------------------------------------------
        //Barangay Name Tab
        //---------------------------------------------------------------------------------------------------------------------
        public ActionResult GetBrgyNameTab()
        {
            BrgyNameModel model = new BrgyNameModel();
            return PartialView("BrgyNameTab/IndexBrgyName", model);
        }
        public ActionResult GetBrgyNameForm(int ActionID, int PrimaryID)
        {
            BrgyNameModel model = new BrgyNameModel();
            if (ActionID == 2)
            {
                var brgy = (from a in BOSSDB.Tbl_FMBrgy_Barangay where a.BrgyID == PrimaryID select a).FirstOrDefault();
                model.BarangayNameList.BrgyID = brgy.BrgyID;
                model.BarangayNameList.BrgyName = brgy.BrgyName;
            }
            model.ActionID = ActionID;
            return PartialView("BrgyNameTab/_BrgyNameForm", model);
        }
        public ActionResult GetBrgyNameDTable()
        {
            BrgyNameModel model = new BrgyNameModel();
            List<BarangayNameList> getBarangayNameList = new List<BarangayNameList>();
            var SQLQuery = "SELECT * FROM [Tbl_FMBrgy_Barangay]";
            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_Brgy]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        getBarangayNameList.Add(new BarangayNameList()
                        {
                            BrgyID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            BrgyName = GlobalFunction.ReturnEmptyString(dr[1])
                        });
                    }
                }
                Connection.Close();
            }
            model.getBarangayNameList = getBarangayNameList.ToList();
            return PartialView("BrgyNameTab/_TableBrgyName", getBarangayNameList);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveBrgyName(BrgyNameModel model)
        {
            var isExist = "";
            if (ModelState.IsValid)
            {
                var brgyname = model.BarangayNameList.BrgyName;

                Tbl_FMBrgy_Barangay checkbrgy = (from a in BOSSDB.Tbl_FMBrgy_Barangay where (a.BrgyName == brgyname) select a).FirstOrDefault();

                if (model.ActionID == 1)
                {
                    if (checkbrgy == null)
                    {
                        Tbl_FMBrgy_Barangay brgy = new Tbl_FMBrgy_Barangay();
                        brgy.BrgyName = brgyname;
                        BOSSDB.Tbl_FMBrgy_Barangay.Add(brgy);
                        BOSSDB.SaveChanges();
                        isExist = "false";
                    }
                    else if (checkbrgy != null)
                    {
                        isExist = "true";
                    }
                }
                else if (model.ActionID == 2)
                {
                    Tbl_FMBrgy_Barangay bgryii = (from a in BOSSDB.Tbl_FMBrgy_Barangay where a.BrgyID == model.BarangayNameList.BrgyID select a).FirstOrDefault();
                    List<Tbl_FMBrgy_Barangay> brgyCount = (from e in BOSSDB.Tbl_FMBrgy_Barangay where e.BrgyName == brgyname select e).ToList();
                    if (checkbrgy != null)
                    {
                        if (bgryii.BrgyName != brgyname && brgyCount.Count >= 1)
                        {
                            isExist = "true";
                        }
                        else
                        {
                            isExist = "justUpdate";
                        }
                    }
                    else if (checkbrgy == null)
                    {
                        isExist = "justUpdate";
                    }

                    if (isExist == "justUpdate")
                    {
                        bgryii.BrgyName = brgyname;
                        BOSSDB.Entry(bgryii);
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
        public ActionResult DeleteBrgyName(int PrimaryID)
        {
            Tbl_FMBrgy_Barangay brgys = (from a in BOSSDB.Tbl_FMBrgy_Barangay where a.BrgyID == PrimaryID select a).FirstOrDefault();
            Tbl_FMBrgy_BrgyBankAccount BrgyBankaccnt = (from a in BOSSDB.Tbl_FMBrgy_BrgyBankAccount where a.BrgyID == PrimaryID select a).FirstOrDefault();
            var confirmDelete = "";
            if (brgys != null)
            {
                if (BrgyBankaccnt != null)
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
        public ActionResult ConfirmDeleteBrgyName(int PrimaryID)
        {
            Tbl_FMBrgy_Barangay brgyss = (from a in BOSSDB.Tbl_FMBrgy_Barangay where a.BrgyID == PrimaryID select a).FirstOrDefault();
            BOSSDB.Tbl_FMBrgy_Barangay.Remove(brgyss);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //---------------------------------------------------------------------------------------------------------------------
        //Barangay Collector Tab
        //---------------------------------------------------------------------------------------------------------------------
        public ActionResult GetBrgyCollectorTab()
        {
            BrgyCollectorModel model = new BrgyCollectorModel();
            return PartialView("BrgyCollectorTab/IndexBrgyCollector", model);
        }
        public ActionResult GetBrgyCollertorForm(int ActionID, int PrimaryID)
        {
            BrgyCollectorModel model = new BrgyCollectorModel();
            if (ActionID == 2)
            {
                var brgycollector = (from a in BOSSDB.Tbl_FMBrgy_BrgyCollector where a.BrgyCollectorID == PrimaryID select a).FirstOrDefault();
                model.BrangayCollectorList.BrgyCollectorID = brgycollector.BrgyCollectorID;
                model.BrangayCollectorList.Fname = brgycollector.Fname;
                model.BrangayCollectorList.Mname = brgycollector.Mname;
                model.BrangayCollectorList.Lname = brgycollector.Lname;
            }
            model.ActionID = ActionID;
            return PartialView("BrgyCollectorTab/_BrgyCollectorForm", model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveBrgyCollector(BrgyCollectorModel model)
        {
            var isExist = "";
            if (ModelState.IsValid)
            {
                var fname = model.BrangayCollectorList.Fname;
                var mname = model.BrangayCollectorList.Mname;
                var lname = model.BrangayCollectorList.Lname;
                var collectorid = model.BrangayCollectorList.BrgyCollectorID;

                Tbl_FMBrgy_BrgyCollector checkcollector = (from a in BOSSDB.Tbl_FMBrgy_BrgyCollector where (a.Fname == fname && a.Mname == mname && a.Lname == lname) select a).FirstOrDefault();

                if (model.ActionID == 1)
                {
                    if (checkcollector == null)
                    {
                        Tbl_FMBrgy_BrgyCollector brgyCollect = new Tbl_FMBrgy_BrgyCollector();
                        brgyCollect.Fname = fname;
                        brgyCollect.Mname = mname;
                        brgyCollect.Lname = lname;
                        BOSSDB.Tbl_FMBrgy_BrgyCollector.Add(brgyCollect);
                        BOSSDB.SaveChanges();
                        isExist = "false";
                    }
                    else if (checkcollector != null)
                    {
                        isExist = "true";
                    }
                }
                else if (model.ActionID == 2)
                {
                    Tbl_FMBrgy_BrgyCollector collectii = (from a in BOSSDB.Tbl_FMBrgy_BrgyCollector where a.BrgyCollectorID == collectorid select a).FirstOrDefault();
                    List<Tbl_FMBrgy_BrgyCollector> fnameCount = (from e in BOSSDB.Tbl_FMBrgy_BrgyCollector where e.Fname == fname select e).ToList();
                    List<Tbl_FMBrgy_BrgyCollector> lnameCount = (from e in BOSSDB.Tbl_FMBrgy_BrgyCollector where e.Lname == lname select e).ToList();
                    List<Tbl_FMBrgy_BrgyCollector> mnameCount = (from e in BOSSDB.Tbl_FMBrgy_BrgyCollector where e.Mname == mname select e).ToList();
                    if (checkcollector != null)
                    {
                        if (mname != null ){
                            if (collectii.Fname != fname && fnameCount.Count >= 1 && collectii.Lname != lname && lnameCount.Count >= 1 && collectii.Mname != mname)
                                { isExist = "true"; } else { isExist = "justUpdate"; }
                        }
                        else
                        {
                            if (collectii.Fname != fname && fnameCount.Count >= 1 && collectii.Lname != lname && lnameCount.Count >= 1)
                            { isExist = "true"; } else { isExist = "justUpdate"; }
                        }
                    }
                    else if (checkcollector == null)
                    { isExist = "justUpdate"; }

                    if (isExist == "justUpdate")
                    {
                        collectii.Fname = fname;
                        collectii.Mname = mname;
                        collectii.Lname = lname;
                        BOSSDB.Entry(collectii);
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
        public ActionResult GetBrgyCollectorDTable()
        {
            BrgyCollectorModel model = new BrgyCollectorModel();
            List<BarangayCollectorList> getBarangayCollectorList = new List<BarangayCollectorList>();
            var SQLQuery = " Select BrgyCollectorID, CONCAT([Fname],' ',[Mname] ,' ',[Lname] , ' ' ) as Fullname from [Tbl_FMBrgy_BrgyCollector]";
            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_Brgy]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        getBarangayCollectorList.Add(new BarangayCollectorList()
                        {
                            BrgyCollectorID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            Fullname = GlobalFunction.ReturnEmptyString(dr[1])
                        });
                    }
                }
                Connection.Close();
            }
            model.getBarangayCollectorList = getBarangayCollectorList.ToList();
            return PartialView("BrgyCollectorTab/_TableBrgyCollector", getBarangayCollectorList);
        }
        public ActionResult DeleteBrgyCollector(int PrimaryID)
        {
            Tbl_FMBrgy_BrgyCollector collector = (from a in BOSSDB.Tbl_FMBrgy_BrgyCollector where a.BrgyCollectorID == PrimaryID select a).FirstOrDefault();
            var confirmDelete = "";
            if (collector != null)
            {
                confirmDelete = "false";
            }
            var result = new { confirmDelete = confirmDelete };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConfirmDeleteBrgyCollector(int PrimaryID)
        {
            Tbl_FMBrgy_BrgyCollector collectors = (from a in BOSSDB.Tbl_FMBrgy_BrgyCollector where a.BrgyCollectorID == PrimaryID select a).FirstOrDefault();
            BOSSDB.Tbl_FMBrgy_BrgyCollector.Remove(collectors);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //---------------------------------------------------------------------------------------------------------------------
        //Barangay Bank Account Tab
        //---------------------------------------------------------------------------------------------------------------------
        public ActionResult GetBrgyBankAccntTab()
        {
            return PartialView("BrgyBankAccountTab/IndexBrgyBankAccount");
        }
        public ActionResult GetBrgyBankAccntForm(int ActionID, int PrimaryID)
        {
            BrgyBankAccountModel model = new BrgyBankAccountModel();
            if (ActionID == 2)
            {
                var brgy = (from a in BOSSDB.Tbl_FMBrgy_BrgyBankAccount where a.BrgyBankAccntID == PrimaryID select a).FirstOrDefault();
                model.BrgyBankAccountList.BrgyBankAccntID = brgy.BrgyBankAccntID;
                model.BrgyID = Convert.ToInt32(brgy.BrgyID);
                model.BankID = Convert.ToInt32(brgy.Tbl_FMBank_BankAccounts.BankID);
                var accntnoCount = (from a in BOSSDB.Tbl_FMBank_BankAccounts orderby a.AccntNo where a.BankID == model.BankID select a).ToList();
                if (accntnoCount.Count > 0)
                {
                    model.BankAccntNoList = new SelectList(accntnoCount, "BankAccntID", "AccntNo");
                }
            }
            else
            {
                var bankTbl = (from a in BOSSDB.Tbl_FMBank_Banks orderby a.BankTitle select a.BankID).FirstOrDefault();
                var accntnoCount = (from a in BOSSDB.Tbl_FMBank_BankAccounts orderby a.AccntNo where a.BankID == bankTbl select a).ToList();
                if (accntnoCount.Count > 0)
                {
                    model.BankAccntNoList = new SelectList(accntnoCount, "BankAccntID", "AccntNo");
                }
            }
            model.BankAccntNoList = (from li in model.BankAccntNoList orderby li.Text select li).ToList();
            model.ActionID = ActionID;
            return PartialView("BrgyBankAccountTab/_BrgyBankAccountForm", model);
        }
        public ActionResult onChangeAccntNo(BrgyBankAccountModel model, int BankID)
        {
            var AccntNoClass = (from a in BOSSDB.Tbl_FMBank_BankAccounts where a.BankID == BankID select a).ToList();
            return Json(new SelectList(AccntNoClass, "BankAccntID", "AccntNo"), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBrgyBankAccntDTable()
        {
            BrgyBankAccountModel model = new BrgyBankAccountModel();
            List<BrgyBankAccountList> getBrgyBankAccountList = new List<BrgyBankAccountList>();
            var SQLQuery = "SELECT [BrgyBankAccntID], Tbl_FMBrgy_Barangay.BrgyName, Tbl_FMBank_Banks.BankTitle, Tbl_FMBank_BankAccounts.AccntNo FROM [Tbl_FMBrgy_BrgyBankAccount], Tbl_FMBrgy_Barangay, Tbl_FMBank_Banks, Tbl_FMBank_BankAccounts WHERE Tbl_FMBank_Banks.BankID = Tbl_FMBank_BankAccounts.BankID And Tbl_FMBank_BankAccounts.BankAccntID =[Tbl_FMBrgy_BrgyBankAccount].BankAccntID And Tbl_FMBrgy_Barangay.BrgyID = [Tbl_FMBrgy_BrgyBankAccount].BrgyID";
            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_Brgy]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        getBrgyBankAccountList.Add(new BrgyBankAccountList()
                        {
                            BrgyBankAccntID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            BrgyName = GlobalFunction.ReturnEmptyString(dr[1]),
                            BankTitle = GlobalFunction.ReturnEmptyString(dr[2]),
                            AccntNo = GlobalFunction.ReturnEmptyString(dr[3])
                        });
                    }
                }
                Connection.Close();
            }
            model.getBrgyBankAccountList = getBrgyBankAccountList.ToList();
            return PartialView("BrgyBankAccountTab/_TableBrgyBankAccount", getBrgyBankAccountList);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveBrgyBankAccnt(BrgyBankAccountModel model)
        {
            var isExist = "";
            if (ModelState.IsValid)
            {
                var brgyid = model.BrgyID;
                var bankaccntid = model.BankAccntID;
                var brgybankaccntid = model.BrgyBankAccountList.BrgyBankAccntID;

                Tbl_FMBrgy_BrgyBankAccount checkBBA = (from a in BOSSDB.Tbl_FMBrgy_BrgyBankAccount where (a.BrgyID == brgyid && a.BankAccntID == bankaccntid) select a).FirstOrDefault();

                if (model.ActionID == 1)
                {
                    if (checkBBA == null)
                    {
                        Tbl_FMBrgy_BrgyBankAccount brgyBAccnt = new Tbl_FMBrgy_BrgyBankAccount();
                        brgyBAccnt.BrgyID = brgyid;
                        brgyBAccnt.BankAccntID = bankaccntid;
                        BOSSDB.Tbl_FMBrgy_BrgyBankAccount.Add(brgyBAccnt);
                        BOSSDB.SaveChanges();
                        isExist = "false";
                    }
                    else if (checkBBA != null)
                    {
                        isExist = "true";
                    }
                }
                else if (model.ActionID == 2)
                {
                    Tbl_FMBrgy_BrgyBankAccount updateBBA = (from a in BOSSDB.Tbl_FMBrgy_BrgyBankAccount where a.BrgyBankAccntID == brgybankaccntid select a).FirstOrDefault();
                    List<Tbl_FMBrgy_BrgyBankAccount> brgyidCount = (from e in BOSSDB.Tbl_FMBrgy_BrgyBankAccount where e.BrgyID == brgyid select e).ToList();
                    List<Tbl_FMBrgy_BrgyBankAccount> bnkaccntidCount = (from e in BOSSDB.Tbl_FMBrgy_BrgyBankAccount where e.BankAccntID == bankaccntid select e).ToList();
                    if (checkBBA != null)
                    {
                        if (updateBBA.BrgyID != brgyid && brgyidCount.Count >= 1 && updateBBA.BankAccntID != bankaccntid && bnkaccntidCount.Count >= 1)
                        {
                            isExist = "true";
                        }
                        else
                        {
                            isExist = "justUpdate";
                        }
                    }
                    else if (checkBBA == null)
                    {
                        isExist = "justUpdate";
                    }

                    if (isExist == "justUpdate")
                    {
                        updateBBA.BrgyID = brgyid;
                        updateBBA.BankAccntID = bankaccntid;
                        updateBBA.BrgyBankAccntID = brgybankaccntid;
                        BOSSDB.Entry(updateBBA);
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
        public ActionResult DeleteBrgyBankAccnt(int PrimaryID)
        {
            Tbl_FMBrgy_BrgyBankAccount brgybnkaccnt = (from a in BOSSDB.Tbl_FMBrgy_BrgyBankAccount where a.BrgyBankAccntID == PrimaryID select a).FirstOrDefault();
            var confirmDelete = "";
            if (brgybnkaccnt != null)
            {
                confirmDelete = "false";
            }
            var result = new { confirmDelete = confirmDelete };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConfirmDeleteBrgyBankAccnt(int PrimaryID)
        {
            Tbl_FMBrgy_BrgyBankAccount BBA = (from a in BOSSDB.Tbl_FMBrgy_BrgyBankAccount where a.BrgyBankAccntID == PrimaryID select a).FirstOrDefault();
            BOSSDB.Tbl_FMBrgy_BrgyBankAccount.Remove(BBA);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}