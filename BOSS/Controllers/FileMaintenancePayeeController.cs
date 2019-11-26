using BOSS.GlobalFunctions;
using BOSS.Models;
using BOSS.Models.FMmodels.FMPayeeModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class FileMaintenancePayeeController : Controller
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        DatatypeValidation GlobalFunction = new DatatypeValidation();
        [Authorize]
        // GET: FileMaintenancePayee
        public ActionResult FilePayee()
        {
            PayeeModel model = new PayeeModel();
            return View();
        }
        //Display Data Table
        public ActionResult GetPayeeDTable()
        {
            PayeeModel model = new PayeeModel();
            List<PayeeList> getPayeeList = new List<PayeeList>();

            var SQLQuery = "SELECT [PayeeID],[Name],[Address],[DeptTitle] FROM [BOSS].[dbo].[Tbl_FMPayee], [dbo].[Tbl_FMRes_Department] where [dbo].[Tbl_FMPayee].DeptID = [dbo].[Tbl_FMRes_Department].DeptID";
            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_Payee]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        getPayeeList.Add(new PayeeList()
                        {
                            PayeeID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            Name = GlobalFunction.ReturnEmptyString(dr[1]),
                            Address = GlobalFunction.ReturnEmptyString(dr[2]),
                            DeptTitle = GlobalFunction.ReturnEmptyString(dr[3])
                        });
                    }
                }
                Connection.Close();
            }
            model.getPayeeList = getPayeeList.ToList();

            return PartialView("_TablePayee", model.getPayeeList);
        }
        public ActionResult GetPayeeForm(int ActionID, int PrimaryID)
        {
            PayeeModel model = new PayeeModel();

            if (ActionID == 2)
            {
                var payee = (from a in BOSSDB.Tbl_FMPayee where a.PayeeID == PrimaryID select a).FirstOrDefault();
                model.PayeeList.Name = payee.Name;
                model.PayeeList.Address = payee.Address;
                model.DeptID = Convert.ToInt32(payee.DeptID);
                model.PayeeList.PayeeID = payee.PayeeID;
            }
            model.ActionID = ActionID;
            return PartialView("_PayeeForm", model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SavePayee(PayeeModel model)
        {
            var isExist = "";
            if (ModelState.IsValid)
            {
                var payeeName = model.PayeeList.Name;
                var payeeAddress = model.PayeeList.Address;
                payeeName = Regex.Replace(payeeName, @"\s\s+", "");
                payeeName = Regex.Replace(payeeName, @"^\s+", "");
                payeeName = Regex.Replace(payeeName, @"\s+$", "");
                payeeName = new CultureInfo("en-US").TextInfo.ToTitleCase(payeeName);
                payeeAddress = new CultureInfo("en-us").TextInfo.ToTitleCase(payeeAddress);
                Tbl_FMPayee checkPayee = (from a in BOSSDB.Tbl_FMPayee where (a.Name == payeeName || a.Address == payeeAddress) select a).FirstOrDefault();

                if (model.ActionID == 1)
                {
                    if (checkPayee == null)
                    {
                        isExist = "false";
                    }
                    else if (checkPayee != null)
                    {
                        isExist = "true";
                    }
                }
                if (isExist == "false")
                {
                    Tbl_FMPayee payee = new Tbl_FMPayee();
                    payee.Name = payeeName;
                    payee.Address = payeeAddress;
                    payee.DeptID = model.DeptID;
                    BOSSDB.Tbl_FMPayee.Add(payee);
                    BOSSDB.SaveChanges();
                }
                else if (model.ActionID == 2)
                {                                                                                                                                                                                                                                                                                                                                           
                    Tbl_FMPayee payeeTbl = (from a in BOSSDB.Tbl_FMPayee where a.PayeeID == model.PayeeList.PayeeID select a).FirstOrDefault();
                    List<Tbl_FMPayee> namePay = (from e in BOSSDB.Tbl_FMPayee where e.Name == payeeName select e).ToList();
                    List<Tbl_FMPayee> addPay = (from e in BOSSDB.Tbl_FMPayee where e.Address == payeeAddress select e).ToList();
                    List<Tbl_FMPayee> deptPay = (from e in BOSSDB.Tbl_FMPayee where e.DeptID == model.DeptID select e).ToList();
                    if (checkPayee != null)
                    {
                        if (payeeTbl.Name == payeeName && payeeTbl.Address == payeeAddress && payeeTbl.DeptID == model.DeptID)
                        {
                            isExist = "justUpdate";
                        }
                        else
                        {
                            if (payeeTbl.Name != payeeName && namePay.Count >= 1 || payeeTbl.Address != payeeAddress && addPay.Count >= 1)
                            {
                                isExist = "true";
                            }
                            else
                            {
                                isExist = "justUpdate";
                            }
                        }
                    }
                    else if (checkPayee == null)
                    {
                        isExist = "justUpdate";
                    }
                    if (isExist == "justUpdate")
                    {
                        payeeTbl.Name = payeeName;
                        payeeTbl.Address = payeeAddress;
                        payeeTbl.DeptID = model.DeptID;
                        BOSSDB.Entry(payeeTbl);
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
        public ActionResult DeletePayee(int PrimaryID)
        {
            Tbl_FMPayee payii = (from a in BOSSDB.Tbl_FMPayee where a.PayeeID == PrimaryID select a).FirstOrDefault();
            var confirmDelete = "";
            if (payii != null)
            {
                confirmDelete = "false";
            }
            var result = new { confirmDelete = confirmDelete };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConfirmDelete(int PrimaryID)
        {
            Tbl_FMPayee tblPayee = (from e in BOSSDB.Tbl_FMPayee where e.PayeeID == PrimaryID select e).FirstOrDefault();
            BOSSDB.Tbl_FMPayee.Remove(tblPayee);
            BOSSDB.SaveChanges();
            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}