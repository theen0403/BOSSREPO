using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class PreparationLBPF1Controller : Controller
    {
        // GET: PreparationLBPF1
        [Authorize]
        public ActionResult LBPF1View()
        {
            return View();
        }
        //Get Table list of State Of Receipt Modal
        public ActionResult GetSORlistDT()
        {
            return PartialView("_TableStateOfReceipt");
        }
        //Get Add Account State Of Receipt Modal
        public ActionResult GetViewAddAccountSOR(int ID)
        {
            return PartialView("_ViewAddAccountStateofReceipt");
        }
        //Get Table list of Receipts/Income Modal
        public ActionResult GetRIlistDT()
        {
            return PartialView("_TableReceiptsIncome");
        }
        //Get Add Account Receipts/Income Modal
        public ActionResult GetViewAddAccountRI(int ID)
        {
            return PartialView("_ViewAddAccountReceiptIncome");
        }
        //Get Add Account Expenditures Modal
        public ActionResult GetViewAddAccountExpenditure(int ID)
        {
            return PartialView("_ViewAddAccountExpenditure");
        }
    }
}