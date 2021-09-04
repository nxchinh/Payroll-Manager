using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Payroll_Manager.Entity.ModelForUser;

namespace Payroll_Manager.Areas.Admin.Models.VM_Category
{
    public class CategorySearchViewModel
    {
        public int Total { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public int CategoryId { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }

        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Inventory> Inventories { get; set; }
    }
}
