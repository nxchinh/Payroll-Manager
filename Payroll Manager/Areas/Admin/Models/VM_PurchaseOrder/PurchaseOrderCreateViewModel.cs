using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll_Manager.Areas.Admin.Models.VM_PurchaseOrder
{
    public class PurchaseOrderCreateViewModel
    {
        public int PurchaseOrderId { get; set; }
        public string MaGiaoDich { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsReceived { get; set; }
    }
}
