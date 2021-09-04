using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll_Manager.Areas.Admin.Models.VM_Sale
{
    public class SaleCreateViewModel
    {
        public int SaleId { get; set; }
        [Display(Name = "Khách hàng")]
        public int? CustomerId { get; set; }
        [Display(Name = "Nhân viên")]
        public int EmployeeId { get; set; }
        public System.DateTime SaleDate { get; set; }
    }
}
