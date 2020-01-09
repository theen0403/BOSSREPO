
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BOSS.Models.FMmodels.FMSupplierModels
{
    public class SupplierModel
    {
        BOSSEFConnectionString BOSSDB = new BOSSEFConnectionString();
        public SupplierModel()
        {
            getSupplierList = new List<SupplierList>();
            SupplierList = new SupplierList();
        }
        public int ActionID { get; set; }
        public List<SupplierList> getSupplierList { get; set; }
        public SupplierList SupplierList { get; set; }
    }
    public class SupplierList
    {
        public int SupplierID { get; set; }
        [Required(ErrorMessage = "Company name is required.")]
        public string CompanyName { get; set; }
        //[Required(ErrorMessage = "Product/services is required.")]
        public string ProductServices { get; set; }
        [Required(ErrorMessage = "Company address is required.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Tax type is required.")]
        public string TaxType { get; set; }
        //[Required]
        //public string DTIRegNo { get; set; }
        //[Required]
        //public string CDARegistry { get; set; }
        //[Required(ErrorMessage = "Fax number is required.")]
        public string FaxNo { get; set; }
        [Required(ErrorMessage = "Telephone number is required.")]
        public string TelNo { get; set; }
       // [Required(ErrorMessage = "TIN is required.")]
        public string TIN { get; set; }
        public string MFName { get; set; }
        public string MFAddress { get; set; }
        public string MFContactNo { get; set; }
        public string AccreNumber { get; set; }
        public string AccreDate { get; set; }
        public string AccreValidUntil { get; set; }
        public string AccreValidity { get; set; }
        //[Required(ErrorMessage = "Name of approver is required.")]
        public string AccreApproveBy { get; set; }
      //  [Required(ErrorMessage = "MOA is required.")]
        public string AccreMOA { get; set; }
    }
}