using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Payroll_Manager.Entity.ModelForUser;

namespace Payroll_Manager.Areas.Admin.Models.VM_Category
{
    public class CategoryDetailViewModel
    {
        public int Total { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Active { get; set; }
        public IEnumerable<Inventory> Inventories { get; set; }
    }
}
