using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll_Manager.Areas.Admin.Models.VM_PurchaseOrderItem
{
    public class PurchaseOrderItemIndexViewModel
    {
        public int PoItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int Received { get; set; }
        public int PurchaseOrderId { get; set; }
        public double TotalPrice { get; set; }
        public int QtyReturned => Quantity - Received;
    }
}
