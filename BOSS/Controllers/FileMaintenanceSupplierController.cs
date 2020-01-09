using BOSS.GlobalFunctions;
using BOSS.Models;
using BOSS.Models.FMmodels.FMSupplierModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class FileMaintenanceSupplierController : Controller
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        DatatypeValidation GlobalFunction = new DatatypeValidation();
        [Authorize]
        // GET: FileMaintenanceSupplier
        public ActionResult FileMaintenanceSupplier()
        {
            return View();
        }
        public ActionResult GetSupplierForm(int ActionID, int SupplierID)
        {
            SupplierModel model = new SupplierModel();
            if (ActionID == 2)
            {
                var supplier = (from a in BOSSDB.Tbl_FMSupplier where a.SupplierID == SupplierID select a).FirstOrDefault();
                model.SupplierList.CompanyName = supplier.CompanyName;
                model.SupplierList.ProductServices = supplier.ProductServices;
                model.SupplierList.Address = supplier.Address;
                model.SupplierList.TaxType = supplier.TaxType;
                model.SupplierList.FaxNo = supplier.FaxNo;
                model.SupplierList.TelNo = supplier.TelNo;
                model.SupplierList.TIN = supplier.TIN;
                model.SupplierList.MFName = supplier.MFName;
                model.SupplierList.MFAddress = supplier.MFAddress;
                model.SupplierList.MFContactNo = supplier.MFContactNo;
                model.SupplierList.AccreNumber = supplier.AccreNumber;
                model.SupplierList.AccreDate = supplier.AccreDate;
                model.SupplierList.AccreValidUntil = supplier.AccreValidUntil;
                model.SupplierList.AccreApproveBy = supplier.AccreApproveBy;
                model.SupplierList.AccreMOA = supplier.AccreMOA;
                model.SupplierList.SupplierID = supplier.SupplierID;
            }
            model.ActionID = ActionID;
            return PartialView("_SupplierForm", model);
        }
        //public ActionResult GetSupplierDTable()
        //{
        //    SupplierModel model = new SupplierModel();

        //    List<SupplierList> getSupplierList = new List<SupplierList>();

        //    var SQLQuery = "SELECT * FROM [Tbl_FMSupplier]";
        //    using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
        //    {
        //        Connection.Open();
        //        using (SqlCommand command = new SqlCommand("[dbo].[SP_Supplier]", Connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
        //            SqlDataReader dr = command.ExecuteReader();
        //            while (dr.Read())
        //            {
        //                getSupplierList.Add(new SupplierList()
        //                {
        //                    SupplierID = GlobalFunction.ReturnEmptyInt(dr[0]),
        //                    CompanyName = GlobalFunction.ReturnEmptyString(dr[1]),
        //                    Address = GlobalFunction.ReturnEmptyString(dr[3]),
        //                    TelNo = GlobalFunction.ReturnEmptyString(dr[8]),
        //                    MFName = GlobalFunction.ReturnEmptyString(dr[10])
        //                });
        //            }
        //        }
        //        Connection.Close();
        //    }
        //    model.getSupplierList = getSupplierList.ToList();
        //    return PartialView("_TableSupplier", model.getSupplierList);
        //}
        public ActionResult GetSupplierDTable()
        {
            SupplierModel model = new SupplierModel();

            var SQLQuery = @"SELECT * FROM Tbl_FMSupplier";

            using (SqlConnection Connection = new SqlConnection(GlobalFunction.ReturnConnectionString()))
            {
                Connection.Open();
                using (SqlCommand command = new SqlCommand("[dbo].[SP_Supplier]", Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@SQLStatement", SQLQuery));
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {

                        model.getSupplierList.Add(new SupplierList()
                        {
                            SupplierID = GlobalFunction.ReturnEmptyInt(dr[0]),
                            CompanyName = GlobalFunction.ReturnEmptyString(dr[1]),
                            ProductServices = GlobalFunction.ReturnEmptyString(dr[2]),
                            Address = GlobalFunction.ReturnEmptyString(dr[3]),
                            TaxType = GlobalFunction.ReturnEmptyString(dr[4]),
                            //DTIRegNo = GlobalFunction.ReturnEmptyString(dr[5]),
                            //CDARegistry = GlobalFunction.ReturnEmptyString(dr[6]),
                            FaxNo = GlobalFunction.ReturnEmptyString(dr[7]),
                            TelNo = GlobalFunction.ReturnEmptyString(dr[8]),
                            TIN = GlobalFunction.ReturnEmptyString(dr[9]),
                            MFName = GlobalFunction.ReturnEmptyString(dr[10]),
                            MFAddress = GlobalFunction.ReturnEmptyString(dr[11]),
                            MFContactNo = GlobalFunction.ReturnEmptyString(dr[12]),
                            AccreNumber = GlobalFunction.ReturnEmptyString(dr[13]),
                            AccreDate = GlobalFunction.ReturnEmptyString(dr[14]),
                            AccreValidUntil = GlobalFunction.ReturnEmptyString(dr[15]),
                            AccreApproveBy = GlobalFunction.ReturnEmptyString(dr[16]),
                            AccreMOA = GlobalFunction.ReturnEmptyString(dr[17]),
                        });
                    }
                }
                Connection.Close();
            }
            return PartialView("_TableSupplier", model.getSupplierList);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSupplier(SupplierModel model)
        {
            var isExist = "";
            if (ModelState.IsValid)
            {
                var SupplierID = model.SupplierList.SupplierID;
                var CompanyName = model.SupplierList.CompanyName;
                var ProductServices = model.SupplierList.ProductServices;
                var Address = model.SupplierList.Address;
                var TaxType = model.SupplierList.TaxType;
                //var DTIRegNo = model.SupplierList.DTIRegNo;
                //var CDARegistry = model.SupplierList.CDARegistry;
                var FaxNo = model.SupplierList.FaxNo;
                var TelNo = model.SupplierList.TelNo;
                var TIN = model.SupplierList.TIN;
                var MFName = model.SupplierList.MFName;
                var MFAddress = model.SupplierList.MFAddress;
                var MFContactNo = model.SupplierList.MFContactNo;
                var AccreNumber = model.SupplierList.AccreNumber;
                var AccreDate = model.SupplierList.AccreDate;
                var AccreValidity = model.SupplierList.AccreValidUntil;
                var AccreApproveBy = model.SupplierList.AccreApproveBy;
                var AccreMOA = model.SupplierList.AccreMOA;

                CompanyName = GlobalFunction.RemoveSpaces(CompanyName);
                Address = GlobalFunction.AutoCaps_RemoveSpaces(Address);
                ProductServices = GlobalFunction.RemoveSpaces(ProductServices);
                TelNo = GlobalFunction.RemoveSpaces(TelNo);
                FaxNo = GlobalFunction.RemoveSpaces(FaxNo);
                TIN = GlobalFunction.RemoveSpaces(TIN);

                AccreNumber = GlobalFunction.RemoveSpaces(AccreNumber);
                AccreApproveBy = GlobalFunction.AutoCaps_RemoveSpaces(AccreApproveBy);
                AccreMOA = GlobalFunction.RemoveSpaces(AccreMOA);

                MFName = GlobalFunction.AutoCaps_RemoveSpaces(MFName);
                MFContactNo = GlobalFunction.RemoveSpaces(MFContactNo);
                MFAddress = GlobalFunction.AutoCaps_RemoveSpaces(MFAddress);

                List<Tbl_FMSupplier> supplierList = (from a in BOSSDB.Tbl_FMSupplier where (a.AccreNumber == AccreNumber) select a).ToList();
                Tbl_FMSupplier supplierRecord = (from a in BOSSDB.Tbl_FMSupplier where a.SupplierID == SupplierID select a).FirstOrDefault();
                var save = false;
                if (supplierList.Count > 0)
                {
                    foreach (var item in supplierList)
                    {
                        if (supplierRecord != null)
                        {
                            if (GlobalFunction.AutoCaps_RemoveSpaces(item.CompanyName) == GlobalFunction.AutoCaps_RemoveSpaces(CompanyName) && item.SupplierID == supplierRecord.SupplierID)  // walang binago
                            {
                                save = true;
                            }
                            else if (GlobalFunction.AutoCaps_RemoveSpaces(item.CompanyName) != GlobalFunction.AutoCaps_RemoveSpaces(CompanyName) || item.SupplierID == supplierRecord.SupplierID) // may binago pero walang kaparehas
                            {
                                save = true;
                            }
                            else if (GlobalFunction.AutoCaps_RemoveSpaces(item.CompanyName) == GlobalFunction.AutoCaps_RemoveSpaces(CompanyName)) // may binago pero may kaparehas
                            {
                                save = false;
                                break;
                            }
                        }
                        else
                        {
                            if (GlobalFunction.AutoCaps_RemoveSpaces(item.CompanyName) != GlobalFunction.AutoCaps_RemoveSpaces(CompanyName)) // for adding
                            {
                                save = true;
                            }
                            else
                            {
                                save = false;
                                break;
                            }
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
                        switch (model.ActionID)
                        {
                            case 1:
                                Tbl_FMSupplier suppliedAdd = new Tbl_FMSupplier();
                                suppliedAdd.CompanyName = CompanyName;
                                suppliedAdd.ProductServices = ProductServices;
                                suppliedAdd.Address = Address;
                                suppliedAdd.TaxType = TaxType;
                                //suppliedAdd.DTIRegNo = DTIRegNo;
                                //suppliedAdd.CDARegistry = CDARegistry;
                                suppliedAdd.FaxNo = FaxNo;
                                suppliedAdd.TelNo = TelNo;
                                suppliedAdd.TIN = TIN;
                                suppliedAdd.MFName = MFName;
                                suppliedAdd.MFAddress = MFAddress;
                                suppliedAdd.MFContactNo = MFContactNo;
                                suppliedAdd.AccreNumber = AccreNumber;
                                suppliedAdd.AccreDate = AccreDate;
                                suppliedAdd.AccreValidUntil = AccreValidity;
                                suppliedAdd.AccreApproveBy = AccreApproveBy;
                                suppliedAdd.AccreMOA = AccreMOA;
                                BOSSDB.Tbl_FMSupplier.Add(suppliedAdd);
                                BOSSDB.SaveChanges();
                                isExist = "false";
                                break;

                            case 2:
                                supplierRecord.CompanyName = CompanyName;
                                supplierRecord.ProductServices = ProductServices;
                                supplierRecord.Address = Address;
                                supplierRecord.TaxType = TaxType;
                                //supplierRecord.DTIRegNo = DTIRegNo;
                                //supplierRecord.CDARegistry = CDARegistry;
                                supplierRecord.FaxNo = FaxNo;
                                supplierRecord.TelNo = TelNo;
                                supplierRecord.TIN = TIN;
                                supplierRecord.MFName = MFName;
                                supplierRecord.MFAddress = MFAddress;
                                supplierRecord.MFContactNo = MFContactNo;
                                supplierRecord.AccreNumber = AccreNumber;
                                supplierRecord.AccreDate = AccreDate;
                                supplierRecord.AccreValidUntil = AccreValidity;
                                supplierRecord.AccreApproveBy = AccreApproveBy;
                                supplierRecord.AccreMOA = AccreMOA;

                                BOSSDB.Entry(supplierRecord);
                                BOSSDB.SaveChanges();
                                isExist = "justUpdate";
                                break;
                        }
                        break;
                    default:
                        isExist = "true";
                        break;
                }
            }
            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new { isExist = isExist }
            };
        }
        public ActionResult DeleteSupplier(int PrimaryID)
        {
            Tbl_FMSupplier supplier = (from a in BOSSDB.Tbl_FMSupplier where a.SupplierID == PrimaryID select a).FirstOrDefault();
           
            var confirmDelete = "";
            if (supplier != null)
            {
                confirmDelete = "false";
            }
            var result = new { confirmDelete = confirmDelete };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConfirmDeleteSupplier(int PrimaryID)
        {
            Tbl_FMSupplier supplier = (from a in BOSSDB.Tbl_FMSupplier where a.SupplierID == PrimaryID select a).FirstOrDefault();

            BOSSDB.Tbl_FMSupplier.Remove(supplier);
            BOSSDB.SaveChanges();

            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}