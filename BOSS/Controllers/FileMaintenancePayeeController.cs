using BOSS.GlobalFunctions;
using BOSS.Models;
using BOSS.Models.FMmodels.FMPayeeModels;
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

            var SQLQuery = "SELECT [PayeeID],[Name],[Address],[DeptTitle] FROM [BOSS].[dbo].[Tbl_FMPayee], [dbo].[Tbl_FMDepartment] where [dbo].[Tbl_FMPayee].DeptID = [dbo].[Tbl_FMDepartment].DeptID";
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
        //Get Add Partial View
        public ActionResult Get_AddPayee()
        {
            PayeeModel model = new PayeeModel();
            return PartialView("_AddPayee", model);
        }
        public JsonResult AddNewPayee(PayeeModel model)
        {
            Tbl_FMPayee payeeTBL = new Tbl_FMPayee();
            payeeTBL.Name = GlobalFunction.ReturnEmptyString(model.getPayeeColumns.Name);
            payeeTBL.Address = GlobalFunction.ReturnEmptyString(model.getPayeeColumns.Address);
            payeeTBL.DeptID = GlobalFunction.ReturnEmptyInt(model.DeptID);

            BOSSDB.Tbl_FMPayee.Add(payeeTBL);

            BOSSDB.SaveChanges();
            return Json(payeeTBL);
        }
        public ActionResult Get_UpdatePayee(PayeeModel model, int PayeeID)
        {
            Tbl_FMPayee tblPayee= (from e in BOSSDB.Tbl_FMPayee where e.PayeeID == PayeeID select e).FirstOrDefault();

            model.getPayeeColumns.Name = tblPayee.Name;
            model.getPayeeColumns.Address = tblPayee.Address;
            model.DeptID = Convert.ToInt32(tblPayee.DeptID);
            model.PayeeID = PayeeID;
            return PartialView("_UpdatePayee", model);
        }
        public ActionResult UpdatePayee(PayeeModel model)
        {
            Tbl_FMPayee payeeTBL = (from e in BOSSDB.Tbl_FMPayee where e.PayeeID == model.PayeeID select e).FirstOrDefault();

            payeeTBL.Name = GlobalFunction.ReturnEmptyString(model.getPayeeColumns.Name);
            payeeTBL.Address = GlobalFunction.ReturnEmptyString(model.getPayeeColumns.Address);
            payeeTBL.DeptID = GlobalFunction.ReturnEmptyInt(model.DeptID);
            BOSSDB.Entry(payeeTBL);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeletePayee(PayeeModel model, int PayeeID)
        {
            Tbl_FMPayee tblPayee = (from e in BOSSDB.Tbl_FMPayee where e.PayeeID == PayeeID select e).FirstOrDefault();
            BOSSDB.Tbl_FMPayee.Remove(tblPayee);
            BOSSDB.SaveChanges();
            return RedirectToAction("FilePayee");
        }
    }
}