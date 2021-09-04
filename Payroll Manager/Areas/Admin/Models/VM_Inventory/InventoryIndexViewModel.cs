using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Payroll_Manager.Entity;
using Payroll_Manager.Entity.ModelForUser;

namespace Payroll_Manager.Areas.Admin.Models.VM_Inventory
{
    public class InventoryIndexViewModel
    {
        public IEnumerable<Inventory> Inventories { get; set; }
        public Inventory InvenDetail { get; set; }
    }
}
