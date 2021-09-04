using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll_Manager.Areas.Admin.Models.VM_Sale
{
    public class SaleIndexViewModel
    { 
        public int SaleId { get; set; }
        public int? CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public System.DateTime SaleDate { get; set; }
        public string SaleDateString => SaleDate.Date.ToShortDateString();
        public double TotalSalePrice { get; set; }
        public int TotalSaleItems { get; set; }

        public int Status { get; set; }
    }
}
