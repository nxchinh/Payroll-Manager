using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll_Manager.Areas.Admin.Models.VM_PurchaseOrder
{
    public class PurchaseOrderIndexViewModel
    {
        public int PurchaseOrderId { get; set; }
        public string OrderDate { get; set; }
        public double price { get; set; }
        public string status { get; set; }


    }
}
